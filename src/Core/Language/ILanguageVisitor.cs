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

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="LanguageParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
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
	/// Visit a parse tree produced by the <c>arithmeticDeclaration</c>
	/// labeled alternative in <see cref="LanguageParser.variable_declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArithmeticDeclaration([NotNull] LanguageParser.ArithmeticDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>comparisonDeclaration</c>
	/// labeled alternative in <see cref="LanguageParser.variable_declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparisonDeclaration([NotNull] LanguageParser.ComparisonDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>declareList</c>
	/// labeled alternative in <see cref="LanguageParser.variable_declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeclareList([NotNull] LanguageParser.DeclareListContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>declareListAll</c>
	/// labeled alternative in <see cref="LanguageParser.variable_declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeclareListAll([NotNull] LanguageParser.DeclareListAllContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>comparisonAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparisonAssignment([NotNull] LanguageParser.ComparisonAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>arithmeticAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArithmeticAssignment([NotNull] LanguageParser.ArithmeticAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListAssignment([NotNull] LanguageParser.ListAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>varMemoryValueAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarMemoryValueAssignment([NotNull] LanguageParser.VarMemoryValueAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listMemoryGlobalValueAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListMemoryGlobalValueAssignment([NotNull] LanguageParser.ListMemoryGlobalValueAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listMemoryValueAssignment</c>
	/// labeled alternative in <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListMemoryValueAssignment([NotNull] LanguageParser.ListMemoryValueAssignmentContext context);
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
	/// Visit a parse tree produced by the <c>notIfEntity</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNotIfEntity([NotNull] LanguageParser.NotIfEntityContext context);
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
	/// Visit a parse tree produced by the <c>notParenthesisIfExpression</c>
	/// labeled alternative in <see cref="LanguageParser.if_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNotParenthesisIfExpression([NotNull] LanguageParser.NotParenthesisIfExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>switchExpression</c>
	/// labeled alternative in <see cref="LanguageParser.switch_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSwitchExpression([NotNull] LanguageParser.SwitchExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>caseStatement</c>
	/// labeled alternative in <see cref="LanguageParser.case_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCaseStatement([NotNull] LanguageParser.CaseStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>defaultStatement</c>
	/// labeled alternative in <see cref="LanguageParser.default_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDefaultStatement([NotNull] LanguageParser.DefaultStatementContext context);
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
	/// Visit a parse tree produced by <see cref="LanguageParser.assignment_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment_operator([NotNull] LanguageParser.Assignment_operatorContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>whileExpression</c>
	/// labeled alternative in <see cref="LanguageParser.loop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhileExpression([NotNull] LanguageParser.WhileExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.break_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBreak_statement([NotNull] LanguageParser.Break_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>sumDatabase</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSumDatabase([NotNull] LanguageParser.SumDatabaseContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>sumListLocal</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSumListLocal([NotNull] LanguageParser.SumListLocalContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>sumVariable</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSumVariable([NotNull] LanguageParser.SumVariableContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>maxDatabase</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMaxDatabase([NotNull] LanguageParser.MaxDatabaseContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>maxListLocal</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMaxListLocal([NotNull] LanguageParser.MaxListLocalContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>maxVariable</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMaxVariable([NotNull] LanguageParser.MaxVariableContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>minDatabase</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMinDatabase([NotNull] LanguageParser.MinDatabaseContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>minListLocal</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMinListLocal([NotNull] LanguageParser.MinListLocalContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>minVariable</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMinVariable([NotNull] LanguageParser.MinVariableContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>averageDatabase</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAverageDatabase([NotNull] LanguageParser.AverageDatabaseContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>averageListLocal</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAverageListLocal([NotNull] LanguageParser.AverageListLocalContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>averageVariable</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAverageVariable([NotNull] LanguageParser.AverageVariableContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>lengthDatabase</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLengthDatabase([NotNull] LanguageParser.LengthDatabaseContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>lengthVariable</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLengthVariable([NotNull] LanguageParser.LengthVariableContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>roundFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRoundFunction([NotNull] LanguageParser.RoundFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>coalesceFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCoalesceFunction([NotNull] LanguageParser.CoalesceFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>sqrtFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSqrtFunction([NotNull] LanguageParser.SqrtFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>absFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAbsFunction([NotNull] LanguageParser.AbsFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>sumIfFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSumIfFunction([NotNull] LanguageParser.SumIfFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>sumIfListLocal</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSumIfListLocal([NotNull] LanguageParser.SumIfListLocalContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>countIfFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCountIfFunction([NotNull] LanguageParser.CountIfFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>countIfListLocal</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCountIfListLocal([NotNull] LanguageParser.CountIfListLocalContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>isNullFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIsNullFunction([NotNull] LanguageParser.IsNullFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>dateFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDateFunction([NotNull] LanguageParser.DateFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>todayFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTodayFunction([NotNull] LanguageParser.TodayFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>nowFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNowFunction([NotNull] LanguageParser.NowFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>dateDifFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDateDifFunction([NotNull] LanguageParser.DateDifFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>getYearFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGetYearFunction([NotNull] LanguageParser.GetYearFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>getMonthFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGetMonthFunction([NotNull] LanguageParser.GetMonthFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>getDayFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGetDayFunction([NotNull] LanguageParser.GetDayFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>getHourFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGetHourFunction([NotNull] LanguageParser.GetHourFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>getMinuteFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGetMinuteFunction([NotNull] LanguageParser.GetMinuteFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>addYearFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAddYearFunction([NotNull] LanguageParser.AddYearFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>addMonthFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAddMonthFunction([NotNull] LanguageParser.AddMonthFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>addDayFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAddDayFunction([NotNull] LanguageParser.AddDayFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>trimFunction</c>
	/// labeled alternative in <see cref="LanguageParser.function_signature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTrimFunction([NotNull] LanguageParser.TrimFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>minusExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMinusExpression([NotNull] LanguageParser.MinusExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>powExpression</c>
	/// labeled alternative in <see cref="LanguageParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPowExpression([NotNull] LanguageParser.PowExpressionContext context);
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
	/// Visit a parse tree produced by the <c>stringEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStringEntity([NotNull] LanguageParser.StringEntityContext context);
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
	/// Visit a parse tree produced by the <c>listEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListEntity([NotNull] LanguageParser.ListEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>varPrimaryEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarPrimaryEntity([NotNull] LanguageParser.VarPrimaryEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>varMemoryEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarMemoryEntity([NotNull] LanguageParser.VarMemoryEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listMemoryEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListMemoryEntity([NotNull] LanguageParser.ListMemoryEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listMemoryGlobalEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListMemoryGlobalEntity([NotNull] LanguageParser.ListMemoryGlobalEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>nullEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNullEntity([NotNull] LanguageParser.NullEntityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>varMemoryValue</c>
	/// labeled alternative in <see cref="LanguageParser.varMemory"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarMemoryValue([NotNull] LanguageParser.VarMemoryValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listMemoryGlobalValue</c>
	/// labeled alternative in <see cref="LanguageParser.listMemoryGlobal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListMemoryGlobalValue([NotNull] LanguageParser.ListMemoryGlobalValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listMemoryValue</c>
	/// labeled alternative in <see cref="LanguageParser.listMemory"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListMemoryValue([NotNull] LanguageParser.ListMemoryValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listValue</c>
	/// labeled alternative in <see cref="LanguageParser.list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListValue([NotNull] LanguageParser.ListValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>string</c>
	/// labeled alternative in <see cref="LanguageParser.text"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString([NotNull] LanguageParser.StringContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>numberInteger</c>
	/// labeled alternative in <see cref="LanguageParser.number_integer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumberInteger([NotNull] LanguageParser.NumberIntegerContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>numberDecimal</c>
	/// labeled alternative in <see cref="LanguageParser.number_decimal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumberDecimal([NotNull] LanguageParser.NumberDecimalContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LanguageParser.date_unit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDate_unit([NotNull] LanguageParser.Date_unitContext context);
}
