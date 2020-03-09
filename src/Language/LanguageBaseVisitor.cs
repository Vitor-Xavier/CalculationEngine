//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Language.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ILanguageVisitor{Result}"/>,
/// which can be extended to create a visitor which only needs to handle a subset
/// of the available methods.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public partial class LanguageBaseVisitor<Result> : AbstractParseTreeVisitor<Result>, ILanguageVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.rule_set"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitRule_set([NotNull] LanguageParser.Rule_setContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.rule_block"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitRule_block([NotNull] LanguageParser.Rule_blockContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>arithmeticAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitArithmeticAssignment([NotNull] LanguageParser.ArithmeticAssignmentContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>comparisonAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitComparisonAssignment([NotNull] LanguageParser.ComparisonAssignmentContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>returnValue</c>
	/// labeled alternative in <see cref="LanguageParser.return_value"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitReturnValue([NotNull] LanguageParser.ReturnValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.conditional"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitConditional([NotNull] LanguageParser.ConditionalContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>thenBlock</c>
	/// labeled alternative in <see cref="LanguageParser.then_block"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitThenBlock([NotNull] LanguageParser.ThenBlockContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>elseBlock</c>
	/// labeled alternative in <see cref="LanguageParser.else_block"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitElseBlock([NotNull] LanguageParser.ElseBlockContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>orExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitOrExpression([NotNull] LanguageParser.OrExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>ifComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitIfComparisonExpression([NotNull] LanguageParser.IfComparisonExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>andExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitAndExpression([NotNull] LanguageParser.AndExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthesisIfExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitParenthesisIfExpression([NotNull] LanguageParser.ParenthesisIfExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>ifEntity</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitIfEntity([NotNull] LanguageParser.IfEntityContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>comparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitComparisonExpression([NotNull] LanguageParser.ComparisonExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthesisComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitParenthesisComparisonExpression([NotNull] LanguageParser.ParenthesisComparisonExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.comparison_operator"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitComparison_operator([NotNull] LanguageParser.Comparison_operatorContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>caracteristicaTabela</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCaracteristicaTabela([NotNull] LanguageParser.CaracteristicaTabelaContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>buscarCaracteristica</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitBuscarCaracteristica([NotNull] LanguageParser.BuscarCaracteristicaContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>parametroFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitParametroFunction([NotNull] LanguageParser.ParametroFunctionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>sumFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSumFunction([NotNull] LanguageParser.SumFunctionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>lengthFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitLengthFunction([NotNull] LanguageParser.LengthFunctionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>coalesceFunction</c>
	/// labeled alternative in <see cref="LanguageParser.coalesce_function"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCoalesceFunction([NotNull] LanguageParser.CoalesceFunctionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>coalesceExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCoalesceExpression([NotNull] LanguageParser.CoalesceExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>minusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitMinusExpression([NotNull] LanguageParser.MinusExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthesisExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitParenthesisExpression([NotNull] LanguageParser.ParenthesisExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>multExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitMultExpression([NotNull] LanguageParser.MultExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>divExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitDivExpression([NotNull] LanguageParser.DivExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>plusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPlusExpression([NotNull] LanguageParser.PlusExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>ifFunctionSignature</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitIfFunctionSignature([NotNull] LanguageParser.IfFunctionSignatureContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>entityExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitEntityExpression([NotNull] LanguageParser.EntityExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.tabela_caracteristica"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTabela_caracteristica([NotNull] LanguageParser.Tabela_caracteristicaContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.descricao_caracteristica"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitDescricao_caracteristica([NotNull] LanguageParser.Descricao_caracteristicaContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.valor_fator_caracteristica"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitValor_fator_caracteristica([NotNull] LanguageParser.Valor_fator_caracteristicaContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.codigo_caracteristica"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCodigo_caracteristica([NotNull] LanguageParser.Codigo_caracteristicaContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.exercicio_caracteristica"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExercicio_caracteristica([NotNull] LanguageParser.Exercicio_caracteristicaContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.coluna_caracteristica"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitColuna_caracteristica([NotNull] LanguageParser.Coluna_caracteristicaContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.coluna_valor_caracteristica"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitColuna_valor_caracteristica([NotNull] LanguageParser.Coluna_valor_caracteristicaContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>boolEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitBoolEntity([NotNull] LanguageParser.BoolEntityContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>numberDecimalEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitNumberDecimalEntity([NotNull] LanguageParser.NumberDecimalEntityContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>numberIntegerEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitNumberIntegerEntity([NotNull] LanguageParser.NumberIntegerEntityContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>dateEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitDateEntity([NotNull] LanguageParser.DateEntityContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>variableEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitVariableEntity([NotNull] LanguageParser.VariableEntityContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>varPrimaryEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitVarPrimaryEntity([NotNull] LanguageParser.VarPrimaryEntityContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>varArrayEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitVarArrayEntity([NotNull] LanguageParser.VarArrayEntityContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>varObjectEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitVarObjectEntity([NotNull] LanguageParser.VarObjectEntityContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>nullEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitNullEntity([NotNull] LanguageParser.NullEntityContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>stringEntity</c>
	/// labeled alternative in <see cref="LanguageParser.text"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStringEntity([NotNull] LanguageParser.StringEntityContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.number_integer"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitNumber_integer([NotNull] LanguageParser.Number_integerContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.number_decimal"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitNumber_decimal([NotNull] LanguageParser.Number_decimalContext context) { return VisitChildren(context); }
}
