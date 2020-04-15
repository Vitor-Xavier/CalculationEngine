using Antlr4.Runtime.Misc;
using Common.Enums;
using Common.Exceptions;
using Common.Extensions;
using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class VisitorLanguage : LanguageBaseVisitor<GenericValueLanguage>
{
    private readonly IDictionary<string, GenericValueLanguage> _memory;
    private readonly IDictionary<string, GenericValueLanguage> _memoryGlobal;
    private readonly IDictionary<string, GenericValueLanguage> _memoryLocal = new Dictionary<string, GenericValueLanguage>();

    private readonly IDictionary<string, GenericValueLanguage> _memoryLocalList = new Dictionary<string, GenericValueLanguage>();
    private readonly GenericValueLanguage empty = new GenericValueLanguage(0);
    private GenericValueLanguage _switchValue;

    public VisitorLanguage(IDictionary<string, GenericValueLanguage> memory, IDictionary<string, GenericValueLanguage> memoryGlobal)
    {
        _memory = memory ?? new Dictionary<string, GenericValueLanguage>();
        _memoryGlobal = memoryGlobal ?? new Dictionary<string, GenericValueLanguage>();

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
    /// Essas variaveis na foruma inicia com @
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

        GenericValueLanguage valueMemory;

        // Busca Memory Formulas
        valueMemory = TryGet(identifier, EnumMemory.Memory);
        if (valueMemory.IsDictionaryStringGeneric())
        {
            valueMemory.AsDictionaryStringGeneric().TryGetValue(objectKey, out GenericValueLanguage result);
            return result;
        }

        // Busca Memory do banco DAdos
        valueMemory = TryGet(identifierTotal, EnumMemory.MemoryOrMemoryGlobal);
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
        int index = GetIndexArray(context.number_integer()?.GetText(), context.IDENTIFIER(0)?.GetText());
        string propertyArray = context.number_integer() is null ? context.IDENTIFIER(1)?.GetText() : context.IDENTIFIER(0)?.GetText();

        GenericValueLanguage valueMemory;

        valueMemory = TryGet(nameArray, EnumMemory.Memory);

        if (valueMemory.IsDictionaryIntDictionaryStringGeneric())
        {
            return GetListValue(valueMemory, index, propertyArray);
        }
        return new GenericValueLanguage(null);

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
        int index = GetIndexArray(context.number_integer()?.GetText(), context.IDENTIFIER(1)?.GetText());
        string propertyArrayKey = context.number_integer() is null ? context.IDENTIFIER(2)?.GetText() : context.IDENTIFIER(1)?.GetText();

        GenericValueLanguage listMemory;

        listMemory = TryGet(nameArray, EnumMemory.Memory);

        if (listMemory.IsDictionaryStringGeneric())
        {
            var listMemoryProperty = listMemory.AsDictionaryStringGeneric()[propertyArray];
            return GetListValue(listMemoryProperty, index, propertyArrayKey);
        }
        else throw new Exception("Variavel nao informada");
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
    private GenericValueLanguage GetListValue(GenericValueLanguage listProperty, int index, string propertyArrayKey)
    {

        // Retorna valor de Dictionary Int com Dictionary String,GenericValue dentro
        // Exemplo
        // var value = array[0].propriedade1;
        if (listProperty.IsDictionaryIntDictionaryStringGeneric())
        {
            IDictionary<string, GenericValueLanguage> indexDictonaryStringGeneric;
            listProperty.AsDictionaryIntDictionaryStringGeneric().TryGetValue(index, out indexDictonaryStringGeneric);

            if (indexDictonaryStringGeneric is null)
                throw new ArgumentException($"Indice nao foi encontrado.");


            // Retorna todas as propriedades do index
            if (string.IsNullOrEmpty(propertyArrayKey))
            {
                IDictionary<int, IDictionary<string, GenericValueLanguage>> createDictIntDictStringGeneric = new Dictionary<int, IDictionary<string, GenericValueLanguage>>
                {
                    { index, indexDictonaryStringGeneric }
                };
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
            return empty;
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

        return OperatorFunctionVariable(identifier, EnumOperation.Sum);
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

        return OperatorFunctionVariable(identifier, EnumOperation.Max);
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

        return OperatorFunctionVariable(identifier, EnumOperation.Min);
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

        return OperatorFunctionVariable(identifier, EnumOperation.Average);
    }
    #endregion

    #region Length
    public override GenericValueLanguage VisitLengthDatabase([NotNull] LanguageParser.LengthDatabaseContext context)
    {
        string varPrimary = context.VAR_PRIMARY().GetText();
        string identifier = context.IDENTIFIER()?.GetText();


        if (string.IsNullOrEmpty(identifier))
        {
            GenericValueLanguage currentValue = TryGet(varPrimary, EnumMemory.MemoryOrMemoryGlobal);

            if (currentValue.IsNull())
                throw new Exception($"Varíavel '{identifier}' não foi declarada");

            if (currentValue.IsDictionaryIntDictionaryStringGeneric())
            {
                var listIndexPropery = (currentValue.AsDictionaryIntDictionaryStringGeneric());
                var soma = listIndexPropery.Count;
                return new GenericValueLanguage(soma);
            }
        }
        else
        {
            GenericValueLanguage value = TryGet(varPrimary, EnumMemory.MemoryOrMemoryGlobal);

            if (value.IsNull())
                throw new Exception($"Varíavel '{varPrimary + "." + identifier}' não foi declarada");

            if (value.IsNull())
                return empty;

            if (value.IsDictionaryStringGeneric())
            {
                IDictionary<string, GenericValueLanguage> resultMemory = value.AsDictionaryStringGeneric();


                if (resultMemory.TryGetValue(identifier, out GenericValueLanguage resultIdentifier01) && resultIdentifier01.IsDictionaryIntDictionaryStringGeneric())
                {
                    return new GenericValueLanguage(resultIdentifier01.AsDictionaryIntDictionaryStringGeneric().Count);
                }

                return empty;
            }

        }


        return empty;


    }

    // Lista Identifer
    public override GenericValueLanguage VisitLengthVariable([NotNull] LanguageParser.LengthVariableContext context)
    {
        string identifier = context.IDENTIFIER().GetText();

        GenericValueLanguage currentValue = TryGet(identifier, EnumMemory.MemoryLocalOrMemoryLocalList);

        if (currentValue.IsNull())
            throw new Exception($"Varíavel '{identifier}' não foi declarada");

        if (currentValue.IsDictionaryIntDictionaryStringGeneric())
        {
            var listIndexPropery = (currentValue.AsDictionaryIntDictionaryStringGeneric());
            var soma = listIndexPropery.Count;
            return new GenericValueLanguage(soma);
        }
        return empty;
    }
    #endregion

    private GenericValueLanguage OperatorFunctionDataBase(string varPrimary, string identifier01, string identifier02, EnumOperation enumOperator)
    {

        GenericValueLanguage value = TryGet(varPrimary, EnumMemory.MemoryOrMemoryGlobal);

        if (value.IsNull())
            return empty;

        if (value.IsDictionaryStringGeneric())
        {
            IDictionary<string, GenericValueLanguage> resultMemory = value.AsDictionaryStringGeneric();

            GenericValueLanguage resultIdentifier01;

            if (resultMemory.TryGetValue(identifier01, out resultIdentifier01) && resultIdentifier01.IsDictionaryIntDictionaryStringGeneric())
            {
                return OperatorDictionaryIntDictionaryStringGeneric(identifier02, enumOperator, resultIdentifier01);
            }
            else if (resultMemory.TryGetValue(identifier01, out resultIdentifier01))
            {
                return resultIdentifier01;
            }

            return empty;
        }
        else if (value.IsDictionaryIntDictionaryStringGeneric())
        {
            return OperatorDictionaryIntDictionaryStringGeneric(identifier01, enumOperator, value);
        }
        else
            return empty;
    }

    private GenericValueLanguage OperatorFunctionListLocal(string identifier01, string identifier02, EnumOperation enumOperator)
    {
        GenericValueLanguage value = TryGet(identifier01, EnumMemory.MemoryLocalList);

        if (value.IsNull())
            return empty;

        if (value.IsDictionaryIntDictionaryStringGeneric())
        {
            return OperatorDictionaryIntDictionaryStringGeneric(identifier02, enumOperator, value);
        }
        else
            return empty;
    }

    private GenericValueLanguage OperatorFunctionVariable(string identifier, EnumOperation enumOperator)
    {
        GenericValueLanguage currentValue = TryGet(identifier, EnumMemory.MemoryLocalOrMemoryLocalList);

        if (currentValue.IsNull())
            throw new Exception($"Varíavel '{identifier}' não foi declarada");

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
            return empty;

        IEnumerable<decimal> list = ExtractIEnumrable(dictResultIdentifier01, identifier);
        return new GenericValueLanguage(Operador(list, enumOperator));
    }

    /// <summary>
    /// Extrai das variaveis Globais pelo Identificador passa pelo parametro EnumMemory
    /// </summary>
    /// <param name="identifier01"></param>
    /// <param name="enumMemory"></param>
    /// <returns></returns>
    private GenericValueLanguage TryGet(string identifier01, EnumMemory enumMemory)
    {
        GenericValueLanguage value = empty;

        switch (enumMemory)
        {
            case EnumMemory.MemoryLocalList:
                _memoryLocalList.TryGetValue(identifier01, out value);
                break;

            case EnumMemory.MemoryLocal:
                _memoryLocal.TryGetValue(identifier01, out value);
                break;

            case EnumMemory.MemoryGlobal:
                _memoryGlobal.TryGetValue(identifier01, out value);
                break;
            case EnumMemory.Memory:
                _memory.TryGetValue(identifier01, out value);
                break;
            case EnumMemory.MemoryOrMemoryGlobal:

                _memory.TryGetValue(identifier01, out value);

                if (value.IsNull())
                    _memoryGlobal.TryGetValue(identifier01, out value);

                break;
            case EnumMemory.MemoryLocalOrMemoryLocalList:

                _memoryLocal.TryGetValue(identifier01, out value);

                if (value.IsNull())
                    _memoryLocalList.TryGetValue(identifier01, out value);

                break;
            default:
                throw new InvalidOperationException($"Memory error.");
        }

        return value;
    }

    /// <summary>
    /// Recebe list e operador
    /// Executa a funcao da lista executando a funcao do EnumOperation
    /// </summary>
    /// <param name="list">Lista de decimais</param>
    /// <param name="op">Operação</param>
    /// <returns></returns>
    private decimal Operador(IEnumerable<decimal> list, EnumOperation op)
    {
        return op switch
        {
            EnumOperation.Sum => list.Sum(),
            EnumOperation.Min => list.Min(),
            EnumOperation.Max => list.Max(),
            EnumOperation.Average => list.Average(),
            EnumOperation.Count => list.Count(),
            EnumOperation.Length => list.Count(),
            _ => throw new InvalidOperationException($"Operador '{op}' nao reconhecido."),
        };
    }

    /// <summary>
    /// Extrai uma lista IDictionary<int, IDictionary<string, GenericValueLanguage>>
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="identifier"></param>
    /// <returns></returns>
    private static IEnumerable<decimal> ExtractIEnumrable(IDictionary<int, IDictionary<string, GenericValueLanguage>> dictionary, string identifier)
    {
        return dictionary.Select(x => x.Value.ToList().Where(p => p.Key == identifier).Select(p => p.Value.TryDecimal()).FirstOrDefault());
    }


    public override GenericValueLanguage VisitAbsFunction([NotNull] LanguageParser.AbsFunctionContext context) =>
        new GenericValueLanguage(Math.Abs(Visit(context.arithmetic_expression()).AsDecimal()));


    public override GenericValueLanguage VisitSumIfFunction([NotNull] LanguageParser.SumIfFunctionContext context)
    {

        string objectKey = context.VAR_PRIMARY().GetText();
        decimal sum = 0m;
        string propertyArrayKey = context.IDENTIFIER(0).GetText();
        string propertyArrayKey2 = context.IDENTIFIER(1)?.GetText();

        GenericValueLanguage value = TryGet(objectKey, EnumMemory.MemoryOrMemoryGlobal);

        if (value.IsDictionaryIntDictionaryStringGeneric())
        {

            string op = context.comparison_operator().GetText();
            for (var i = 0; i < value.AsDictionaryIntDictionaryStringGeneric().Count; i++)
            {
                value.AsDictionaryIntDictionaryStringGeneric().TryGetValue(i, out IDictionary<string, GenericValueLanguage> item);
                var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out GenericValueLanguage leftValue) ? leftValue : Visit(context.arithmetic_expression(0));
                var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out GenericValueLanguage rightValue) ? rightValue : Visit(context.arithmetic_expression(1));

                if (CompareGenericValues(left, right, op).AsBoolean())
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

                    if (CompareGenericValues(left, right, op).AsBoolean())
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


        GenericValueLanguage value = TryGet(identifier01, EnumMemory.MemoryLocalOrMemoryLocalList);

        if (value.IsDictionaryIntDictionaryStringGeneric())
        {
            string propertyArrayKey = context.IDENTIFIER(1).GetText();
            string op = context.comparison_operator().GetText();
            for (var i = 0; i < value.AsDictionaryIntDictionaryStringGeneric().Count; i++)
            {
                value.AsDictionaryIntDictionaryStringGeneric().TryGetValue(i, out IDictionary<string, GenericValueLanguage> item);
                var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out GenericValueLanguage leftValue) ? leftValue : Visit(context.arithmetic_expression(0));
                var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out GenericValueLanguage rightValue) ? rightValue : Visit(context.arithmetic_expression(1));

                if (CompareGenericValues(left, right, op).AsBoolean())
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

        GenericValueLanguage value = TryGet(objectKey, EnumMemory.MemoryOrMemoryGlobal);
        if (value.IsDictionaryIntDictionaryStringGeneric())
        {

            string op = context.comparison_operator().GetText();
            for (var i = 0; i < value.AsDictionaryIntDictionaryStringGeneric().Count; i++)
            {
                value.AsDictionaryIntDictionaryStringGeneric().TryGetValue(i, out IDictionary<string, GenericValueLanguage> item);
                var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out GenericValueLanguage leftValue) ? leftValue : Visit(context.arithmetic_expression(0));
                var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out GenericValueLanguage rightValue) ? rightValue : Visit(context.arithmetic_expression(1));

                if (CompareGenericValues(left, right, op).AsBoolean())
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

                if (CompareGenericValues(left, right, op).AsBoolean())
                    count++;
            }
        }

        return new GenericValueLanguage(count);
    }

    public override GenericValueLanguage VisitCountIfListLocal([NotNull] LanguageParser.CountIfListLocalContext context)
    {
        string objectKey = context.IDENTIFIER().GetText();
        decimal count = 0;

        GenericValueLanguage value = TryGet(objectKey, EnumMemory.MemoryLocalOrMemoryLocalList);
        if (value.IsDictionaryIntDictionaryStringGeneric())
        {

            string op = context.comparison_operator().GetText();
            for (var i = 0; i < value.AsDictionaryIntDictionaryStringGeneric().Count; i++)
            {
                value.AsDictionaryIntDictionaryStringGeneric().TryGetValue(i, out IDictionary<string, GenericValueLanguage> item);
                var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out GenericValueLanguage leftValue) ? leftValue : Visit(context.arithmetic_expression(0));
                var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out GenericValueLanguage rightValue) ? rightValue : Visit(context.arithmetic_expression(1));

                if (CompareGenericValues(left, right, op).AsBoolean())
                    count++;
            }
        }

        return new GenericValueLanguage(count);
    }
    public override GenericValueLanguage VisitParametroFunction([NotNull] LanguageParser.ParametroFunctionContext context)
    {
        var nome = Visit(context.text()).AsString();
        int exercicio = int.TryParse(context.number_integer(0).GetText(), out int num) ? num : default;
        int? codigo = int.TryParse(context.number_integer(1)?.GetText(), out int num2) ? num2 : default(int?);

        IDictionary<int, IDictionary<string, GenericValueLanguage>> _dicIndexIntWithDic = new Dictionary<int, IDictionary<string, GenericValueLanguage>>();
        int i = 0;

        GenericValueLanguage value = TryGet("@ParametroVlrs", EnumMemory.MemoryOrMemoryGlobal);
        if (value.IsDictionaryIntDictionaryStringGeneric())
        {
            var result = value.AsDictionaryIntDictionaryStringGeneric();
            foreach (var item in result)
            {
                var resultItem = (item.Value as IDictionary<string, GenericValueLanguage>);
                resultItem.TryGetValue("NomeParam", out GenericValueLanguage obj);
                resultItem.TryGetValue("Exercicio", out GenericValueLanguage obj2);

                GenericValueLanguage obj3 = new GenericValueLanguage(null);
                if (codigo.HasValue)
                    resultItem.TryGetValue("Codigo", out obj3);

                if (obj.AsString() == nome && obj2.AsInt() == exercicio && (codigo.HasValue && !obj3.IsNull() ? obj3.AsInt() == codigo.Value : true))
                    _dicIndexIntWithDic[i++] = resultItem;
            }
        }
        return new GenericValueLanguage(_dicIndexIntWithDic);
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

        return new GenericValueLanguage(null);
    }

    public override GenericValueLanguage VisitCaracteristicaTabela(LanguageParser.CaracteristicaTabelaContext context)
    {
        var tabela = Visit(context.tabela_caracteristica());
        var descricao = Visit(context.descricao_caracteristica());

        string identifierFull = $"@{tabela.Value}.{descricao.Value}";

        GenericValueLanguage value = TryGet(identifierFull, EnumMemory.Memory);
        if (value.IsDictionaryIntDictionaryStringGeneric())
        {
            var listIndexPropery = (value.AsDictionaryIntDictionaryStringGeneric());
            listIndexPropery.TryGetValue(0, out IDictionary<string, GenericValueLanguage> objectIndex);
            decimal.TryParse((objectIndex as IDictionary<string, GenericValueLanguage>)["Valor"].Value?.ToString(), out decimal resultado);
            return new GenericValueLanguage(resultado);
        }

        return empty;
    }

    public override GenericValueLanguage VisitCaracteristica(LanguageParser.CaracteristicaContext context)
    {
        var descricao = Visit(context.descricao_caracteristica());

        string identifierFull = $"@Caracteristica.{descricao.Value}";
        GenericValueLanguage value = TryGet(identifierFull, EnumMemory.MemoryGlobal);
        if (value.IsDictionaryStringGeneric())
        {
            decimal.TryParse((value.Value as IDictionary<string, GenericValueLanguage>)["Valor"].Value?.ToString(), out decimal resultado);
            return new GenericValueLanguage(resultado);
        }

        return empty;
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

        return new GenericValueLanguage(null);
    }

    public override GenericValueLanguage VisitElseBlock([NotNull] LanguageParser.ElseBlockContext context)
    {
        foreach (var ruleBlock in context.rule_block())
        {
            if (Visit(ruleBlock).Value is SpecialValue specialValue && specialValue == SpecialValue.Break)
                return new GenericValueLanguage(specialValue);
        }

        return new GenericValueLanguage(null);
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

        return CompareGenericValues(left, right, op);
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
                return new GenericValueLanguage(null);
        }

        if (context.default_statement() != null)
            Visit(context.default_statement());

        return new GenericValueLanguage(null);
    }

    public override GenericValueLanguage VisitCaseStatement([NotNull] LanguageParser.CaseStatementContext context)
    {
        if (CompareGenericValues(_switchValue, Visit(context.arithmetic_expression()), "==").AsBoolean())
        {
            foreach (var ruleBlock in context.rule_block())
            {
                if (Visit(ruleBlock).Value is SpecialValue specialValue && specialValue == SpecialValue.Break)
                    return new GenericValueLanguage(specialValue);
            }
        }

        return new GenericValueLanguage(null);
    }

    public override GenericValueLanguage VisitDefaultStatement([NotNull] LanguageParser.DefaultStatementContext context)
    {
        foreach (var ruleBlock in context.rule_block())
        {
            if (Visit(ruleBlock).Value is SpecialValue specialValue && specialValue == SpecialValue.Break)
                return new GenericValueLanguage(specialValue);
        }

        return new GenericValueLanguage(null);
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
            throw new Exception("Nao e possivel dividir por zero.");

        return left / right;
    }

    public override GenericValueLanguage VisitPowExpression([NotNull] LanguageParser.PowExpressionContext context)
    {
        var left = Visit(context.arithmetic_expression(0));
        var right = Visit(context.arithmetic_expression(1));

        if (left.IsNumeric && right.IsNumeric)
            return new GenericValueLanguage(Math.Pow((double)left, (double)right));
        throw new ArithmeticException($"Não é possível elevar o valor {left.Value} a {right.Value}");
    }

    public override GenericValueLanguage VisitSqrtFunction([NotNull] LanguageParser.SqrtFunctionContext context)
    {
        var number = Visit(context.arithmetic_expression());

        if (number.IsNumeric)
            return new GenericValueLanguage(Math.Sqrt((double)number));
        throw new ArithmeticException($"Não foi possível retornar a raiz de {number.Value}");
    }

    public override GenericValueLanguage VisitParenthesisExpression([NotNull] LanguageParser.ParenthesisExpressionContext context) =>
        Visit(context.arithmetic_expression());

    #region variable_declaration


    public override GenericValueLanguage VisitArithmeticDeclaration([NotNull] LanguageParser.ArithmeticDeclarationContext context)
    {
        var id = context.IDENTIFIER().GetText();
        var value = Visit(context.arithmetic_expression());

        VarAlreadyDeclared(id);

        _memoryLocal.Add(id, value);

        return value;
    }


    public override GenericValueLanguage VisitComparisonDeclaration([NotNull] LanguageParser.ComparisonDeclarationContext context)
    {
        var id = context.IDENTIFIER().GetText();
        var value = Visit(context.comparison_expression());

        VarAlreadyDeclared(id);

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

        VerifyListAlreadyDeclared(identifier);

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

        return empty;

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

        VerifyListAlreadyDeclared(nameArray);

        IDictionary<int, IDictionary<string, GenericValueLanguage>> _dicIndexIntWithDic = new Dictionary<int, IDictionary<string, GenericValueLanguage>>();

        _memoryLocalList[nameArray] = new GenericValueLanguage(_dicIndexIntWithDic);

        return empty;
    }
    private void VarAlreadyDeclared(string id)
    {
        if (_memoryLocal.ContainsKey(id) || _memoryLocalList.ContainsKey(id))
            throw new ArgumentException($"Variável {id} já foi declarada");
    }

    private void VerifyListAlreadyDeclared(string nameArray)
    {
        if (_memoryLocalList.ContainsKey(nameArray))
            throw new ArgumentException($"Lista {nameArray} já foi declarada");
    }

    #endregion

    #region assignment

    public override GenericValueLanguage VisitComparisonAssignment([NotNull] LanguageParser.ComparisonAssignmentContext context)
    {
        string identifier = context.IDENTIFIER().GetText();
        var value = Visit(context.comparison_expression());

        if (!_memoryLocal.ContainsKey(identifier))
            throw new Exception($"Varíavel '{identifier}' não foi declarada");

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
                    return empty;
                default:
                    throw new InvalidOperationException("Operador de atribuição inválido para tipo lista.");
            }
        }
        else if (_memoryLocal.ContainsKey(identifier))
        {
            _memoryLocal[identifier] = OperadorAtribuicao(_memoryLocal[identifier], value, op);
        }
        else throw new Exception($"Varíavel '{identifier}' não foi declarada");

        return empty;
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
        int index = GetIndexArray(context.number_integer()?.GetText(), context.IDENTIFIER(1)?.GetText());
        string propertyArray = context.number_integer() is null ? context.IDENTIFIER(2)?.GetText() : context.IDENTIFIER(1)?.GetText();
        string op = context.assignment_operator().GetText();
        var value = Visit(context.arithmetic_expression());

        if (!_memoryLocalList.ContainsKey(nameArray))
            throw new ArgumentException($"Lista {nameArray} não declarada");

        _memoryLocalList.TryGetValue(nameArray, out GenericValueLanguage array);

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
                    objectIndex[propertyArray] = OperadorAtribuicao(objectIndex[propertyArray], value, op);
            }
        }
        else
        {
            throw new Exception("Propriedade nao informada");
        }

        return value;
    }

    public GenericValueLanguage OperadorAtribuicao(GenericValueLanguage currentValue, GenericValueLanguage value, string op)
    {
        GenericValueLanguage valueNew = op switch
        {
            "=" => value,
            "+=" when currentValue.Value is string currentString => new GenericValueLanguage(currentString + value.AsString()),
            "+=" when currentValue.IsNumeric && value.IsNumeric => currentValue + value,
            "-=" when currentValue.IsNumeric && value.IsNumeric => currentValue - value,
            "*=" when currentValue.IsNumeric && value.IsNumeric => currentValue * value,
            "/=" when currentValue.IsNumeric && value.IsNumeric && (decimal)value != 0 => currentValue / value,
            _ => throw new InvalidOperationException("Operador de atribuição inválido"),
        };
        return valueNew;

    }
    public override GenericValueLanguage VisitVarMemoryValueAssignment([NotNull] LanguageParser.VarMemoryValueAssignmentContext context)
    {
        throw new Exception("Nao pode atribuir valores a variaveis globais.");
    }
    public override GenericValueLanguage VisitListMemoryGlobalValueAssignment([NotNull] LanguageParser.ListMemoryGlobalValueAssignmentContext context)
    {
        throw new Exception("Nao pode atribuir valores a variaveis globais.");
    }
    public override GenericValueLanguage VisitListMemoryValueAssignment([NotNull] LanguageParser.ListMemoryValueAssignmentContext context)
    {
        throw new Exception("Nao pode atribuir valores a variaveis globais.");
    }

    #endregion

    public override GenericValueLanguage VisitAtividadeTabela([NotNull] LanguageParser.AtividadeTabelaContext context)
    {
        string exercicioCaracteristica = Visit(context.exercicio_caracteristica()).AsString();
        string descricaoCaracteristica = Visit(context.descricao_caracteristica()).AsString();
        string atividade = "@Atividade." + descricaoCaracteristica + "." + exercicioCaracteristica;

        return TryGet(atividade, EnumMemory.Memory);
    }



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
    /// 
    public override GenericValueLanguage VisitListValue(LanguageParser.ListValueContext context)
    {
        string nameArray = context.IDENTIFIER(0).GetText();
        int index = GetIndexArray(context.number_integer()?.GetText(), context.IDENTIFIER(1)?.GetText());
        string propertyArray = context.number_integer() is null ? context.IDENTIFIER(2)?.GetText() : context.IDENTIFIER(1)?.GetText();

        // Valida se nameArray contém na varial global _memoryLocalArrayGlobal
        if (_memoryLocalList.TryGetValue(nameArray, out GenericValueLanguage arrayGlobal) && arrayGlobal.Value is object)
        {
            return GetListValue(arrayGlobal, index, propertyArray);
        }

        else throw new Exception("Variavel nao informada");

    }

    private int GetIndexArray(string number, string ident)
    {
        int index = int.TryParse(number, out int num) ? num : -1;
        string index2 = ident;
        if (index < 0)
        {
            if (!_memoryLocal.TryGetValue(index2, out GenericValueLanguage currentValue))
                throw new Exception($"Varíavel '{index2}' não foi declarada");

            index = int.TryParse(currentValue.Value.ToString(), out int num3) ? num3 : -1;
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

        if (id is null) throw new NullReferenceException("Variavel nao informada");

        if (_memoryLocal.TryGetValue(id, out GenericValueLanguage value) || _memoryLocalList.TryGetValue(id, out value))
            return value;
        else throw new Exception($"Variavel '{id}' nao encontrada");
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

        return new GenericValueLanguage(null);
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
            _ => throw new ArgumentException($"Unidade {unit} nao reconhecida"),
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

    private GenericValueLanguage CompareGenericValues(GenericValueLanguage left, GenericValueLanguage right, string op) => op switch
    {
        "==" => Equal(left, right),
        "!=" => NotEqual(left, right),
        ">" => GreaterThan(left, right),
        "<" => LessThan(left, right),
        ">=" => GreaterThanOrEqual(left, right),
        "<=" => LessThanOrEqual(left, right),
        _ => throw new InvalidOperationException($"Operador '{op}' nao reconhecido."),
    };

    private GenericValueLanguage Equal(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (left.IsNumeric && right.IsNumeric)
            return new GenericValueLanguage(left.AsDecimal() == right.AsDecimal());
        else if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() == right.AsDateTime());

        return new GenericValueLanguage(left.Value == right.Value);
    }

    private GenericValueLanguage NotEqual(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (left.IsNumeric && right.IsNumeric)
            return new GenericValueLanguage(left.AsDecimal() != right.AsDecimal());
        else if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() != right.AsDateTime());

        return new GenericValueLanguage(left.Value != right.Value);
    }

    private GenericValueLanguage GreaterThan(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (left.IsNumeric && right.IsNumeric)
            return new GenericValueLanguage(left.AsDecimal() > right.AsDecimal());

        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() > right.AsDateTime());

        if (left.Value is null || right.Value is null)
            return new GenericValueLanguage(false);

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }

    private GenericValueLanguage LessThan(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (left.IsNumeric && right.IsNumeric)
            return new GenericValueLanguage(left.AsDecimal() < right.AsDecimal());

        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() < right.AsDateTime());

        if (left.Value is null || right.Value is null)
            return new GenericValueLanguage(false);

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }

    private GenericValueLanguage GreaterThanOrEqual(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (left.IsNumeric && right.IsNumeric)
            return new GenericValueLanguage(left.AsDecimal() >= right.AsDecimal());

        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() >= right.AsDateTime());

        if (left.Value is null || right.Value is null)
            return new GenericValueLanguage(false);

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }

    private GenericValueLanguage LessThanOrEqual(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (left.IsNumeric && right.IsNumeric)
            return new GenericValueLanguage(left.AsDecimal() <= right.AsDecimal());

        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() <= right.AsDateTime());

        if (left.Value is null || right.Value is null)
            return new GenericValueLanguage(false);

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }
}