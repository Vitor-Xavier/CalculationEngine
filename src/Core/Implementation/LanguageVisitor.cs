using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Common.Enums;
using Common.Exceptions;
using Common.Extensions;
using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Core.Implementation
{
    public class LanguageVisitor : LanguageBaseVisitor<GenericValueLanguage>
    {
        private readonly IDictionary<string, GenericValueLanguage> _memory;

        private readonly IDictionary<string, GenericValueLanguage> _memoryLocal = new Dictionary<string, GenericValueLanguage>();

        private readonly IDictionary<string, GenericValueLanguage> _memoryLocalList = new Dictionary<string, GenericValueLanguage>();

        private GenericValueLanguage _switchValue;

        public LanguageVisitor(IDictionary<string, GenericValueLanguage> memory)
        {
            _memory = memory ?? new Dictionary<string, GenericValueLanguage>();
        }

        public override GenericValueLanguage VisitVarPrimaryEntity([NotNull] LanguageParser.VarPrimaryEntityContext context)
        {
            string id = context.VAR_PRIMARY().GetText();
            _memory.TryGetValue(id, out GenericValueLanguage retono);
            return retono;
        }

        #region entity VAR_PRIMARY

        /// <summary>
        /// Metodo retorna um valor da lista _MEMORY ou _MEMORYGLOBAL
        /// Essas variaveis na formula inicia com @
        /// Tambem retorna Formulas anteriores
        /// VAR_PRIMARY DOT IDENTIFIER #varMemoryValue
        /// </summary>
        /// <example>
        /// @Fisico.Crc
        /// @Fisico.IdCep
        /// </example>
        /// <returns>
        /// GenericValueLanguage
        /// </returns>
        public override GenericValueLanguage VisitVarMemoryValue([NotNull] LanguageParser.VarMemoryValueContext context)
        {
            string identifier = context.VAR_PRIMARY().GetText();
            string objectKey = context.IDENTIFIER().GetText();

            string identifierTotal = string.Concat(identifier, ".", objectKey);

            // Busca Memory Formulas
            if (TryGetMemory(identifier, EnumMemory.Memory, out GenericValueLanguage valueMemory) && valueMemory.IsDictionaryStringGeneric())
            {
                valueMemory.AsDictionaryStringGeneric().TryGetValue(objectKey, out GenericValueLanguage result);
                return result;
            }

            // Busca Memory do banco DAdos
            TryGetMemory(identifierTotal, EnumMemory.Memory, out valueMemory);
            return valueMemory;
        }

        /// <summary>
        /// Metodo retorna uma lista da lista _MEMORY
        /// Inicia com @
        /// VAR_PRIMARY LBRACKET (IDENTIFIER | number_integer) RBRACKET DOT IDENTIFIER #listMemoryGlobalValue
        /// </summary>
        /// <example>
        /// @FisicoOutros[0].Crc
        /// @FisicoOutros[1].IdCep
        /// </example>
        /// <returns>
        /// GenericValueLanguage
        /// </returns>
        public override GenericValueLanguage VisitListMemoryGlobalValue([NotNull] LanguageParser.ListMemoryGlobalValueContext context)
        {
            string nameArray = context.VAR_PRIMARY().GetText();
            int index = GetIndexArray(context.number_integer()?.GetText(), context.IDENTIFIER(0)?.GetText(), context);
            string propertyArray = context.number_integer() is null ? context.IDENTIFIER(1)?.GetText() : context.IDENTIFIER(0)?.GetText();

            if (TryGetMemory(nameArray, EnumMemory.Memory, out GenericValueLanguage valueMemory) && valueMemory.IsDictionaryIntDictionaryStringGeneric())
            {
                return GetListValue(valueMemory, index, propertyArray, context);
            }
            return GenericValueLanguage.NULL;
        }

        /// <summary>
        /// Metodo retorna uma lista/valor de Formulas Anteriores salvar no _MEMORY
        /// Inicia com @Roteiro...
        /// VAR_PRIMARY DOT IDENTIFIER LBRACKET (IDENTIFIER | number_integer) RBRACKET DOT IDENTIFIER #listMemoryValue
        /// </summary>
        /// <example>
        /// @Roteiro.Formula01
        /// @Roteiro.Formula01[0].Nome
        /// </example>
        /// <returns>
        /// GenericValueLanguage
        /// </returns>
        public override GenericValueLanguage VisitListMemoryValue([NotNull] LanguageParser.ListMemoryValueContext context)
        {
            string nameArray = context.VAR_PRIMARY().GetText();
            string propertyArray = context.IDENTIFIER(0).GetText();
            int index = GetIndexArray(context.number_integer()?.GetText(), context.IDENTIFIER(1)?.GetText(), context);
            string propertyArrayKey = context.number_integer() is null ? context.IDENTIFIER(2)?.GetText() : context.IDENTIFIER(1)?.GetText();

            if (TryGetMemory(nameArray, EnumMemory.Memory, out GenericValueLanguage listMemory) && listMemory.IsDictionaryStringGeneric())
            {
                var listMemoryProperty = listMemory.AsDictionaryStringGeneric()[propertyArray];
                return GetListValue(listMemoryProperty, index, propertyArrayKey, context);
            }
            else throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + nameArray.Length, $"Variável '{nameArray}' não encontrada.", nameArray);
        }

        #endregion

        /// <summary>
        /// Método retorno List
        /// Usado para retorna lista criada pelo Usuario.
        /// </summary>
        /// 
        /// <example>
        /// lista listaTeste[0] = arrayTest04[0].Propriedade01;
        /// </example>
        /// 
        /// <param name="context">
        /// context contém acesso as tokens definidos na language G4 da #VariableArrayEntity
        /// </param>
        /// 
        /// <returns>
        /// Tipo GenericValueLanguage com o valor ou array definido.
        /// </returns>
        /// 
        private GenericValueLanguage GetListValue(GenericValueLanguage listProperty, int index, string propertyArrayKey, ParserRuleContext context)
        {

            // Retorna valor de Dictionary Int com Dictionary String,GenericValue dentro
            // Exemplo
            // var value = array[0].propriedade1;
            if (listProperty.IsDictionaryIntDictionaryStringGeneric())
            {
                if (!listProperty.AsDictionaryIntDictionaryStringGeneric().TryGetValue(index, out IDictionary<string, GenericValueLanguage> indexDictonaryStringGeneric))
                    throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Índice '{index}' excede o limite da lista.", context.GetText());

                // Retorna todas as propriedades do index
                if (string.IsNullOrEmpty(propertyArrayKey))
                {
                    IDictionary<int, IDictionary<string, GenericValueLanguage>> createDictIntDictStringGeneric = new Dictionary<int, IDictionary<string, GenericValueLanguage>>
                    {
                        { index, indexDictonaryStringGeneric }
                    };

                    // TODO: Verificar
                    //return new GenericValueLanguage(indexDictonaryStringGeneric);
                    return new GenericValueLanguage(createDictIntDictStringGeneric);
                }
                // Caso nao tenha propriedade exibe erro, se nao retorna valor
                else
                {
                    indexDictonaryStringGeneric.TryGetValue(propertyArrayKey, out GenericValueLanguage objectProperty);
                    return objectProperty;
                }

            }
            else
            {
                return GenericValueLanguage.Empty;
            }
        }

        #region Sum
        public override GenericValueLanguage VisitSumDatabase([NotNull] LanguageParser.SumDatabaseContext context)
        {
            string varPrimary = context.VAR_PRIMARY().GetText();
            string identifier01 = context.IDENTIFIER(0).GetText();
            string identifier02 = context.IDENTIFIER(1)?.GetText();

            return OperatorFunctionDataBase(varPrimary, identifier01, identifier02, EnumOperation.Sum);
        }

        // Lista Identifer
        public override GenericValueLanguage VisitSumListLocal([NotNull] LanguageParser.SumListLocalContext context)
        {
            string identifier01 = context.IDENTIFIER(0).GetText();
            string identifier02 = context.IDENTIFIER(1).GetText();

            return OperatorFunctionListLocal(identifier01, identifier02, EnumOperation.Sum);
        }

        public override GenericValueLanguage VisitSumVariable([NotNull] LanguageParser.SumVariableContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            return OperatorFunctionVariable(identifier, EnumOperation.Sum, context);
        }
        #endregion

        #region Max
        public override GenericValueLanguage VisitMaxDatabase([NotNull] LanguageParser.MaxDatabaseContext context)
        {
            string varPrimary = context.VAR_PRIMARY().GetText();
            string identifier01 = context.IDENTIFIER(0).GetText();
            string identifier02 = context.IDENTIFIER(1)?.GetText();

            return OperatorFunctionDataBase(varPrimary, identifier01, identifier02, EnumOperation.Max);
        }

        // Lista Identifer
        public override GenericValueLanguage VisitMaxListLocal([NotNull] LanguageParser.MaxListLocalContext context)
        {
            string identifier01 = context.IDENTIFIER(0).GetText();
            string identifier02 = context.IDENTIFIER(1).GetText();

            return OperatorFunctionListLocal(identifier01, identifier02, EnumOperation.Max);
        }

        public override GenericValueLanguage VisitMaxVariable([NotNull] LanguageParser.MaxVariableContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            return OperatorFunctionVariable(identifier, EnumOperation.Max, context);
        }
        #endregion

        #region Min
        public override GenericValueLanguage VisitMinDatabase([NotNull] LanguageParser.MinDatabaseContext context)
        {
            string varPrimary = context.VAR_PRIMARY().GetText();
            string identifier01 = context.IDENTIFIER(0).GetText();
            string identifier02 = context.IDENTIFIER(1)?.GetText();

            return OperatorFunctionDataBase(varPrimary, identifier01, identifier02, EnumOperation.Min);
        }

        // Lista Identifer
        public override GenericValueLanguage VisitMinListLocal([NotNull] LanguageParser.MinListLocalContext context)
        {
            string identifier01 = context.IDENTIFIER(0).GetText();
            string identifier02 = context.IDENTIFIER(1).GetText();

            return OperatorFunctionListLocal(identifier01, identifier02, EnumOperation.Min);
        }

        public override GenericValueLanguage VisitMinVariable([NotNull] LanguageParser.MinVariableContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            return OperatorFunctionVariable(identifier, EnumOperation.Min, context);
        }
        #endregion

        #region Average
        public override GenericValueLanguage VisitAverageDatabase([NotNull] LanguageParser.AverageDatabaseContext context)
        {
            string varPrimary = context.VAR_PRIMARY().GetText();
            string identifier01 = context.IDENTIFIER(0).GetText();
            string identifier02 = context.IDENTIFIER(1)?.GetText();

            return OperatorFunctionDataBase(varPrimary, identifier01, identifier02, EnumOperation.Average);
        }

        // Lista Identifer
        public override GenericValueLanguage VisitAverageListLocal([NotNull] LanguageParser.AverageListLocalContext context)
        {
            string identifier01 = context.IDENTIFIER(0).GetText();
            string identifier02 = context.IDENTIFIER(1).GetText();

            return OperatorFunctionListLocal(identifier01, identifier02, EnumOperation.Average);
        }

        public override GenericValueLanguage VisitAverageVariable([NotNull] LanguageParser.AverageVariableContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            return OperatorFunctionVariable(identifier, EnumOperation.Average, context);
        }
        #endregion

        #region Length
        public override GenericValueLanguage VisitLengthDatabase([NotNull] LanguageParser.LengthDatabaseContext context)
        {
            string varPrimary = context.VAR_PRIMARY().GetText();
            string identifier = context.IDENTIFIER()?.GetText();


            if (string.IsNullOrEmpty(identifier))
            {
                if (TryGetMemory(varPrimary, EnumMemory.Memory, out GenericValueLanguage currentValue) && currentValue.IsDictionaryIntDictionaryStringGeneric())
                {
                    var listIndexPropery = (currentValue.AsDictionaryIntDictionaryStringGeneric());
                    var soma = listIndexPropery.Count;
                    return new GenericValueLanguage(soma);
                }
            }
            else
            {
                if (!TryGetMemory(varPrimary, EnumMemory.Memory, out GenericValueLanguage value))
                    throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + identifier.Length, $"Varíavel '{varPrimary + "." + identifier}' não foi declarada", identifier);

                if (value.IsDictionaryStringGeneric())
                {
                    IDictionary<string, GenericValueLanguage> resultMemory = value.AsDictionaryStringGeneric();

                    if (resultMemory.TryGetValue(identifier, out GenericValueLanguage resultIdentifier01) && resultIdentifier01.IsDictionaryIntDictionaryStringGeneric())
                    {
                        return new GenericValueLanguage(resultIdentifier01.AsDictionaryIntDictionaryStringGeneric().Count);
                    }

                    return GenericValueLanguage.Empty;
                }

            }

            return GenericValueLanguage.Empty;
        }

        // Lista Identifer
        public override GenericValueLanguage VisitLengthVariable([NotNull] LanguageParser.LengthVariableContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            if (!TryGetMemory(identifier, EnumMemory.MemoryLocalOrMemoryLocalList, out GenericValueLanguage currentValue))
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + identifier.Length, $"Varíavel '{identifier}' não foi declarada", identifier);

            if (currentValue.IsDictionaryIntDictionaryStringGeneric())
            {
                var listIndexPropery = (currentValue.AsDictionaryIntDictionaryStringGeneric());
                var soma = listIndexPropery.Count;
                return new GenericValueLanguage(soma);
            }
            return GenericValueLanguage.Empty;
        }
        #endregion

        private GenericValueLanguage OperatorFunctionDataBase(string varPrimary, string identifier01, string identifier02, EnumOperation enumOperator)
        {
            if (!TryGetMemory(varPrimary, EnumMemory.Memory, out GenericValueLanguage value))
                return GenericValueLanguage.Empty;

            if (value.IsDictionaryStringGeneric())
            {
                IDictionary<string, GenericValueLanguage> resultMemory = value.AsDictionaryStringGeneric();

                if (resultMemory.TryGetValue(identifier01, out GenericValueLanguage resultIdentifier01) && resultIdentifier01.IsDictionaryIntDictionaryStringGeneric())
                {
                    return OperatorDictionaryIntDictionaryStringGeneric(identifier02, enumOperator, resultIdentifier01);
                }
                else if (resultMemory.TryGetValue(identifier01, out resultIdentifier01))
                {
                    return resultIdentifier01;
                }

                return GenericValueLanguage.Empty;
            }
            else if (value.IsDictionaryIntDictionaryStringGeneric())
            {
                return OperatorDictionaryIntDictionaryStringGeneric(identifier01, enumOperator, value);
            }
            else
                return GenericValueLanguage.Empty;
        }

        private GenericValueLanguage OperatorFunctionListLocal(string identifier01, string identifier02, EnumOperation enumOperator)
        {
            if (!TryGetMemory(identifier01, EnumMemory.MemoryLocalList, out GenericValueLanguage value))
                return GenericValueLanguage.Empty;

            if (value.IsDictionaryIntDictionaryStringGeneric())
            {
                return OperatorDictionaryIntDictionaryStringGeneric(identifier02, enumOperator, value);
            }
            else
                return GenericValueLanguage.Empty;
        }

        private GenericValueLanguage OperatorFunctionVariable(string identifier, EnumOperation enumOperator, ParserRuleContext context)
        {
            if (!TryGetMemory(identifier, EnumMemory.MemoryLocalOrMemoryLocalList, out GenericValueLanguage currentValue))
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + identifier.Length, $"Varíavel '{identifier}' não foi declarada", identifier);

            if (currentValue.IsDictionaryIntGeneric())
            {
                var listIndexPropery = (currentValue.AsDictionaryIntGeneric());
                var soma = listIndexPropery.Select(p => p.Value.TryDecimal());
                currentValue = new GenericValueLanguage(Operador(soma, enumOperator));
            }
            return currentValue;
        }

        /// <summary>
        /// Recebe Identificador, operador e valor do tipo IDictionary<int, IDictionary<string, GenericValueLanguage>>
        /// Valida se o tipo esta correto
        /// Extrai o tipo IEnumerable para que contem as functions do enumOperator
        /// Passa a IEnumerable e o enumOperator para obter o valor da funcao
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="enumOperator"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private GenericValueLanguage OperatorDictionaryIntDictionaryStringGeneric(string identifier, EnumOperation enumOperator, GenericValueLanguage value)
        {
            IDictionary<int, IDictionary<string, GenericValueLanguage>> dictResultIdentifier01 = value.AsDictionaryIntDictionaryStringGeneric();

            if (dictResultIdentifier01 == null)
                return GenericValueLanguage.Empty;

            IEnumerable<decimal> list = ExtractIEnumrable(dictResultIdentifier01, identifier);
            return new GenericValueLanguage(Operador(list, enumOperator));
        }

        /// <summary>
        /// Busca na memória o identificador informado.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="memoryType">Tipo de memória a ser consultada</param>
        /// <param name="value">Valor extraido da memória, ou valor padrão</param>
        /// <returns>Sucesso da busca</returns>
        private bool TryGetMemory(string identifier, EnumMemory memoryType, out GenericValueLanguage value) => memoryType switch
        {
            EnumMemory.MemoryLocalList => _memoryLocalList.TryGetValue(identifier, out value),
            EnumMemory.MemoryLocal => _memoryLocal.TryGetValue(identifier, out value),
            EnumMemory.Memory => _memory.TryGetValue(identifier, out value),
            EnumMemory.MemoryLocalOrMemoryLocalList => _memoryLocal.TryGetValue(identifier, out value) ? true : _memoryLocalList.TryGetValue(identifier, out value),
            _ => throw new InvalidOperationException($"Memory error.")
        };

        /// <summary>
        /// Realiza operação em lista.
        /// Executa a funcao da lista executando a funcao do EnumOperation
        /// </summary>
        /// <param name="list">Lista de decimais</param>
        /// <param name="op">Operação</param>
        /// <returns></returns>
        private decimal Operador(IEnumerable<decimal> list, EnumOperation op) => op switch
        {
            _ when list.Count() == 0 => 0,
            EnumOperation.Sum => list.Sum(),
            EnumOperation.Min => list.Min(),
            EnumOperation.Max => list.Max(),
            EnumOperation.Average => list.Average(),
            EnumOperation.Count => list.Count(),
            EnumOperation.Length => list.Count(),
            _ => throw new InvalidOperationException($"Operador '{op}' nao reconhecido."),
        };

        /// <summary>
        /// Extrai uma lista IDictionary<int, IDictionary<string, GenericValueLanguage>>
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        private static IEnumerable<decimal> ExtractIEnumrable(IDictionary<int, IDictionary<string, GenericValueLanguage>> dictionary, string identifier) =>
            dictionary.Select(x => x.Value.ToList().Where(p => p.Key == identifier).Select(p => p.Value.TryDecimal()).FirstOrDefault());

        public override GenericValueLanguage VisitAbsFunction([NotNull] LanguageParser.AbsFunctionContext context)
        {
            var genericValue = Visit(context.arithmetic_expression());

            if (genericValue.Value is null)
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Não foi possível encontrar o valor absoluto da entrada {genericValue.Value}", context.GetText());
            
            return new GenericValueLanguage(Math.Abs(genericValue.AsDecimal()));
        }

        public override GenericValueLanguage VisitSumIfFunction([NotNull] LanguageParser.SumIfFunctionContext context)
        {
            string objectKey = context.VAR_PRIMARY().GetText();
            decimal sum = 0m;
            string propertyArrayKey = context.IDENTIFIER(0).GetText();
            string propertyArrayKey2 = context.IDENTIFIER(1)?.GetText();

            TryGetMemory(objectKey, EnumMemory.Memory, out GenericValueLanguage value);

            if (value.IsDictionaryIntDictionaryStringGeneric())
            {

                string op = context.comparison_operator().GetText();
                for (var i = 0; i < value.AsDictionaryIntDictionaryStringGeneric().Count; i++)
                {
                    value.AsDictionaryIntDictionaryStringGeneric().TryGetValue(i, out IDictionary<string, GenericValueLanguage> item);
                    var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out GenericValueLanguage leftValue) ? leftValue : Visit(context.arithmetic_expression(0));
                    var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out GenericValueLanguage rightValue) ? rightValue : Visit(context.arithmetic_expression(1));

                    if (CompareGenericValues(left, right, op, context).AsBoolean())
                        sum += !item[propertyArrayKey].IsNull() ? decimal.Parse(item[propertyArrayKey].IsNull() ? "0" : item[propertyArrayKey].Value.ToString()) : 0m;
                }
            }
            else if (value.IsDictionaryStringGeneric())
            {
                value.AsDictionaryStringGeneric().TryGetValue(propertyArrayKey, out GenericValueLanguage result);

                if (result.IsDictionaryIntDictionaryStringGeneric())
                {
                    string op = context.comparison_operator().GetText();
                    for (var i = 0; i < result.AsDictionaryIntDictionaryStringGeneric().Count; i++)
                    {
                        result.AsDictionaryIntDictionaryStringGeneric().TryGetValue(i, out IDictionary<string, GenericValueLanguage> item);
                        var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out GenericValueLanguage leftValue) ? leftValue : Visit(context.arithmetic_expression(0));
                        var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out GenericValueLanguage rightValue) ? rightValue : Visit(context.arithmetic_expression(1));

                        if (CompareGenericValues(left, right, op, context).AsBoolean())
                            sum += !item[propertyArrayKey2].IsNull() ? decimal.Parse(item[propertyArrayKey2].IsNull() ? "0" : item[propertyArrayKey2].Value.ToString()) : 0m;
                    }
                }

            }

            return new GenericValueLanguage(sum);
        }

        public override GenericValueLanguage VisitSumIfListLocal([NotNull] LanguageParser.SumIfListLocalContext context)
        {
            string identifier01 = context.IDENTIFIER(0).GetText();
            decimal sum = 0m;

            if (TryGetMemory(identifier01, EnumMemory.MemoryLocalOrMemoryLocalList, out GenericValueLanguage value) && value.IsDictionaryIntDictionaryStringGeneric())
            {
                string propertyArrayKey = context.IDENTIFIER(1).GetText();
                string op = context.comparison_operator().GetText();
                for (var i = 0; i < value.AsDictionaryIntDictionaryStringGeneric().Count; i++)
                {
                    value.AsDictionaryIntDictionaryStringGeneric().TryGetValue(i, out IDictionary<string, GenericValueLanguage> item);
                    var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out GenericValueLanguage leftValue) ? leftValue : Visit(context.arithmetic_expression(0));
                    var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out GenericValueLanguage rightValue) ? rightValue : Visit(context.arithmetic_expression(1));

                    if (CompareGenericValues(left, right, op, context).AsBoolean())
                        sum += !item[propertyArrayKey].IsNull() ? decimal.Parse(item[propertyArrayKey].IsNull() ? "0" : item[propertyArrayKey].Value.ToString()) : 0m;
                }
            }

            return new GenericValueLanguage(sum);
        }


        public override GenericValueLanguage VisitCountIfFunction([NotNull] LanguageParser.CountIfFunctionContext context)
        {
            string objectKey = context.VAR_PRIMARY().GetText();
            string propertyArrayKey = context.IDENTIFIER()?.GetText();

            decimal count = 0;

            TryGetMemory(objectKey, EnumMemory.Memory, out GenericValueLanguage value);
            if (value.IsDictionaryIntDictionaryStringGeneric())
            {

                string op = context.comparison_operator().GetText();
                for (var i = 0; i < value.AsDictionaryIntDictionaryStringGeneric().Count; i++)
                {
                    value.AsDictionaryIntDictionaryStringGeneric().TryGetValue(i, out IDictionary<string, GenericValueLanguage> item);
                    var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out GenericValueLanguage leftValue) ? leftValue : Visit(context.arithmetic_expression(0));
                    var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out GenericValueLanguage rightValue) ? rightValue : Visit(context.arithmetic_expression(1));

                    if (CompareGenericValues(left, right, op, context).AsBoolean())
                        count++;
                }
            }
            else if (value.IsDictionaryStringGeneric())
            {

                string op = context.comparison_operator().GetText();
                value.AsDictionaryStringGeneric().TryGetValue(propertyArrayKey, out GenericValueLanguage result);
                for (var i = 0; i < result.AsDictionaryIntDictionaryStringGeneric().Count; i++)
                {
                    result.AsDictionaryIntDictionaryStringGeneric().TryGetValue(i, out IDictionary<string, GenericValueLanguage> item);
                    var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out GenericValueLanguage leftValue) ? leftValue : Visit(context.arithmetic_expression(0));
                    var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out GenericValueLanguage rightValue) ? rightValue : Visit(context.arithmetic_expression(1));

                    if (CompareGenericValues(left, right, op, context).AsBoolean())
                        count++;
                }
            }

            return new GenericValueLanguage(count);
        }

        public override GenericValueLanguage VisitCountIfListLocal([NotNull] LanguageParser.CountIfListLocalContext context)
        {
            string objectKey = context.IDENTIFIER().GetText();
            decimal count = 0;

            TryGetMemory(objectKey, EnumMemory.MemoryLocalOrMemoryLocalList, out GenericValueLanguage value);
            if (value.IsDictionaryIntDictionaryStringGeneric())
            {

                string op = context.comparison_operator().GetText();
                for (var i = 0; i < value.AsDictionaryIntDictionaryStringGeneric().Count; i++)
                {
                    value.AsDictionaryIntDictionaryStringGeneric().TryGetValue(i, out IDictionary<string, GenericValueLanguage> item);
                    var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out GenericValueLanguage leftValue) ? leftValue : Visit(context.arithmetic_expression(0));
                    var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out GenericValueLanguage rightValue) ? rightValue : Visit(context.arithmetic_expression(1));

                    if (CompareGenericValues(left, right, op, context).AsBoolean())
                        count++;
                }
            }

            return new GenericValueLanguage(count);
        }

        public override GenericValueLanguage VisitConditional([NotNull] LanguageParser.ConditionalContext context)
        {
            if (Visit(context.if_expression()).AsBoolean())
            {
                if (context.then_block() != null)
                    Visit(context.then_block());
            }
            else
            {
                if (context.else_block() != null)
                    Visit(context.else_block());
            }

            return GenericValueLanguage.NULL;
        }

        public override GenericValueLanguage VisitAndExpression([NotNull] LanguageParser.AndExpressionContext context)
        {
            var left = bool.Parse(Visit(context.if_expression(0)).Value.ToString());
            var right = bool.Parse(Visit(context.if_expression(1)).Value.ToString());

            return new GenericValueLanguage(left && right);
        }

        public override GenericValueLanguage VisitOrExpression([NotNull] LanguageParser.OrExpressionContext context)
        {
            var left = bool.Parse(Visit(context.if_expression(0)).Value.ToString());
            var right = bool.Parse(Visit(context.if_expression(1)).Value.ToString());

            return new GenericValueLanguage(left || right);
        }

        public override GenericValueLanguage VisitThenBlock([NotNull] LanguageParser.ThenBlockContext context)
        {
            foreach (var ruleBlock in context.rule_block())
            {
                if (Visit(ruleBlock).Value is SpecialValue specialValue && specialValue == SpecialValue.Break)
                    return new GenericValueLanguage(specialValue);
            }

            return GenericValueLanguage.NULL;
        }

        public override GenericValueLanguage VisitElseBlock([NotNull] LanguageParser.ElseBlockContext context)
        {
            foreach (var ruleBlock in context.rule_block())
            {
                if (Visit(ruleBlock).Value is SpecialValue specialValue && specialValue == SpecialValue.Break)
                    return new GenericValueLanguage(specialValue);
            }

            return GenericValueLanguage.NULL;
        }

        public override GenericValueLanguage VisitParenthesisIfExpression([NotNull] LanguageParser.ParenthesisIfExpressionContext context) =>
            Visit(context.if_expression());

        public override GenericValueLanguage VisitIfEntity([NotNull] LanguageParser.IfEntityContext context)
        {
            var entity = Visit(context.entity());
            return entity.Value is bool ? entity : new GenericValueLanguage(entity.Value != null);
        }

        public override GenericValueLanguage VisitNotIfEntity([NotNull] LanguageParser.NotIfEntityContext context)
        {
            var entity = Visit(context.entity());
            return new GenericValueLanguage(entity.Value is bool ? !entity.AsBoolean() : entity.Value is null);
        }

        public override GenericValueLanguage VisitComparisonExpression([NotNull] LanguageParser.ComparisonExpressionContext context)
        {
            var left = Visit(context.arithmetic_expression(0));
            var right = Visit(context.arithmetic_expression(1));
            string op = context.comparison_operator().GetText();

            return CompareGenericValues(left, right, op, context);
        }

        public override GenericValueLanguage VisitNotParenthesisIfExpression([NotNull] LanguageParser.NotParenthesisIfExpressionContext context) =>
            new GenericValueLanguage(!Visit(context.if_expression()).AsBoolean());

        public override GenericValueLanguage VisitParenthesisComparisonExpression([NotNull] LanguageParser.ParenthesisComparisonExpressionContext context) =>
            Visit(context.comparison_expression());

        public override GenericValueLanguage VisitSwitchExpression([NotNull] LanguageParser.SwitchExpressionContext context)
        {
            _switchValue = Visit(context.arithmetic_expression());
            foreach (var caseStatement in context.case_statement())
            {
                if (Visit(caseStatement).Value is SpecialValue specialValue && specialValue == SpecialValue.Break)
                    return GenericValueLanguage.NULL;
            }

            if (context.default_statement() != null)
                Visit(context.default_statement());

            return GenericValueLanguage.NULL;
        }

        public override GenericValueLanguage VisitCaseStatement([NotNull] LanguageParser.CaseStatementContext context)
        {
            if (CompareGenericValues(_switchValue, Visit(context.arithmetic_expression()), "==", context).AsBoolean())
            {
                foreach (var ruleBlock in context.rule_block())
                {
                    if (Visit(ruleBlock).Value is SpecialValue specialValue && specialValue == SpecialValue.Break)
                        return new GenericValueLanguage(specialValue);
                }
            }

            return GenericValueLanguage.NULL;
        }

        public override GenericValueLanguage VisitDefaultStatement([NotNull] LanguageParser.DefaultStatementContext context)
        {
            foreach (var ruleBlock in context.rule_block())
            {
                if (Visit(ruleBlock).Value is SpecialValue specialValue && specialValue == SpecialValue.Break)
                    return new GenericValueLanguage(specialValue);
            }

            return GenericValueLanguage.NULL;
        }

        public override GenericValueLanguage VisitPlusExpression(LanguageParser.PlusExpressionContext context)
        {
            var left = Visit(context.arithmetic_expression(0));
            var right = Visit(context.arithmetic_expression(1));

            return left + right;
        }

        public override GenericValueLanguage VisitMinusExpression([NotNull] LanguageParser.MinusExpressionContext context)
        {
            var left = Visit(context.arithmetic_expression(0));
            var right = Visit(context.arithmetic_expression(1));

            return left - right;
        }

        public override GenericValueLanguage VisitMultExpression([NotNull] LanguageParser.MultExpressionContext context)
        {
            var left = Visit(context.arithmetic_expression(0));
            var right = Visit(context.arithmetic_expression(1));

            return left * right;
        }

        public override GenericValueLanguage VisitDivExpression([NotNull] LanguageParser.DivExpressionContext context)
        {
            var left = Visit(context.arithmetic_expression(0));
            var right = Visit(context.arithmetic_expression(1));

            if (right.AsDecimal() == 0)
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Não é possivel dividir por zero.", context.GetText());

            return left / right;
        }

        public override GenericValueLanguage VisitPowExpression([NotNull] LanguageParser.PowExpressionContext context)
        {
            var left = Visit(context.arithmetic_expression(0));
            var right = Visit(context.arithmetic_expression(1));

            if (left.IsNumeric && right.IsNumeric)
                return new GenericValueLanguage(Math.Pow((double)left, (double)right));
            throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Não é possível elevar o valor {left} a {right}", context.GetText());
        }

        public override GenericValueLanguage VisitSqrtFunction([NotNull] LanguageParser.SqrtFunctionContext context)
        {
            var number = Visit(context.arithmetic_expression());

            if (number.IsNumeric)
                return new GenericValueLanguage(Math.Sqrt((double)number));
            throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Não foi possível calcular a raiz de {number}", context.GetText());
        }

        public override GenericValueLanguage VisitParenthesisExpression([NotNull] LanguageParser.ParenthesisExpressionContext context) =>
            Visit(context.arithmetic_expression());

        #region variable_declaration
        public override GenericValueLanguage VisitArithmeticDeclaration([NotNull] LanguageParser.ArithmeticDeclarationContext context)
        {
            var id = context.IDENTIFIER().GetText();
            var value = Visit(context.arithmetic_expression());

            if (_memoryLocal.ContainsKey(id) || _memoryLocalList.ContainsKey(id))
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + id.Length, $"Variável '{id}' já foi declarada", id);

            _memoryLocal.Add(id, value);

            return value;
        }

        public override GenericValueLanguage VisitComparisonDeclaration([NotNull] LanguageParser.ComparisonDeclarationContext context)
        {
            var id = context.IDENTIFIER().GetText();
            var value = Visit(context.comparison_expression());

            if (_memoryLocal.ContainsKey(id) || _memoryLocalList.ContainsKey(id))
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + id.Length, $"Variável '{id}' já foi declarada", id);

            _memoryLocal.Add(id, value);

            return value;
        }

        /// <summary>
        /// Método responsável por declarar uma lista recebendo apenas outra lista
        /// E salvar os dados na variavel global _memoryLocalArray.
        /// Set Languagem G4
        /// LISTA IDENTIFIER ATRIB arithmetic_expression SEMI #declareList
        /// </summary>
        /// 
        /// <example>
        /// lista arrayTest = ["value1", "value2", 10];
        /// lista arrayTestTwo = arrayTest;
        /// retorno arrayTestTwo[2]; -> Result 10
        /// </example>
        /// 
        /// <param name="context">
        /// context contém acesso as tokens definidos na language G4 da #declareList
        /// </param>
        /// 
        /// <returns>
        /// Tipo GenericValueLanguage vazio
        /// </returns>
        public override GenericValueLanguage VisitDeclareList([NotNull] LanguageParser.DeclareListContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            var value = Visit(context.arithmetic_expression());

            if (_memoryLocalList.ContainsKey(identifier))
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + identifier.Length, $"Lista '{identifier}' já foi declarada", identifier);

            /// <example>
            /// Recebe de outra lista criada pelo usuario
            // lista listaExample01[0].PropriedadeUm = 1;
            // listaExample01[0].PropriedadeDois = 2;
            // listaExample01[1].PropriedadeUm = 20;
            // lista listaExample02 = listaExample01;
            // retorno listaExample01[0].PropriedadeDois;
            // Result => 2;
            /// </example>
            if (value.Value is IDictionary<int, IDictionary<string, GenericValueLanguage>>)
            {
                _memoryLocalList[identifier] = new GenericValueLanguage(value.Value);
            }
            else
                _memoryLocalList[identifier] = new GenericValueLanguage(new Dictionary<int, IDictionary<string, GenericValueLanguage>>());

            return GenericValueLanguage.Empty;

        }

        /// <summary>
        /// Método responsável por declarar uma lista que vai poder receber
        /// index e propriedadw.
        /// E salvar os dados na variavel global _memoryLocalArray.
        /// Set Languagem G4
        /// LISTA IDENTIFIER ATRIB LBRACKET RBRACKET SEMI #declareListAll
        /// </summary>
        /// 
        /// <example>
        /// lista arrayTest[0].propriedade = 0;
        /// </example>
        /// 
        /// <param name="context">
        /// context contém acesso as tokens definidos na language G4 da #declareListWithBracketAndProperty
        /// </param>
        /// 
        /// <returns>
        /// Tipo GenericValueLanguage vazio
        /// </returns>
        public override GenericValueLanguage VisitDeclareListAll([NotNull] LanguageParser.DeclareListAllContext context)
        {
            string nameArray = context.IDENTIFIER().GetText();

            if (_memoryLocalList.ContainsKey(nameArray))
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + nameArray.Length, $"Lista '{nameArray}' já foi declarada", nameArray);

            IDictionary<int, IDictionary<string, GenericValueLanguage>> _dicIndexIntWithDic = new Dictionary<int, IDictionary<string, GenericValueLanguage>>();

            _memoryLocalList[nameArray] = new GenericValueLanguage(_dicIndexIntWithDic);

            return GenericValueLanguage.Empty;
        }

        #endregion

        #region assignment

        public override GenericValueLanguage VisitComparisonAssignment([NotNull] LanguageParser.ComparisonAssignmentContext context)
        {
            string identifier = context.IDENTIFIER().GetText();
            var value = Visit(context.comparison_expression());

            if (!_memoryLocal.ContainsKey(identifier))
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Varíavel '{identifier}' não foi declarada", context.GetText());

            _memoryLocal[identifier] = value;

            return _memoryLocal[identifier];
        }

        public override GenericValueLanguage VisitArithmeticAssignment([NotNull] LanguageParser.ArithmeticAssignmentContext context)
        {
            string identifier = context.IDENTIFIER().GetText();
            var value = Visit(context.arithmetic_expression());
            string op = context.assignment_operator().GetText();

            if (_memoryLocalList.ContainsKey(identifier))
            {
                switch (op)
                {
                    case "=":
                        _memoryLocalList[identifier] = value;
                        return GenericValueLanguage.Empty;
                    default:
                        throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Operador de atribuição inválido para tipo lista.", context.GetText());
                }
            }
            else if (_memoryLocal.ContainsKey(identifier))
            {
                _memoryLocal[identifier] = OperadorAtribuicao(_memoryLocal[identifier], value, op, context);
            }
            else throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Varíavel '{identifier}' não foi declarada", context.GetText());

            return GenericValueLanguage.Empty;
        }

        /// <summary>
        /// Método atribur valor lista criadas pelo usuario
        /// Valores possiveis: todos contidos na arithmetic_expression;
        /// IDENTIFIER LBRACKET (IDENTIFIER | number_integer) RBRACKET (DOT IDENTIFIER)? ATRIB arithmetic_expression SEMI #listAssignment
        /// </summary>
        /// 
        /// <example>
        /// Set array -> arrayTest03[0] = 10;
        /// Set array -> arrayTest03[1] = 50;
        /// Set array -> arrayTest04[0].Propriedade01 = "Claudio";
        /// Set array -> arrayTest04[0].Propriedade02 = "Monstro";
        /// ---
        /// Set -> var valorVar01 = 10;
        /// Set -> var valorVar02 = 20;
        /// --
        /// 
        /// arrayTest01[0] = valorVar;
        /// Resultado -> arrayTest01[0] -> 10
        /// 
        /// arrayTest02[1] = arrayTest01[0] + arrayTest02[2];
        /// Resultado -> arrayTest02[1] -> 60
        /// 
        /// arrayTest04[0].Propriedade01 = valorVar01;
        /// Resultado -> arrayTest04[0].Propriedade01 -> 10
        /// 
        /// arrayTest04[0].Propriedade02 =  arrayTest04[0].Propriedade01;
        /// Resultado -> arrayTest04[0].Propriedade02 -> "Claudio"
        /// ---
        /// </example>
        /// 
        /// <param name="context">
        /// context contém acesso as tokens definidos na language G4 da #listAssignment
        /// </param>
        /// 
        /// <returns>
        /// Tipo GenericValueLanguage com o valor ou array definido.
        /// </returns>
        /// 
        public override GenericValueLanguage VisitListAssignment([NotNull] LanguageParser.ListAssignmentContext context)
        {
            string nameArray = context.IDENTIFIER(0).GetText();
            int index = GetIndexArray(context.number_integer()?.GetText(), context.IDENTIFIER(1)?.GetText(), context);
            string propertyArray = context.number_integer() is null ? context.IDENTIFIER(2)?.GetText() : context.IDENTIFIER(1)?.GetText();
            string op = context.assignment_operator().GetText();
            var value = Visit(context.arithmetic_expression());

            if (!_memoryLocalList.TryGetValue(nameArray, out GenericValueLanguage array))
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Lista '{nameArray}' não foi declarada", context.GetText());

            // IDictionary<int, IDictionary<string, GenericValueLanguage>>
            // Ocorre quando a lista vem de com Index e Property
            // listaInicioColchete[0].Nome
            if (!string.IsNullOrEmpty(propertyArray) && array.IsDictionaryIntDictionaryStringGeneric())
            {
                var listIndexPropery = (array.AsDictionaryIntDictionaryStringGeneric());
                listIndexPropery.TryGetValue(index, out IDictionary<string, GenericValueLanguage> objectIndex);

                // Index nao existe cria index e adiciona propriedade
                if (objectIndex == null)
                {
                    IDictionary<string, GenericValueLanguage> newObject = new Dictionary<string, GenericValueLanguage>
                    {
                        { propertyArray, value }
                    };
                    listIndexPropery.Add(index, newObject);
                    var newValue = new GenericValueLanguage(listIndexPropery);

                    _memoryLocalList[nameArray] = newValue;
                }
                // Index existe index e adiciona ou atualiza propriedade
                else
                {
                    objectIndex.TryGetValue(propertyArray, out GenericValueLanguage objectoPropery);
                    // Se não contém propriedade cria
                    if (objectoPropery.Value is null)
                        objectIndex.Add(propertyArray, value);
                    // Se contém propriedade atualiza
                    else
                        objectIndex[propertyArray] = OperadorAtribuicao(objectIndex[propertyArray], value, op, context);
                }
            }
            else
            {
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Propriedade não informada", context.GetText());
            }

            return value;
        }

        /// <summary>
        /// Realiza operações de atribuição.
        /// </summary>
        /// <param name="currentValue">Valor atual da propriedade</param>
        /// <param name="value">Valor a ser atribuído</param>
        /// <param name="op">Operação de atribuição</param>
        /// <param name="context">Contexto da execução</param>
        /// <returns>Novo valor da propriedade</returns>
        public GenericValueLanguage OperadorAtribuicao(GenericValueLanguage currentValue, GenericValueLanguage value, string op, ParserRuleContext context) => op switch
        {
            "=" => value,
            "+=" when currentValue.Value is string currentString => new GenericValueLanguage(currentString + value.AsString()),
            "+=" when currentValue.IsNumeric && value.IsNumeric => currentValue + value,
            "-=" when currentValue.IsNumeric && value.IsNumeric => currentValue - value,
            "*=" when currentValue.IsNumeric && value.IsNumeric => currentValue * value,
            "/=" when currentValue.IsNumeric && value.IsNumeric && (decimal)value != 0 => currentValue / value,
            _ => throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, "Atribuição de valor incompatível", context.GetText()),
        };

        public override GenericValueLanguage VisitVarMemoryValueAssignment([NotNull] LanguageParser.VarMemoryValueAssignmentContext context) =>
            throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Não é possível atribuir valores a constantes globais", context.GetText());

        public override GenericValueLanguage VisitListMemoryGlobalValueAssignment([NotNull] LanguageParser.ListMemoryGlobalValueAssignmentContext context) =>
            throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Não é possível atribuir valores a constantes globais", context.GetText());

        public override GenericValueLanguage VisitListMemoryValueAssignment([NotNull] LanguageParser.ListMemoryValueAssignmentContext context) =>
            throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Não é possível atribuir valores a constantes globais", context.GetText());

        #endregion

        /// <summary>
        /// Método responsável por retornar um valor de Lista criada pelo usuario.
        /// IDENTIFIER LBRACKET (IDENTIFIER | number_integer) RBRACKET PROPERTY? #listValue
        /// </summary>
        /// 
        /// <param name="context">
        /// context contém acesso as tokens definidos na language G4 da #VariableArrayEntity
        /// </param>
        /// 
        /// <returns>
        /// Tipo GenericValueLanguage com o valor ou array definido.
        /// </returns>
        public override GenericValueLanguage VisitListValue(LanguageParser.ListValueContext context)
        {
            string nameArray = context.IDENTIFIER(0).GetText();
            int index = GetIndexArray(context.number_integer()?.GetText(), context.IDENTIFIER(1)?.GetText(), context);
            string propertyArray = context.number_integer() is null ? context.IDENTIFIER(2)?.GetText() : context.IDENTIFIER(1)?.GetText();

            // Valida se nameArray contém na varial global _memoryLocalArrayGlobal
            if (_memoryLocalList.TryGetValue(nameArray, out GenericValueLanguage arrayGlobal))
            {
                return GetListValue(arrayGlobal, index, propertyArray, context);
            }
            throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Lista '{nameArray}' não encontrada", context.GetText());

        }

        private int GetIndexArray(string number, string property, ParserRuleContext context)
        {
            if (!int.TryParse(number, out int index) || index < 0)
            {
                if (!_memoryLocal.TryGetValue(property, out GenericValueLanguage currentValue))
                    throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + property.Length, $"'{property}' não foi declarada", property);

                return currentValue.IsNumeric ? (int)currentValue : -1;
            }

            return index;
        }

        public override GenericValueLanguage VisitReturnValue([NotNull] LanguageParser.ReturnValueContext context) =>
            Visit(context.arithmetic_expression());

        public override GenericValueLanguage VisitBoolEntity(LanguageParser.BoolEntityContext context) =>
            new GenericValueLanguage(bool.Parse(context.GetText()));

        public override GenericValueLanguage VisitNumberDecimalEntity(LanguageParser.NumberDecimalEntityContext context) =>
            new GenericValueLanguage(decimal.Parse(context.GetText(), CultureInfo.InvariantCulture));

        public override GenericValueLanguage VisitNumberIntegerEntity(LanguageParser.NumberIntegerEntityContext context) =>
            new GenericValueLanguage(int.Parse(context.GetText(), CultureInfo.InvariantCulture));

        public override GenericValueLanguage VisitNumberInteger([NotNull] LanguageParser.NumberIntegerContext context) =>
            new GenericValueLanguage(int.Parse(context.GetText(), CultureInfo.InvariantCulture));

        public override GenericValueLanguage VisitNumberDecimal([NotNull] LanguageParser.NumberDecimalContext context) =>
            new GenericValueLanguage(decimal.Parse(context.GetText(), CultureInfo.InvariantCulture));

        public override GenericValueLanguage VisitDateEntity(LanguageParser.DateEntityContext context)
        {
            var culture = new CultureInfo("pt-BR", true);
            DateTime.TryParse(context.GetText(), culture, DateTimeStyles.None, out DateTime dateTime);
            return new GenericValueLanguage(dateTime);
        }

        public override GenericValueLanguage VisitStringEntity(LanguageParser.StringEntityContext context) =>
            Visit(context.text());

        public override GenericValueLanguage VisitString(LanguageParser.StringContext context) =>
            new GenericValueLanguage(context.GetText().Replace("\"", string.Empty));

        public override GenericValueLanguage VisitVariableEntity(LanguageParser.VariableEntityContext context)
        {
            var id = context.GetText();

            if (id is null) throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Variável não informada", context.GetText());

            if (_memoryLocal.TryGetValue(id, out GenericValueLanguage value) || _memoryLocalList.TryGetValue(id, out value))
                return value;
            else throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Variável '{id}' não encontrada", context.GetText());
        }

        //TODO
        public override GenericValueLanguage VisitCoalesceFunction([NotNull] LanguageParser.CoalesceFunctionContext context)
        {
            int parameters = context.GetText().Count(x => x == ',') + 1;
            for (int i = 0; i < parameters; i++)
            {
                var value = Visit(context.entity(i));
                if (value.Value != null)
                    return value;
            }
            return new GenericValueLanguage(null);
        }

        public override GenericValueLanguage VisitRoundFunction([NotNull] LanguageParser.RoundFunctionContext context)
        {
            var obj = Visit(context.arithmetic_expression(0));

            if (obj.Value is null)
                throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Não foi arredondar a entrada {obj}", context.GetText());

            decimal number = obj.IsNull() && !obj.IsDecimal() && !obj.IsInt() ? 0 : obj.AsDecimal();
            int? decimalPlaces = context.arithmetic_expression(1) != null ? Visit(context.arithmetic_expression(1)).AsInt() : default(int?);

            return new GenericValueLanguage(Math.Round(number, decimalPlaces ?? 0));
        }

        public override GenericValueLanguage VisitWhileExpression([NotNull] LanguageParser.WhileExpressionContext context)
        {
            while (Visit(context.if_expression()).AsBoolean())
            {
                foreach (var item in context.rule_block())
                    Visit(item);
            }

            return GenericValueLanguage.NULL;
        }

        public override GenericValueLanguage VisitIsNullFunction([NotNull] LanguageParser.IsNullFunctionContext context) =>
            new GenericValueLanguage(Visit(context.arithmetic_expression()).Value is null);

        public override GenericValueLanguage VisitDateFunction([NotNull] LanguageParser.DateFunctionContext context)
        {
            var dia = Visit(context.arithmetic_expression(0));
            var mes = Visit(context.arithmetic_expression(1));
            var ano = Visit(context.arithmetic_expression(2));

            if (dia.IsNumeric && mes.IsNumeric && ano.IsNumeric)
                return new GenericValueLanguage(new DateTime(ano.AsInt(), mes.AsInt(), dia.AsInt()));

            throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Não foi possível converter a entrada {dia}/{mes}/{ano} em uma data", context.GetText());
        }

        public override GenericValueLanguage VisitTodayFunction([NotNull] LanguageParser.TodayFunctionContext context) =>
            new GenericValueLanguage(DateTime.Today);

        public override GenericValueLanguage VisitNowFunction([NotNull] LanguageParser.NowFunctionContext context) =>
            new GenericValueLanguage(DateTime.Now);

        public override GenericValueLanguage VisitDateDifFunction([NotNull] LanguageParser.DateDifFunctionContext context)
        {
            var left = Visit(context.arithmetic_expression(0));
            var right = Visit(context.arithmetic_expression(1));
            string unit = context.date_unit().GetText();

            return unit switch
            {
                "ANO" => new GenericValueLanguage(Math.Abs((right.AsDateTime() - left.AsDateTime()).Days / 365)),
                "MES" => new GenericValueLanguage(Math.Abs(right.AsDateTime().MonthDifference(left.AsDateTime()))),
                "DIA" => new GenericValueLanguage((right.AsDateTime() - left.AsDateTime()).Days),
                _ => throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, $"Unidade '{unit}' inválida", context.GetText())
            };
        }

        public override GenericValueLanguage VisitGetDayFunction([NotNull] LanguageParser.GetDayFunctionContext context) =>
            new GenericValueLanguage(Visit(context.arithmetic_expression()).AsDateTime().Day);

        public override GenericValueLanguage VisitGetMonthFunction([NotNull] LanguageParser.GetMonthFunctionContext context) =>
            new GenericValueLanguage(Visit(context.arithmetic_expression()).AsDateTime().Month);

        public override GenericValueLanguage VisitGetYearFunction([NotNull] LanguageParser.GetYearFunctionContext context) =>
            new GenericValueLanguage(Visit(context.arithmetic_expression()).AsDateTime().Year);

        public override GenericValueLanguage VisitGetHourFunction([NotNull] LanguageParser.GetHourFunctionContext context) =>
            new GenericValueLanguage(Visit(context.arithmetic_expression()).AsDateTime().Hour);

        public override GenericValueLanguage VisitGetMinuteFunction([NotNull] LanguageParser.GetMinuteFunctionContext context) =>
            new GenericValueLanguage(Visit(context.arithmetic_expression()).AsDateTime().Minute);

        public override GenericValueLanguage VisitAddDayFunction([NotNull] LanguageParser.AddDayFunctionContext context) =>
            new GenericValueLanguage(Visit(context.arithmetic_expression(0)).AsDateTime().AddDays(Visit(context.arithmetic_expression(1)).AsInt()));

        public override GenericValueLanguage VisitAddMonthFunction([NotNull] LanguageParser.AddMonthFunctionContext context) =>
            new GenericValueLanguage(Visit(context.arithmetic_expression(0)).AsDateTime().AddMonths(Visit(context.arithmetic_expression(1)).AsInt()));

        public override GenericValueLanguage VisitAddYearFunction([NotNull] LanguageParser.AddYearFunctionContext context) =>
            new GenericValueLanguage(Visit(context.arithmetic_expression(0)).AsDateTime().AddYears(Visit(context.arithmetic_expression(1)).AsInt()));

        public override GenericValueLanguage VisitTrimFunction([NotNull] LanguageParser.TrimFunctionContext context) =>
            new GenericValueLanguage(StringHelper.NormalizeWhiteSpace(Visit(context.arithmetic_expression()).AsString()));

        /// <summary>
        /// Realiza comparações entre duas propriedades.
        /// </summary>
        /// <param name="left">Valor à esquerda da comparação</param>
        /// <param name="right">Valor à direita da comparação</param>
        /// <param name="op">Operador de comparação</param>
        /// <param name="context">Contexto da execução</param>
        /// <returns>Resultado da comparação</returns>
        private GenericValueLanguage CompareGenericValues(GenericValueLanguage left, GenericValueLanguage right, string op, ParserRuleContext context) => op switch
        {
            "==" => Equal(left, right),
            "!=" => NotEqual(left, right),
            ">" => GreaterThan(left, right, context),
            "<" => LessThan(left, right, context),
            ">=" => GreaterThanOrEqual(left, right, context),
            "<=" => LessThanOrEqual(left, right, context),
            _ => throw new LanguageException(context.Start.Line, context.Start.Column, context.Stop.Column + context.GetText().Length, $"Operador '{op}' nao reconhecido.", context.GetText())
        };

        /// <summary>
        /// Comparação de igualdade entre duas propriedades.
        /// </summary>
        /// <param name="left">Valor à esquerda da comparação</param>
        /// <param name="right">Valor à direita da comparação</param>
        /// <returns>Resultado da comparação</returns>
        private GenericValueLanguage Equal(GenericValueLanguage left, GenericValueLanguage right)
        {
            if (left.IsNumeric && right.IsNumeric)
                return new GenericValueLanguage(left.AsDecimal() == right.AsDecimal());
            else if (left.IsDate() && right.IsDate())
                return new GenericValueLanguage(left.AsDateTime() == right.AsDateTime());

            return new GenericValueLanguage(left.Value == right.Value);
        }

        /// <summary>
        /// Comparação de não igualdade entre duas propriedades.
        /// </summary>
        /// <param name="left">Valor à esquerda da comparação</param>
        /// <param name="right">Valor à direita da comparação</param>
        /// <returns>Resultado da comparação</returns>
        private GenericValueLanguage NotEqual(GenericValueLanguage left, GenericValueLanguage right)
        {
            if (left.IsNumeric && right.IsNumeric)
                return new GenericValueLanguage(left.AsDecimal() != right.AsDecimal());
            else if (left.IsDate() && right.IsDate())
                return new GenericValueLanguage(left.AsDateTime() != right.AsDateTime());

            return new GenericValueLanguage(left.Value != right.Value);
        }

        /// <summary>
        /// Comparação de maior que entre duas propriedades.
        /// </summary>
        /// <param name="left">Valor à esquerda da comparação</param>
        /// <param name="right">Valor à direita da comparação</param>
        /// <returns>Resultado da comparação</returns>
        private GenericValueLanguage GreaterThan(GenericValueLanguage left, GenericValueLanguage right, ParserRuleContext context)
        {
            if (left.IsNumeric && right.IsNumeric)
                return new GenericValueLanguage(left.AsDecimal() > right.AsDecimal());

            if (left.IsDate() && right.IsDate())
                return new GenericValueLanguage(left.AsDateTime() > right.AsDateTime());

            if (left.Value is null || right.Value is null)
                return new GenericValueLanguage(false);

            throw new LanguageException(context.Start.Line, context.Start.Column, context.Stop.Column + context.GetText().Length, "Comparando valores incompatíveis.", context.GetText());
        }

        /// <summary>
        /// Comparação de menor que entre duas propriedades.
        /// </summary>
        /// <param name="left">Valor à esquerda da comparação</param>
        /// <param name="right">Valor à direita da comparação</param>
        /// <returns>Resultado da comparação</returns>
        private GenericValueLanguage LessThan(GenericValueLanguage left, GenericValueLanguage right, ParserRuleContext context)
        {
            if (left.IsNumeric && right.IsNumeric)
                return new GenericValueLanguage(left.AsDecimal() < right.AsDecimal());

            if (left.IsDate() && right.IsDate())
                return new GenericValueLanguage(left.AsDateTime() < right.AsDateTime());

            if (left.Value is null || right.Value is null)
                return new GenericValueLanguage(false);

            throw new LanguageException(context.Start.Line, context.Start.Column, context.Stop.Column + context.GetText().Length, "Comparando valores incompatíveis.", context.GetText());
        }

        /// <summary>
        /// Comparação de maior que ou igual entre duas propriedades.
        /// </summary>
        /// <param name="left">Valor à esquerda da comparação</param>
        /// <param name="right">Valor à direita da comparação</param>
        /// <returns>Resultado da comparação</returns>
        private GenericValueLanguage GreaterThanOrEqual(GenericValueLanguage left, GenericValueLanguage right, ParserRuleContext context)
        {
            if (left.IsNumeric && right.IsNumeric)
                return new GenericValueLanguage(left.AsDecimal() >= right.AsDecimal());

            if (left.IsDate() && right.IsDate())
                return new GenericValueLanguage(left.AsDateTime() >= right.AsDateTime());

            if (left.Value is null || right.Value is null)
                return new GenericValueLanguage(false);

            throw new LanguageException(context.Start.Line, context.Start.Column, context.Stop.Column + context.GetText().Length, "Comparando valores incompatíveis.", context.GetText());
        }

        /// <summary>
        /// Comparação de menor que ou igual entre duas propriedades.
        /// </summary>
        /// <param name="left">Valor à esquerda da comparação</param>
        /// <param name="right">Valor à direita da comparação</param>
        /// <returns>Resultado da comparação</returns>
        private GenericValueLanguage LessThanOrEqual(GenericValueLanguage left, GenericValueLanguage right, ParserRuleContext context)
        {
            if (left.IsNumeric && right.IsNumeric)
                return new GenericValueLanguage(left.AsDecimal() <= right.AsDecimal());

            if (left.IsDate() && right.IsDate())
                return new GenericValueLanguage(left.AsDateTime() <= right.AsDateTime());

            if (left.Value is null || right.Value is null)
                return new GenericValueLanguage(false);

            throw new LanguageException(context.Start.Line, context.Start.Column, context.Start.Column + context.GetText().Length, "Comparando valores incompatíveis.", context.GetText());
        }
    }
}