//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from .\Language.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="LanguageParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface ILanguageListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.rule_set"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRule_set([NotNull] LanguageParser.Rule_setContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.rule_set"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRule_set([NotNull] LanguageParser.Rule_setContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.rule_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRule_block([NotNull] LanguageParser.Rule_blockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.rule_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRule_block([NotNull] LanguageParser.Rule_blockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.function"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunction([NotNull] LanguageParser.FunctionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.function"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunction([NotNull] LanguageParser.FunctionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>arithmeticAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArithmeticAssignment([NotNull] LanguageParser.ArithmeticAssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>arithmeticAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArithmeticAssignment([NotNull] LanguageParser.ArithmeticAssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>comparisonAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterComparisonAssignment([NotNull] LanguageParser.ComparisonAssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>comparisonAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitComparisonAssignment([NotNull] LanguageParser.ComparisonAssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>returnValue</c>
	/// labeled alternative in <see cref="LanguageParser.return_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterReturnValue([NotNull] LanguageParser.ReturnValueContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>returnValue</c>
	/// labeled alternative in <see cref="LanguageParser.return_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitReturnValue([NotNull] LanguageParser.ReturnValueContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.conditional"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConditional([NotNull] LanguageParser.ConditionalContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.conditional"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConditional([NotNull] LanguageParser.ConditionalContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>thenBlock</c>
	/// labeled alternative in <see cref="LanguageParser.then_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterThenBlock([NotNull] LanguageParser.ThenBlockContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>thenBlock</c>
	/// labeled alternative in <see cref="LanguageParser.then_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitThenBlock([NotNull] LanguageParser.ThenBlockContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>elseBlock</c>
	/// labeled alternative in <see cref="LanguageParser.else_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterElseBlock([NotNull] LanguageParser.ElseBlockContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>elseBlock</c>
	/// labeled alternative in <see cref="LanguageParser.else_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitElseBlock([NotNull] LanguageParser.ElseBlockContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>orExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOrExpression([NotNull] LanguageParser.OrExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>orExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOrExpression([NotNull] LanguageParser.OrExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ifComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfComparisonExpression([NotNull] LanguageParser.IfComparisonExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ifComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfComparisonExpression([NotNull] LanguageParser.IfComparisonExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>andExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAndExpression([NotNull] LanguageParser.AndExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>andExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAndExpression([NotNull] LanguageParser.AndExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesisIfExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenthesisIfExpression([NotNull] LanguageParser.ParenthesisIfExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesisIfExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenthesisIfExpression([NotNull] LanguageParser.ParenthesisIfExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ifEntity</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfEntity([NotNull] LanguageParser.IfEntityContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ifEntity</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfEntity([NotNull] LanguageParser.IfEntityContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ifFunctionSignature</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfFunctionSignature([NotNull] LanguageParser.IfFunctionSignatureContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ifFunctionSignature</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfFunctionSignature([NotNull] LanguageParser.IfFunctionSignatureContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>comparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterComparisonExpression([NotNull] LanguageParser.ComparisonExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>comparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitComparisonExpression([NotNull] LanguageParser.ComparisonExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesisComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenthesisComparisonExpression([NotNull] LanguageParser.ParenthesisComparisonExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesisComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenthesisComparisonExpression([NotNull] LanguageParser.ParenthesisComparisonExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.comparison_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterComparison_operator([NotNull] LanguageParser.Comparison_operatorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.comparison_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitComparison_operator([NotNull] LanguageParser.Comparison_operatorContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>buscarCaracteristica</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBuscarCaracteristica([NotNull] LanguageParser.BuscarCaracteristicaContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>buscarCaracteristica</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBuscarCaracteristica([NotNull] LanguageParser.BuscarCaracteristicaContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>minusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMinusExpression([NotNull] LanguageParser.MinusExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>minusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMinusExpression([NotNull] LanguageParser.MinusExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesisExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenthesisExpression([NotNull] LanguageParser.ParenthesisExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesisExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenthesisExpression([NotNull] LanguageParser.ParenthesisExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>multExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMultExpression([NotNull] LanguageParser.MultExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>multExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMultExpression([NotNull] LanguageParser.MultExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>divExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDivExpression([NotNull] LanguageParser.DivExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>divExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDivExpression([NotNull] LanguageParser.DivExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>plusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPlusExpression([NotNull] LanguageParser.PlusExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>plusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPlusExpression([NotNull] LanguageParser.PlusExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>entityExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEntityExpression([NotNull] LanguageParser.EntityExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>entityExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEntityExpression([NotNull] LanguageParser.EntityExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.tabela_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTabela_caracteristica([NotNull] LanguageParser.Tabela_caracteristicaContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.tabela_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTabela_caracteristica([NotNull] LanguageParser.Tabela_caracteristicaContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.descricao_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDescricao_caracteristica([NotNull] LanguageParser.Descricao_caracteristicaContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.descricao_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDescricao_caracteristica([NotNull] LanguageParser.Descricao_caracteristicaContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.valor_fator_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterValor_fator_caracteristica([NotNull] LanguageParser.Valor_fator_caracteristicaContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.valor_fator_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitValor_fator_caracteristica([NotNull] LanguageParser.Valor_fator_caracteristicaContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.exercicio_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExercicio_caracteristica([NotNull] LanguageParser.Exercicio_caracteristicaContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.exercicio_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExercicio_caracteristica([NotNull] LanguageParser.Exercicio_caracteristicaContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>stringEntity</c>
	/// labeled alternative in <see cref="LanguageParser.text"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStringEntity([NotNull] LanguageParser.StringEntityContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>stringEntity</c>
	/// labeled alternative in <see cref="LanguageParser.text"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStringEntity([NotNull] LanguageParser.StringEntityContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>boolEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBoolEntity([NotNull] LanguageParser.BoolEntityContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>boolEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBoolEntity([NotNull] LanguageParser.BoolEntityContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>numberEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNumberEntity([NotNull] LanguageParser.NumberEntityContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>numberEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNumberEntity([NotNull] LanguageParser.NumberEntityContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>dateEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDateEntity([NotNull] LanguageParser.DateEntityContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>dateEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDateEntity([NotNull] LanguageParser.DateEntityContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>variableEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableEntity([NotNull] LanguageParser.VariableEntityContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>variableEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableEntity([NotNull] LanguageParser.VariableEntityContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>varTableColunaEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVarTableColunaEntity([NotNull] LanguageParser.VarTableColunaEntityContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>varTableColunaEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVarTableColunaEntity([NotNull] LanguageParser.VarTableColunaEntityContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>nullEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNullEntity([NotNull] LanguageParser.NullEntityContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>nullEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNullEntity([NotNull] LanguageParser.NullEntityContext context);
}
