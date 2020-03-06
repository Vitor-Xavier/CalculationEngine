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
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="LanguageParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface ILanguageVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.rule_set"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRule_set([NotNull] LanguageParser.Rule_setContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.rule_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRule_block([NotNull] LanguageParser.Rule_blockContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>arithmeticAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArithmeticAssignment([NotNull] LanguageParser.ArithmeticAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>comparisonAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparisonAssignment([NotNull] LanguageParser.ComparisonAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>returnValue</c>
	/// labeled alternative in <see cref="LanguageParser.return_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReturnValue([NotNull] LanguageParser.ReturnValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.conditional"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditional([NotNull] LanguageParser.ConditionalContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>thenBlock</c>
	/// labeled alternative in <see cref="LanguageParser.then_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitThenBlock([NotNull] LanguageParser.ThenBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>elseBlock</c>
	/// labeled alternative in <see cref="LanguageParser.else_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElseBlock([NotNull] LanguageParser.ElseBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>orExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOrExpression([NotNull] LanguageParser.OrExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ifComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfComparisonExpression([NotNull] LanguageParser.IfComparisonExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>andExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAndExpression([NotNull] LanguageParser.AndExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthesisIfExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesisIfExpression([NotNull] LanguageParser.ParenthesisIfExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ifEntity</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfEntity([NotNull] LanguageParser.IfEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>comparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparisonExpression([NotNull] LanguageParser.ComparisonExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthesisComparisonExpression</c>
	/// labeled alternative in <see cref="LanguageParser.comparison_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesisComparisonExpression([NotNull] LanguageParser.ParenthesisComparisonExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.comparison_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparison_operator([NotNull] LanguageParser.Comparison_operatorContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>caracteristicaTabela</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCaracteristicaTabela([NotNull] LanguageParser.CaracteristicaTabelaContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>buscarCaracteristica</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBuscarCaracteristica([NotNull] LanguageParser.BuscarCaracteristicaContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>coalesceFunction</c>
	/// labeled alternative in <see cref="LanguageParser.coalesce_function"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCoalesceFunction([NotNull] LanguageParser.CoalesceFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>coalesceExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCoalesceExpression([NotNull] LanguageParser.CoalesceExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>minusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMinusExpression([NotNull] LanguageParser.MinusExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthesisExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesisExpression([NotNull] LanguageParser.ParenthesisExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>multExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultExpression([NotNull] LanguageParser.MultExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>divExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDivExpression([NotNull] LanguageParser.DivExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>plusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPlusExpression([NotNull] LanguageParser.PlusExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ifFunctionSignature</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfFunctionSignature([NotNull] LanguageParser.IfFunctionSignatureContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>entityExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEntityExpression([NotNull] LanguageParser.EntityExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.tabela_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTabela_caracteristica([NotNull] LanguageParser.Tabela_caracteristicaContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.descricao_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDescricao_caracteristica([NotNull] LanguageParser.Descricao_caracteristicaContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.valor_fator_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitValor_fator_caracteristica([NotNull] LanguageParser.Valor_fator_caracteristicaContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.codigo_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCodigo_caracteristica([NotNull] LanguageParser.Codigo_caracteristicaContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.exercicio_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExercicio_caracteristica([NotNull] LanguageParser.Exercicio_caracteristicaContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.coluna_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitColuna_caracteristica([NotNull] LanguageParser.Coluna_caracteristicaContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.coluna_valor_caracteristica"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitColuna_valor_caracteristica([NotNull] LanguageParser.Coluna_valor_caracteristicaContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>boolEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolEntity([NotNull] LanguageParser.BoolEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>numberDecimalEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumberDecimalEntity([NotNull] LanguageParser.NumberDecimalEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>numberIntegerEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumberIntegerEntity([NotNull] LanguageParser.NumberIntegerEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>dateEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDateEntity([NotNull] LanguageParser.DateEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>variableEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableEntity([NotNull] LanguageParser.VariableEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>varTableColunaEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarTableColunaEntity([NotNull] LanguageParser.VarTableColunaEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>nullEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNullEntity([NotNull] LanguageParser.NullEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>stringEntity</c>
	/// labeled alternative in <see cref="LanguageParser.text"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStringEntity([NotNull] LanguageParser.StringEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.number_integer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber_integer([NotNull] LanguageParser.Number_integerContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.number_decimal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber_decimal([NotNull] LanguageParser.Number_decimalContext context);
}
