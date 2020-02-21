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
	/// Enter a parse tree produced by <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignment([NotNull] LanguageParser.AssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignment([NotNull] LanguageParser.AssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LanguageParser.return_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterReturn_value([NotNull] LanguageParser.Return_valueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LanguageParser.return_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitReturn_value([NotNull] LanguageParser.Return_valueContext context);
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
	/// Enter a parse tree produced by the <c>stringEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStringEntity([NotNull] LanguageParser.StringEntityContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>stringEntity</c>
	/// labeled alternative in <see cref="LanguageParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStringEntity([NotNull] LanguageParser.StringEntityContext context);
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
