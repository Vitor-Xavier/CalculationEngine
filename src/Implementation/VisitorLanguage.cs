using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class VisitorLanguage : LanguageBaseVisitor<GenericValueLanguage>
{
    private readonly Dictionary<string, GenericValueLanguage> _memory;

    public VisitorLanguage(Dictionary<string, GenericValueLanguage> memory)
    {
        _memory = memory ?? new Dictionary<string, GenericValueLanguage>();
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

        return new GenericValueLanguage(string.Empty, false);
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

        return new GenericValueLanguage(left && right);
    }

    public override GenericValueLanguage VisitThenBlock([NotNull] LanguageParser.ThenBlockContext context)
    {
        for (var index = 0; index < context.rule_block().Count(); index++)
            Visit(context.rule_block(index));

        return new GenericValueLanguage(string.Empty, false);
    }

    public override GenericValueLanguage VisitElseBlock([NotNull] LanguageParser.ElseBlockContext context)
    {
        for (var index = 0; index < context.rule_block().Count(); index++)
            Visit(context.rule_block(index));

        return new GenericValueLanguage(string.Empty, false);
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
                    return new GenericValueLanguage(left == right);
                }
            case "!=":
                {
                    if (double.TryParse(left.Value.ToString(), out double leftDouble) && double.TryParse(right.Value.ToString(), out double rightDouble))
                        return new GenericValueLanguage(leftDouble != rightDouble);
                    else if (DateTime.TryParse(left.Value.ToString(), localCulture, DateTimeStyles.None, out DateTime leftDate) &&
                            DateTime.TryParse(right.Value.ToString(), localCulture, DateTimeStyles.None, out DateTime rightDate))
                        return new GenericValueLanguage(leftDate != rightDate);
                    return new GenericValueLanguage(left != right);
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

        return new GenericValueLanguage(left.AsDouble() * right.AsDouble());
    }

    public override GenericValueLanguage VisitDivExpression([NotNull] LanguageParser.DivExpressionContext context)
    {
        var left = Visit(context.arithmetic_expression(0));
        var right = Visit(context.arithmetic_expression(1));

        if (right.AsDouble() == 0)
            throw new Exception("Nao e possivel dividir por zero.");

        return new GenericValueLanguage(left.AsDouble() / right.AsDouble());
    }

    public override GenericValueLanguage VisitCaracteristicaTabela(LanguageParser.CaracteristicaTabelaContext context)
    {
        var tabela = Visit(context.tabela_caracteristica());
        var descricao = Visit(context.descricao_caracteristica());

        _memory.TryGetValue($"@{tabela.Value}.{descricao.Value}[0].Valor", out GenericValueLanguage value);
        return value ?? new GenericValueLanguage(null);
    }

    public override GenericValueLanguage VisitParenthesisExpression([NotNull] LanguageParser.ParenthesisExpressionContext context) =>
        Visit(context.arithmetic_expression());

    public override GenericValueLanguage VisitVarTableColunaEntity([NotNull] LanguageParser.VarTableColunaEntityContext context)
    {
        var id = context.VAR_TABLE_COLUNA()?.GetText();

        if (id is null) throw new Exception("Nome da variavel global nao encontrado");

        return _memory.TryGetValue(id, out GenericValueLanguage retono) ? retono : new GenericValueLanguage(null);
    }

    public override GenericValueLanguage VisitArithmeticAssignment([NotNull] LanguageParser.ArithmeticAssignmentContext context)
    {
        var id = context.IDENTIFIER().GetText();

        var value = Visit(context.arithmetic_expression());

      
        value = new GenericValueLanguage(value.Value, true);
        

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

        value = new GenericValueLanguage(value.Value, true);

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
        new GenericValueLanguage(context.GetText().Replace("\"", ""));

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