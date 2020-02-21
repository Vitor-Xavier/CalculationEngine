grammar Language;


/* Lexical rules */

IF   : 'if' ;
ELSE : 'else';
SWITCH: 'switch';
CASE: 'case';
DEFAULT: 'default';

AND : '&&' ;
OR  : '||' ;

TRUE  : 'true' ;
FALSE : 'false' ;
NULL : 'null';
MARKER: 'marker';

LOOKUP_FUNC: 'lookupFunction';
BASE_FUNC: 'baseFunction';
TOTAL_PAYMENTS: 'totalPayments';
TOTAL_DISCOUNTS: 'totalDiscounts';
DIRECT_RECALCULATION: 'directRecalculation';
PROPORTIONAL_RECALCULATION: 'proportionalRecalculation';
ROUND_FUNCTION: 'roundFunction';
CLEAR_VALUES_FUNCTION: 'clearValuesFunction';
CLEAR_DISCOUNTS_FUNCTION: 'clearDiscountsFunction';
CLEAR_PAYMENTS_FUNCTION: 'clearPaymentsFunction';
GET_DAY: 'getDay';
GET_MONTH: 'getMonth';
GET_YEAR: 'getYear';
ADD_DAY: 'addDay';
ADD_MONTH: 'addMonth';
ADD_YEAR: 'addYear';
GET_DAY_DIFF: 'getDayDiff';
GET_MONTH_DIFF: 'getMonthDiff';
GET_YEAR_DIFF: 'getYearDiff';
GET_DATE: 'getDate';
LAST_DAY_PROCESS: 'lastDayProcess';
DESPREZAR: 'desprezar';

RBRACES: '{';
LBRACES: '}';

LPAREN : '(' ;
RPAREN : ')' ;

MULT  : '*' ;
DIV   : '/' ;
PLUS  : '+' ;
MINUS : '-' ;

GT : '>' ;
GE : '>=' ;
LT : '<' ;
LE : '<=' ;
EQ : '==' ;
NEQ : '!=' ;
ATRIB: '=';

VAR: 'var';
CONST: 'const';
RETURN: 'retorno';

COMMA: ',';
QUOTE : '"' ;

DECIMAL : '-'?[0-9]+('.'[0-9]+)? ;
DATE : ([0-9])+'/'([0-9])+'/'([0-9])+;
IDENTIFIER : [@]?[a-zA-Z_][a-zA-Z_0-9]* ;

SEMI : ';';
COLON : ':';

COMMENT : '//' .+? ('\n'|EOF) -> skip ;
WS : [ \r\t\u000C\n]+ -> skip ;

/* Grammar rules */


rule_set
	: rule_block*return_value
	;

rule_block
    : assignment
    | arithmetic_expression
    ;

    assignment
    : (CONST)? (VAR)? IDENTIFIER ATRIB arithmetic_expression SEMI
	;

return_value
	: RETURN (IDENTIFIER)? SEMI
	;

arithmetic_expression
    : arithmetic_expression PLUS arithmetic_expression	#plusExpression
    | entity																	          #entityExpression
    ;

entity
    : (TRUE | FALSE)            #boolEntity
    | DECIMAL                   #numberEntity
	| DATE						          #dateEntity
    | QUOTE IDENTIFIER QUOTE    #stringEntity
    | IDENTIFIER                #variableEntity
    | NULL                      #nullEntity
    ;