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
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ILanguageListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public partial class LanguageBaseListener : ILanguageListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.rule_set"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRule_set([NotNull] LanguageParser.Rule_setContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.rule_set"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRule_set([NotNull] LanguageParser.Rule_setContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.rule_block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRule_block([NotNull] LanguageParser.Rule_blockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.rule_block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRule_block([NotNull] LanguageParser.Rule_blockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.function"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunction([NotNull] LanguageParser.FunctionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.function"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunction([NotNull] LanguageParser.FunctionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>arithmeticAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArithmeticAssignment([NotNull] LanguageParser.ArithmeticAssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>arithmeticAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArithmeticAssignment([NotNull] LanguageParser.ArithmeticAssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>comparisonAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterComparisonAssignment([NotNull] LanguageParser.ComparisonAssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>comparisonAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitComparisonAssignment([NotNull] LanguageParser.ComparisonAssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>returnValue</c>
	/// labeled alternative in <see cref="LanguageParser.return_value"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterReturnValue([NotNull] LanguageParser.ReturnValueContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>returnValue</c>
	/// labeled alternative in <see cref="LanguageParser.return_value"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitReturnValue([NotNull] LanguageParser.ReturnValueContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.conditional"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterConditional([NotNull] LanguageParser.ConditionalContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.conditional"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitConditional([NotNull] LanguageParser.ConditionalContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>thenBlock</c>
	/// labeled alternative in <see cref="LanguageParser.then_block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterThenBlock([NotNull] LanguageParser.ThenBlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>thenBlock</c>
	/// labeled alternative in <see cref="LanguageParser.then_block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitThenBlock([NotNull] LanguageParser.ThenBlockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>elseBlock</c>
	/// labeled alternative in <see cref="LanguageParser.else_block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterElseBlock([NotNull] LanguageParser.ElseBlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>elseBlock</c>
	/// labeled alternative in <see cref="LanguageParser.else_block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitElseBlock([NotNull] LanguageParser.ElseBlockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>orExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOrExpression([NotNull] LanguageParser.OrExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>orExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOrExpression([NotNull] LanguageParser.OrExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ifComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfComparisonExpression([NotNull] LanguageParser.IfComparisonExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ifComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfComparisonExpression([NotNull] LanguageParser.IfComparisonExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>andExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAndExpression([NotNull] LanguageParser.AndExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>andExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAndExpression([NotNull] LanguageParser.AndExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesisIfExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParenthesisIfExpression([NotNull] LanguageParser.ParenthesisIfExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesisIfExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParenthesisIfExpression([NotNull] LanguageParser.ParenthesisIfExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ifEntity</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfEntity([NotNull] LanguageParser.IfEntityContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ifEntity</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfEntity([NotNull] LanguageParser.IfEntityContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ifFunctionSignature</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfFunctionSignature([NotNull] LanguageParser.IfFunctionSignatureContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ifFunctionSignature</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfFunctionSignature([NotNull] LanguageParser.IfFunctionSignatureContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>comparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterComparisonExpression([NotNull] LanguageParser.ComparisonExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>comparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitComparisonExpression([NotNull] LanguageParser.ComparisonExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesisComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParenthesisComparisonExpression([NotNull] LanguageParser.ParenthesisComparisonExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesisComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParenthesisComparisonExpression([NotNull] LanguageParser.ParenthesisComparisonExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.comparison_operator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterComparison_operator([NotNull] LanguageParser.Comparison_operatorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.comparison_operator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitComparison_operator([NotNull] LanguageParser.Comparison_operatorContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>buscarCaracteristica</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBuscarCaracteristica([NotNull] LanguageParser.BuscarCaracteristicaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>buscarCaracteristica</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBuscarCaracteristica([NotNull] LanguageParser.BuscarCaracteristicaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>coalesceFunction</c>
	/// labeled alternative in <see cref="LanguageParser.coalesce_function"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCoalesceFunction([NotNull] LanguageParser.CoalesceFunctionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>coalesceFunction</c>
	/// labeled alternative in <see cref="LanguageParser.coalesce_function"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCoalesceFunction([NotNull] LanguageParser.CoalesceFunctionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>coalesceExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCoalesceExpression([NotNull] LanguageParser.CoalesceExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>coalesceExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCoalesceExpression([NotNull] LanguageParser.CoalesceExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>minusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMinusExpression([NotNull] LanguageParser.MinusExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>minusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMinusExpression([NotNull] LanguageParser.MinusExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesisExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParenthesisExpression([NotNull] LanguageParser.ParenthesisExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesisExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParenthesisExpression([NotNull] LanguageParser.ParenthesisExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>multExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMultExpression([NotNull] LanguageParser.MultExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>multExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMultExpression([NotNull] LanguageParser.MultExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>divExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDivExpression([NotNull] LanguageParser.DivExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>divExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDivExpression([NotNull] LanguageParser.DivExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>plusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPlusExpression([NotNull] LanguageParser.PlusExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>plusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPlusExpression([NotNull] LanguageParser.PlusExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>entityExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEntityExpression([NotNull] LanguageParser.EntityExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>entityExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEntityExpression([NotNull] LanguageParser.EntityExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.tabela_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTabela_caracteristica([NotNull] LanguageParser.Tabela_caracteristicaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.tabela_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTabela_caracteristica([NotNull] LanguageParser.Tabela_caracteristicaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.descricao_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDescricao_caracteristica([NotNull] LanguageParser.Descricao_caracteristicaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.descricao_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDescricao_caracteristica([NotNull] LanguageParser.Descricao_caracteristicaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.valor_fator_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterValor_fator_caracteristica([NotNull] LanguageParser.Valor_fator_caracteristicaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.valor_fator_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitValor_fator_caracteristica([NotNull] LanguageParser.Valor_fator_caracteristicaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.exercicio_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExercicio_caracteristica([NotNull] LanguageParser.Exercicio_caracteristicaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.exercicio_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExercicio_caracteristica([NotNull] LanguageParser.Exercicio_caracteristicaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.coluna_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterColuna_caracteristica([NotNull] LanguageParser.Coluna_caracteristicaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.coluna_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitColuna_caracteristica([NotNull] LanguageParser.Coluna_caracteristicaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.coluna_valor_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterColuna_valor_caracteristica([NotNull] LanguageParser.Coluna_valor_caracteristicaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.coluna_valor_caracteristica"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitColuna_valor_caracteristica([NotNull] LanguageParser.Coluna_valor_caracteristicaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>stringEntity</c>
	/// labeled alternative in <see cref="LanguageParser.text"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStringEntity([NotNull] LanguageParser.StringEntityContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>stringEntity</c>
	/// labeled alternative in <see cref="LanguageParser.text"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStringEntity([NotNull] LanguageParser.StringEntityContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>inteiroEntity</c>
	/// labeled alternative in <see cref="LanguageParser.number"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterInteiroEntity([NotNull] LanguageParser.InteiroEntityContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>inteiroEntity</c>
	/// labeled alternative in <see cref="LanguageParser.number"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitInteiroEntity([NotNull] LanguageParser.InteiroEntityContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>boolEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBoolEntity([NotNull] LanguageParser.BoolEntityContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>boolEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBoolEntity([NotNull] LanguageParser.BoolEntityContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>numberEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNumberEntity([NotNull] LanguageParser.NumberEntityContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>numberEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNumberEntity([NotNull] LanguageParser.NumberEntityContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>dateEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDateEntity([NotNull] LanguageParser.DateEntityContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>dateEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDateEntity([NotNull] LanguageParser.DateEntityContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>variableEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableEntity([NotNull] LanguageParser.VariableEntityContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>variableEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableEntity([NotNull] LanguageParser.VariableEntityContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>varTableColunaEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVarTableColunaEntity([NotNull] LanguageParser.VarTableColunaEntityContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>varTableColunaEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVarTableColunaEntity([NotNull] LanguageParser.VarTableColunaEntityContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>nullEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNullEntity([NotNull] LanguageParser.NullEntityContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>nullEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNullEntity([NotNull] LanguageParser.NullEntityContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
