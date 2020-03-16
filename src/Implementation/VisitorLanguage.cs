using Antlr4.Runtime.Misc;
using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;

public class VisitorLanguage : LanguageBaseVisitor<GenericValueLanguage>
{

    private readonly IDictionary<string, GenericValueLanguage> _memory;
    private readonly IDictionary<string, GenericValueLanguage> _memoryGlobal;
    private readonly IDictionary<string, GenericValueLanguage> _memoryLocal = new Dictionary<string, GenericValueLanguage>();

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

    public override GenericValueLanguage VisitVarArrayEntity([NotNull] LanguageParser.VarArrayEntityContext context)
    {
        string identifier = context.VAR_ARRAY().GetText();
        int start = identifier.IndexOf("[", StringComparison.Ordinal);
        int end = identifier.IndexOf("]", StringComparison.Ordinal) - 1;

        string arrayKey = identifier.Substring(0, start);
        if (_memory.TryGetValue(arrayKey, out GenericValueLanguage value) && value.Value is object[] array)
        {
            string pos = identifier.Substring(start + 1, end - start);
            int index = int.TryParse(identifier.Substring(start + 1, end - start), out int idx) ? idx : _memoryLocal[pos].AsInt();
            if (array.Length > index)
            {
                string propertyKey = identifier.Substring(identifier.IndexOf(".", StringComparison.Ordinal) + 1);
                if (!string.IsNullOrEmpty(propertyKey))
                    return new GenericValueLanguage((array[index] as IDictionary<string, object>).TryGetValue(propertyKey, out object obj) ? obj : null);
                else
                    return new GenericValueLanguage(array[index]);
            }
        }
        return new GenericValueLanguage(null);
    }

    public override GenericValueLanguage VisitVarObjectEntity([NotNull] LanguageParser.VarObjectEntityContext context)
    {
        string identifier = context.VAR_OBJECT().GetText();
        string objectKey = identifier.Substring(0, identifier.IndexOf(".", StringComparison.Ordinal));

        if (_memory.TryGetValue(objectKey, out GenericValueLanguage value) && value.Value is ExpandoObject obj)
        {
            string propertyKey = identifier.Substring(identifier.IndexOf(".", StringComparison.Ordinal) + 1);
            return new GenericValueLanguage((obj as IDictionary<string, object>)[propertyKey]);
        }
        return new GenericValueLanguage(null);
    }

    public override GenericValueLanguage VisitSumFunction([NotNull] LanguageParser.SumFunctionContext context)
    {
        string id = context.VAR_OBJECT().GetText();
        string identifier = id.Substring(0, id.IndexOf(".", StringComparison.Ordinal));
        if (_memory.TryGetValue(identifier, out GenericValueLanguage value) && value.Value is object[] array)
        {
            string propertyIdentifier = id.Substring(id.IndexOf(".", StringComparison.Ordinal) + 1);
            return new GenericValueLanguage(array.Sum(x => decimal.Parse((x as IDictionary<string, object>)[propertyIdentifier]?.ToString() ?? "0.0")));
        }
        else
            return new GenericValueLanguage(0);
    }

    public override GenericValueLanguage VisitMaxFunction([NotNull] LanguageParser.MaxFunctionContext context)
    {
        string id = context.VAR_OBJECT().GetText();
        string identifier = id.Substring(0, id.IndexOf(".", StringComparison.Ordinal));
        if (_memory.TryGetValue(identifier, out GenericValueLanguage value) && value.Value is object[] array)
        {
            string propertyIdentifier = id.Substring(id.IndexOf(".", StringComparison.Ordinal) + 1);
            return new GenericValueLanguage(array.Max(x => decimal.Parse((x as IDictionary<string, object>)[propertyIdentifier]?.ToString() ?? "0.0")));
        }
        else
            return new GenericValueLanguage(0);
    }

    public override GenericValueLanguage VisitMinFunction([NotNull] LanguageParser.MinFunctionContext context)
    {
        string id = context.VAR_OBJECT().GetText();
        string identifier = id.Substring(0, id.IndexOf(".", StringComparison.Ordinal));
        if (_memory.TryGetValue(identifier, out GenericValueLanguage value) && value.Value is object[] array)
        {
            string propertyIdentifier = id.Substring(id.IndexOf(".", StringComparison.Ordinal) + 1);
            return new GenericValueLanguage(array.Min(x => decimal.Parse((x as IDictionary<string, object>)[propertyIdentifier]?.ToString() ?? "0.0")));
        }
        else
            return new GenericValueLanguage(0);
    }

    public override GenericValueLanguage VisitAverageFunction([NotNull] LanguageParser.AverageFunctionContext context)
    {
        string id = context.VAR_OBJECT().GetText();
        string identifier = id.Substring(0, id.IndexOf(".", StringComparison.Ordinal));
        if (_memory.TryGetValue(identifier, out GenericValueLanguage value) && value.Value is object[] array)
        {
            string propertyIdentifier = id.Substring(id.IndexOf(".", StringComparison.Ordinal) + 1);
            return new GenericValueLanguage(array.Average(x => decimal.Parse((x as IDictionary<string, object>)[propertyIdentifier]?.ToString() ?? "0.0")));
        }
        else
            return new GenericValueLanguage(0);
    }

    public override GenericValueLanguage VisitAbsFunction([NotNull] LanguageParser.AbsFunctionContext context) =>
        new GenericValueLanguage(Math.Abs(Visit(context.arithmetic_expression()).AsDecimal()));

    public override GenericValueLanguage VisitSumIfFunction([NotNull] LanguageParser.SumIfFunctionContext context)
    {
        string identifier = context.VAR_OBJECT().GetText();
        string objectKey = identifier.Substring(0, identifier.IndexOf(".", StringComparison.Ordinal));
        decimal sum = 0m;

        if (_memory.TryGetValue(objectKey, out GenericValueLanguage value) && value.Value is object[] array)
        {
            string propertyKey = identifier.Substring(identifier.IndexOf(".", StringComparison.Ordinal) + 1);
            string op = context.comparison_operator().GetText();
            foreach (IDictionary<string, object> item in array)
            {
                var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out object leftValue) ? new GenericValueLanguage(leftValue) : Visit(context.arithmetic_expression(0));
                var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out object rightValue) ? new GenericValueLanguage(rightValue) : Visit(context.arithmetic_expression(1));

                if (CompareGenericValues(left, right, op).AsBoolean())
                    sum += item[propertyKey].IsNumericType() ? decimal.Parse(item[propertyKey]?.ToString() ?? "0") : 0m;
            }
        }

        return new GenericValueLanguage(sum);
    }

    public override GenericValueLanguage VisitCountIfFunction([NotNull] LanguageParser.CountIfFunctionContext context)
    {
        string identifier = context.VAR_PRIMARY().GetText();
        int count = 0;

        if (_memory.TryGetValue(identifier, out GenericValueLanguage value) && value.Value is object[] array)
        {
            string op = context.comparison_operator().GetText();
            foreach (IDictionary<string, object> item in array)
            {
                var left = item.TryGetValue(context.arithmetic_expression(0).GetText(), out object leftValue) ? new GenericValueLanguage(leftValue) : Visit(context.arithmetic_expression(0));
                var right = item.TryGetValue(context.arithmetic_expression(1).GetText(), out object rightValue) ? new GenericValueLanguage(rightValue) : Visit(context.arithmetic_expression(1));

                if (CompareGenericValues(left, right, op).AsBoolean())
                    count++;
            }
        }

        return new GenericValueLanguage(count);
    }

    public override GenericValueLanguage VisitParametroFunction([NotNull] LanguageParser.ParametroFunctionContext context)
    {
        string nome = Visit(context.text()).AsString();
        int? exercicio = int.TryParse(context.number_integer()?.GetText(), out int num) ? num : default(int?);

        var value = (_memoryGlobal["@ParametroVlrs"].Value as object[]).Where(x =>
        {
            var dict = x as IDictionary<string, object>;
            return dict["NomeParam"].ToString() == nome && (!exercicio.HasValue || (int.TryParse(dict["Exercicio"]?.ToString(), out int ex) ? ex == exercicio : false));
        }).FirstOrDefault();

        return new GenericValueLanguage(value != null ? (value as IDictionary<string, object>)["Valor"] : null);
    }

    public override GenericValueLanguage VisitParametroIntervaloFunction([NotNull] LanguageParser.ParametroIntervaloFunctionContext context)
    {
        string nome = Visit(context.text(0)).AsString();
        string valor = Visit(context.text(1)).AsString();
        int? exercicio = int.TryParse(context.number_integer()?.GetText(), out int num) ? num : default(int?);

        var value = (_memoryGlobal["@ParametroVlrs"].Value as object[]).Where(x =>
        {
            var dict = x as IDictionary<string, object>;
            return dict["NomeParam"].ToString() == nome && dict["Codigo"].ToString() == valor &&
                (!exercicio.HasValue || (int.TryParse(dict["Exercicio"].ToString(), out int ex) ? ex == exercicio : false));
        }).FirstOrDefault();

        return new GenericValueLanguage(value != null ? (value as IDictionary<string, object>)["Valor"] : null);
    }

    public override GenericValueLanguage VisitParametroCodigoFunction([NotNull] LanguageParser.ParametroCodigoFunctionContext context)
    {
        string nome = Visit(context.text(0)).AsString();
        string codigo = Visit(context.text(1)).AsString();
        int? exercicio = int.TryParse(context.number_integer()?.GetText(), out int num) ? num : default(int?);

        var value = (_memoryGlobal["@ParametroVlrs"].Value as object[]).Where(x =>
        {
            var dict = x as IDictionary<string, object>;
            return dict["NomeParam"].ToString() == nome && dict["Codigo"].ToString() == codigo &&
                (!exercicio.HasValue || (int.TryParse(dict["Exercicio"].ToString(), out int ex) ? ex == exercicio : false));
        }).FirstOrDefault();

        return new GenericValueLanguage(value != null ? (value as IDictionary<string, object>)["Valor"] : null);
    }

    public override GenericValueLanguage VisitLengthFunction([NotNull] LanguageParser.LengthFunctionContext context)
    {
        var identifier = context.VAR_PRIMARY().GetText();
        return new GenericValueLanguage(_memory.TryGetValue(identifier, out GenericValueLanguage value) && value.Value is object[] array ? array.Length : 0);
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

        return new GenericValueLanguage(string.Empty);
    }

    public override GenericValueLanguage VisitCaracteristicaTabela(LanguageParser.CaracteristicaTabelaContext context)
    {
        var tabela = Visit(context.tabela_caracteristica());
        var descricao = Visit(context.descricao_caracteristica());

        if (_memory.TryGetValue($"@{tabela.Value}.{descricao.Value}", out GenericValueLanguage value) && value.Value is object[] array)
        {
            decimal resultado = 0;
            decimal.TryParse((array[0] as IDictionary<string, object>)["Valor"]?.ToString(), out resultado);
            return new GenericValueLanguage(resultado);
        }


        return new GenericValueLanguage(null);
    }

    public override GenericValueLanguage VisitCaracteristica(LanguageParser.CaracteristicaContext context)
    {
        var descricao = Visit(context.descricao_caracteristica());

        if (_memoryGlobal.TryGetValue($"@Caracteristica.{descricao.Value}", out GenericValueLanguage value) && value.Value is ExpandoObject array)
        {

            decimal resultado = 0;
            decimal.TryParse((array as IDictionary<string, object>)["Valor"]?.ToString(), out resultado);
            return new GenericValueLanguage(resultado);
        }

        return new GenericValueLanguage(null);
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
        for (var index = 0; index < context.rule_block().Count(); index++)
            Visit(context.rule_block(index));

        return new GenericValueLanguage(string.Empty);
    }

    public override GenericValueLanguage VisitElseBlock([NotNull] LanguageParser.ElseBlockContext context)
    {
        for (var index = 0; index < context.rule_block().Count(); index++)
            Visit(context.rule_block(index));

        return new GenericValueLanguage(string.Empty);
    }

    public override GenericValueLanguage VisitParenthesisIfExpression([NotNull] LanguageParser.ParenthesisIfExpressionContext context) =>
        Visit(context.if_expression());

    public override GenericValueLanguage VisitComparisonExpression([NotNull] LanguageParser.ComparisonExpressionContext context)
    {
        var left = Visit(context.arithmetic_expression(0));
        var right = Visit(context.arithmetic_expression(1));
        string op = context.comparison_operator().GetText();

        return CompareGenericValues(left, right, op);
    }

    public override GenericValueLanguage VisitParenthesisComparisonExpression([NotNull] LanguageParser.ParenthesisComparisonExpressionContext context) =>
        Visit(context.comparison_expression());

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

    public override GenericValueLanguage VisitArithmeticAssignment([NotNull] LanguageParser.ArithmeticAssignmentContext context)
    {
        string identifier = context.IDENTIFIER().GetText();
        var value = Visit(context.arithmetic_expression());
        string op = context.assignment_operator().GetText();

        if (!_memoryLocal.TryGetValue(identifier, out GenericValueLanguage currentValue))
            throw new Exception($"Varíavel '{identifier}' não foi declarada");

        switch (op)
        {
            case "=":
                _memoryLocal[identifier] = value;
                break;
            case "+=" when currentValue.Value is string currentString:
                _memoryLocal[identifier] = new GenericValueLanguage(currentString + value.AsString());
                break;
            case "+=" when currentValue.IsNumeric && value.IsNumeric:
                _memoryLocal[identifier] = currentValue + value;
                break;
            case "-=" when currentValue.IsNumeric && value.IsNumeric:
                _memoryLocal[identifier] = currentValue - value;
                break;
            case "*=" when currentValue.IsNumeric && value.IsNumeric:
                _memoryLocal[identifier] = currentValue * value;
                break;
            case "/=" when currentValue.IsNumeric && value.IsNumeric && (decimal)value != 0:
                _memoryLocal[identifier] = currentValue / value;
                break;
            default:
                throw new InvalidOperationException("Operador de atribuição inválido");
        }

        return _memoryLocal[identifier];
    }

    public override GenericValueLanguage VisitComparisonAssignment([NotNull] LanguageParser.ComparisonAssignmentContext context)
    {
        string identifier = context.IDENTIFIER().GetText();
        var value = Visit(context.comparison_expression());

        if (!_memoryLocal.ContainsKey(identifier))
            throw new Exception($"Varíavel '{identifier}' não foi declarada");

        _memoryLocal[identifier] = value;

        return _memoryLocal[identifier];
    }

    public override GenericValueLanguage VisitArithmeticDeclaration([NotNull] LanguageParser.ArithmeticDeclarationContext context)
    {
        var id = context.IDENTIFIER().GetText();
        var value = Visit(context.arithmetic_expression());

        if (_memoryLocal.ContainsKey(id))
            throw new ArgumentException($"Variável {id} já foi declarada");
        _memoryLocal.Add(id, value);

        return value;
    }

    public override GenericValueLanguage VisitComparisonDeclaration([NotNull] LanguageParser.ComparisonDeclarationContext context)
    {
        var id = context.IDENTIFIER().GetText();
        var value = Visit(context.comparison_expression());

        if (_memoryLocal.ContainsKey(id))
            throw new ArgumentException($"Variável {id} já foi declarada");
        _memoryLocal.Add(id, value);

        return value;
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
        new GenericValueLanguage(context.GetText().Replace("\"", string.Empty));

    public override GenericValueLanguage VisitVariableEntity(LanguageParser.VariableEntityContext context)
    {
        var id = context.GetText();

        if (id is null) throw new NullReferenceException("Variavel nao informada");

        if (_memoryLocal.TryGetValue(id, out GenericValueLanguage value))
            return value;
        else throw new Exception("Variavel nao informada");
    }

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
        decimal number = Visit(context.arithmetic_expression(0)).AsDecimal();
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

    public override GenericValueLanguage VisitTodayFunction([NotNull] LanguageParser.TodayFunctionContext context) =>
        new GenericValueLanguage(DateTime.Today);

    public override GenericValueLanguage VisitNowFunction([NotNull] LanguageParser.NowFunctionContext context) =>
        new GenericValueLanguage(DateTime.Now);

    public override GenericValueLanguage VisitDateDifFunction([NotNull] LanguageParser.DateDifFunctionContext context)
    {
        var left = Visit(context.entity(0));
        var right = Visit(context.entity(1));
        string unit = context.date_unit().GetText();

        switch (unit)
        {
            case "ANO":
                return new GenericValueLanguage(Math.Abs((right.AsDateTime() - left.AsDateTime()).Days / 365));
            case "MES":
                return new GenericValueLanguage(Math.Abs(right.AsDateTime().MonthDifference(left.AsDateTime())));
            case "DIA":
                return new GenericValueLanguage((right.AsDateTime() - left.AsDateTime()).Days);
            default:
                throw new ArgumentException($"Unidade {unit} nao reconhecida");
        }
    }

    public override GenericValueLanguage VisitGetDayFunction([NotNull] LanguageParser.GetDayFunctionContext context) =>
        new GenericValueLanguage(Visit(context.entity()).AsDateTime().Day);

    public override GenericValueLanguage VisitGetMonthFunction([NotNull] LanguageParser.GetMonthFunctionContext context) =>
        new GenericValueLanguage(Visit(context.entity()).AsDateTime().Month);

    public override GenericValueLanguage VisitGetYearFunction([NotNull] LanguageParser.GetYearFunctionContext context) =>
        new GenericValueLanguage(Visit(context.entity()).AsDateTime().Year);

    private GenericValueLanguage CompareGenericValues(GenericValueLanguage left, GenericValueLanguage right, string op)
    {
        switch (op)
        {
            case "==": return Equal(left, right);
            case "!=": return NotEqual(left, right);
            case ">": return GreaterThan(left, right);
            case "<": return LessThan(left, right);
            case ">=": return GreaterThanOrEqual(left, right);
            case "<=": return LessThanOrEqual(left, right);
            default:
                throw new InvalidOperationException($"Operador '{op}' nao reconhecido.");
        }
    }

    private GenericValueLanguage Equal(GenericValueLanguage left, GenericValueLanguage right)
    {
        var localCulture = new CultureInfo("pt-BR");

        if (decimal.TryParse(left.Value.ToString(), out decimal leftDecimal) && decimal.TryParse(right.Value.ToString(), out decimal rightDecimal))
            return new GenericValueLanguage(leftDecimal == rightDecimal);
        else if (DateTime.TryParse(left.Value.ToString(), localCulture, DateTimeStyles.None, out DateTime leftDate) &&
                DateTime.TryParse(right.Value.ToString(), localCulture, DateTimeStyles.None, out DateTime rightDate))
            return new GenericValueLanguage(leftDate == rightDate);
        return new GenericValueLanguage(left.Value == right.Value);
    }

    private GenericValueLanguage NotEqual(GenericValueLanguage left, GenericValueLanguage right)
    {
        var localCulture = new CultureInfo("pt-BR");

        if (decimal.TryParse(left.Value.ToString(), out decimal leftDecimal) && decimal.TryParse(right.Value.ToString(), out decimal rightDecimal))
            return new GenericValueLanguage(leftDecimal != rightDecimal);
        else if (DateTime.TryParse(left.Value.ToString(), localCulture, DateTimeStyles.None, out DateTime leftDate) &&
                DateTime.TryParse(right.Value.ToString(), localCulture, DateTimeStyles.None, out DateTime rightDate))
            return new GenericValueLanguage(leftDate != rightDate);
        return new GenericValueLanguage(left.Value != right.Value);
    }

    private GenericValueLanguage GreaterThan(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (decimal.TryParse(left.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal leftDecimal) &&
            decimal.TryParse(right.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rightDecimal))
            return new GenericValueLanguage(leftDecimal > rightDecimal);

        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() > right.AsDateTime());

        if (left.Value is null || right.Value is null)
            return new GenericValueLanguage(false);

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }

    private GenericValueLanguage LessThan(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (decimal.TryParse(left.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal leftDecimal) &&
            decimal.TryParse(right.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rightDecimal))
            return new GenericValueLanguage(leftDecimal < rightDecimal);

        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() < right.AsDateTime());

        if (left.Value is null || right.Value is null)
            return new GenericValueLanguage(false);

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }

    private GenericValueLanguage GreaterThanOrEqual(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (decimal.TryParse(left.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal leftDecimal) &&
            decimal.TryParse(right.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rightDecimal))
            return new GenericValueLanguage(leftDecimal >= rightDecimal);

        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() >= right.AsDateTime());

        if (left.Value is null || right.Value is null)
            return new GenericValueLanguage(false);

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }

    private GenericValueLanguage LessThanOrEqual(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (decimal.TryParse(left.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal leftDecimal) &&
            decimal.TryParse(right.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rightDecimal))
            return new GenericValueLanguage(leftDecimal <= rightDecimal);

        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() <= right.AsDateTime());

        if (left.Value is null || right.Value is null)
            return new GenericValueLanguage(false);

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }
}