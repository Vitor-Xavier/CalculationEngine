
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;
public class VisitorLanguage : LanguageBaseVisitor<GenericValueLanguage> {

    public Dictionary<string, GenericValueLanguage> memory = new Dictionary<string, GenericValueLanguage>();

    public bool _blockRule { get; set; }
    public override GenericValueLanguage VisitPlusExpression(LanguageParser.PlusExpressionContext ctx)
    {
        var left = Visit(ctx.arithmetic_expression(0));
        var right = Visit(ctx.arithmetic_expression(1));

        var teste = new GenericValueLanguage(left.AsDouble() + right.AsDouble());
        return teste;
    }

    public override GenericValueLanguage VisitRule_block(LanguageParser.Rule_blockContext context){
        // if(_blockRule && context.marker() == null)
        // return GenericValueLanguage.VOID;

        return base.VisitRule_block(context);
    }

    public override GenericValueLanguage VisitAssignment(LanguageParser.AssignmentContext ctx)
    {
        var id = ctx.IDENTIFIER().GetText();

        var value = Visit(ctx.arithmetic_expression());

        if (id.Contains("_VALOR") && value.IsDouble())
            value = new GenericValueLanguage(Math.Round(value.AsDouble(), 2, MidpointRounding.AwayFromZero));

        if (ctx.CONST() != null)
        {
            value = new GenericValueLanguage(value.Value, true);
        }
        else if (value != null && value.Contant)
        {
            value = new GenericValueLanguage(value.Value, false);
        }

        memory.Add(id, value);

       
        return value;
    }


    public override GenericValueLanguage VisitReturn_value(LanguageParser.Return_valueContext ctx)
    {

        var id = ctx.IDENTIFIER() != null ? ctx.IDENTIFIER().GetText() : null;

        
        GenericValueLanguage retono;

        if(id == null) return new GenericValueLanguage(string.Empty, false);
        
        memory.TryGetValue(id, out retono);

        if(retono == null)
        return new GenericValueLanguage(id, false);

        return retono;
    }


    public override GenericValueLanguage VisitBoolEntity(LanguageParser.BoolEntityContext ctx)
    {
        return new GenericValueLanguage(Boolean.Parse(ctx.GetText()));
    }

    public override GenericValueLanguage VisitNumberEntity(LanguageParser.NumberEntityContext ctx)
    {
        return new GenericValueLanguage(Double.Parse(ctx.GetText(), CultureInfo.InvariantCulture));
    }

    public override GenericValueLanguage VisitDateEntity(LanguageParser.DateEntityContext context)
    {
        var culture = new CultureInfo("pt-BR", true);
        DateTime dateTime;
        DateTime.TryParse(context.GetText(), culture, DateTimeStyles.None, out dateTime);
        return new GenericValueLanguage(dateTime);
    }

    public override GenericValueLanguage VisitStringEntity(LanguageParser.StringEntityContext ctx)
    {
        var parsedString = ctx.GetText().Replace("\"", "");
        return new GenericValueLanguage(parsedString);
    }

    public override GenericValueLanguage VisitVariableEntity(LanguageParser.VariableEntityContext ctx)
    {

        var id = ctx.GetText();

        return new GenericValueLanguage(id);
    }
}
