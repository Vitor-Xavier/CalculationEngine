using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;

public class VisitorLanguage : LanguageBaseVisitor<GenericValueLanguage>
{
    private readonly IDictionary<string, GenericValueLanguage> _memory;

    public VisitorLanguage(IDictionary<string, GenericValueLanguage> memory)
    {
        _memory = memory ?? new Dictionary<string, GenericValueLanguage>();
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
            int index = int.Parse(identifier.Substring(start + 1, end - start));
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
            string propertyKey = identifier.Substring(identifier.IndexOf(".", StringComparison.Ordinal) + 1); ;
            return new GenericValueLanguage((obj as IDictionary<string, object>)[propertyKey]);
        }
        return new GenericValueLanguage(null);
    }

    public override GenericValueLanguage VisitConditional([NotNull] LanguageParser.ConditionalContext context)
    {
        if (bool.Parse(Visit(context.if_expression()).Value.ToString()))
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

        if (_memory.TryGetValue($"@{tabela.Value}.{descricao.Value}", out GenericValueLanguage value) && value.Value is object[] array){
            double resultado = 0;
            double.TryParse((array[0] as IDictionary<string, object>)["Valor"].ToString(), out resultado);
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
        var localCulture = new CultureInfo("pt-BR");

        switch (op)
        {
            case "==":
                {
                    if (double.TryParse(left.Value.ToString(), out double leftDouble) && double.TryParse(right.Value.ToString(), out double rightDouble))
                        return new GenericValueLanguage(leftDouble == rightDouble);
                    else if (DateTime.TryParse(left.Value.ToString(), localCulture, DateTimeStyles.None, out DateTime leftDate) &&
                            DateTime.TryParse(right.Value.ToString(), localCulture, DateTimeStyles.None, out DateTime rightDate))
                        return new GenericValueLanguage(leftDate == rightDate);
                    return new GenericValueLanguage(left.Value == right.Value);
                }
            case "!=":
                {
                    if (double.TryParse(left.Value.ToString(), out double leftDouble) && double.TryParse(right.Value.ToString(), out double rightDouble))
                        return new GenericValueLanguage(leftDouble != rightDouble);
                    else if (DateTime.TryParse(left.Value.ToString(), localCulture, DateTimeStyles.None, out DateTime leftDate) &&
                            DateTime.TryParse(right.Value.ToString(), localCulture, DateTimeStyles.None, out DateTime rightDate))
                        return new GenericValueLanguage(leftDate != rightDate);
                    return new GenericValueLanguage(left.Value != right.Value);
                }
            case ">":
                return GreaterThan(left, right);
            case ">=":
                return GreaterThanOrEqual(left, right);
            case "<":
                return LessThan(left, right);
            case "<=":
                return LessThanOrEqual(left, right);
            default:
                throw new InvalidOperationException($"Operador '{op}' nao reconhecido.");
        }
    }

    public override GenericValueLanguage VisitParenthesisComparisonExpression([NotNull] LanguageParser.ParenthesisComparisonExpressionContext context) =>
        Visit(context.comparison_expression());

    public override GenericValueLanguage VisitPlusExpression(LanguageParser.PlusExpressionContext context)
    {
        var left = Visit(context.arithmetic_expression(0));
        var right = Visit(context.arithmetic_expression(1));

        return new GenericValueLanguage(left.AsDouble() + right.AsDouble());
    }

    public override GenericValueLanguage VisitMinusExpression([NotNull] LanguageParser.MinusExpressionContext context)
    {
        var left = Visit(context.arithmetic_expression(0));
        var right = Visit(context.arithmetic_expression(1));

        return new GenericValueLanguage(left.AsDouble() - right.AsDouble());
    }

    public override GenericValueLanguage VisitMultExpression([NotNull] LanguageParser.MultExpressionContext context)
    {
        var left = Visit(context.arithmetic_expression(0));
        var right = Visit(context.arithmetic_expression(1));

        var value = new GenericValueLanguage(Math.Round(left.AsDouble() * right.AsDouble(),4));
        return value;
  }

    public override GenericValueLanguage VisitDivExpression([NotNull] LanguageParser.DivExpressionContext context)
    {
        var left = Visit(context.arithmetic_expression(0));
        var right = Visit(context.arithmetic_expression(1));

        if (right.AsDouble() == 0)
            throw new Exception("Nao e possivel dividir por zero.");

        return new GenericValueLanguage(Math.Round(left.AsDouble() / right.AsDouble(),4));
    }

    public override GenericValueLanguage VisitParenthesisExpression([NotNull] LanguageParser.ParenthesisExpressionContext context) =>
        Visit(context.arithmetic_expression());

    public override GenericValueLanguage VisitArithmeticAssignment([NotNull] LanguageParser.ArithmeticAssignmentContext context)
    {
        var id = context.IDENTIFIER().GetText();
        var value = Visit(context.arithmetic_expression());

        if (_memory.ContainsKey(id))
            _memory[id] = value;
        else
            _memory.Add(id, value);

        return value;
    }

    public override GenericValueLanguage VisitComparisonAssignment([NotNull] LanguageParser.ComparisonAssignmentContext context)
    {
        var id = context.IDENTIFIER().GetText();

        var value = Visit(context.comparison_expression());

        if (_memory.ContainsKey(id))
            _memory[id] = value;
        else
            _memory.Add(id, value);

        return value;
    }

    public override GenericValueLanguage VisitReturnValue([NotNull] LanguageParser.ReturnValueContext context) =>
        Visit(context.arithmetic_expression());

    public override GenericValueLanguage VisitBoolEntity(LanguageParser.BoolEntityContext context) =>
        new GenericValueLanguage(bool.Parse(context.GetText()));

    public override GenericValueLanguage VisitNumberDecimalEntity(LanguageParser.NumberDecimalEntityContext context) =>
        new GenericValueLanguage(double.Parse(context.GetText(), CultureInfo.InvariantCulture));

    public override GenericValueLanguage VisitNumberIntegerEntity(LanguageParser.NumberIntegerEntityContext context) =>
        new GenericValueLanguage(int.Parse(context.GetText(), CultureInfo.InvariantCulture));

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

        if (_memory.TryGetValue(id, out GenericValueLanguage value))
            return value;
        else throw new Exception("Variavel nao informada");
    }

    public override GenericValueLanguage VisitCoalesceFunction([NotNull] LanguageParser.CoalesceFunctionContext context)
    {
        int parameters = context.GetText().Count(x => x == ',') + 1;
        for (int i = 0; i < parameters; i++)
        {
            var value = Visit(context.entity(i));
            if (value?.Value != null)
                return value;
        }
        return new GenericValueLanguage(null);
    }

    public override GenericValueLanguage VisitCoalesceExpression([NotNull] LanguageParser.CoalesceExpressionContext context) =>
        Visit(context.coalesce_function());


    private GenericValueLanguage GreaterThan(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() > right.AsDateTime());
        else
            return new GenericValueLanguage(left.AsDouble() > right.AsDouble());

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }

    private GenericValueLanguage LessThan(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (left.IsDouble() && right.IsDouble())
            return new GenericValueLanguage(left.AsDouble() < right.AsDouble());

        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() < right.AsDateTime());

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }

    private GenericValueLanguage GreaterThanOrEqual(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (left.IsDouble() && right.IsDouble())
            return new GenericValueLanguage(left.AsDouble() >= right.AsDouble());

        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() >= right.AsDateTime());

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }

    private GenericValueLanguage LessThanOrEqual(GenericValueLanguage left, GenericValueLanguage right)
    {
        if (left.IsDouble() && right.IsDouble())
            return new GenericValueLanguage(left.AsDouble() <= right.AsDouble());

        if (left.IsDate() && right.IsDate())
            return new GenericValueLanguage(left.AsDateTime() <= right.AsDateTime());

        throw new InvalidOperationException("Comparando valores incompatíveis.");
    }
}