grammar Language;


/* Lexical rules */

IF   : 'se' ;
ELSE : 'senao';
SWITCH: 'switch';
CASE: 'case';
DEFAULT: 'default';

AND : '&&' ;
OR  : '||' ;

TRUE  : 'true' ;
FALSE : 'false' ;
NULL : 'null';
MARKER: 'marker';

BUSCAR_CARACTERISTICA: '_BuscarCaracteristica';
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

RBRACES: '}';
LBRACES: '{';

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
IDENTIFIER : [a-zA-Z_][a-zA-Z_0-9]* ;
VAR_TABLE_COLUNA : [@][a-zA-Z_][a-zA-Z_0-9]*[.][a-zA-Z_][a-zA-Z_0-9]* ;



SEMI : ';';
COLON : ':';

COMMENT : '//' .+? ('\n'|EOF) -> skip ;
WS : [ \r\t\u000C\n]+ -> skip ;

/* Grammar rules */


rule_set
	: rule_block* return_value? 
	;

rule_block
    : assignment
    | function
    | arithmetic_expression
    | conditional
    ;

function
	: function_signature SEMI
	;

assignment
    : (CONST)? (VAR)? IDENTIFIER ATRIB arithmetic_expression SEMI #arithmeticAssignment
    | (CONST)? (VAR)? IDENTIFIER ATRIB comparison_expression SEMI #comparisonAssignment
	;

return_value
	: RETURN arithmetic_expression? SEMI #returnValue
	;


conditional
    : IF if_expression LBRACES then_block RBRACES (ELSE LBRACES else_block RBRACES)?
    ;

then_block
	: rule_block* #thenBlock
    ;

else_block
	: rule_block* #elseBlock
    ;

if_expression
    : if_expression AND if_expression   #andExpression
    | if_expression OR if_expression    #orExpression
    | comparison_expression             #ifComparisonExpression
    | function_signature                #ifFunctionSignature
    | LPAREN if_expression RPAREN       #parenthesisIfExpression
    | entity                            #ifEntity
    ;

comparison_expression
    : arithmetic_expression comparison_operator arithmetic_expression   #comparisonExpression
    | LPAREN comparison_expression RPAREN                               #parenthesisComparisonExpression
    ;

comparison_operator
    : GT
    | GE
    | LT
    | LE
    | EQ
    | NEQ
    ;


function_signature
	: BUSCAR_CARACTERISTICA LPAREN tabela_caracteristica COMMA descricao_caracteristica (COMMA exercicio_caracteristica)? RPAREN  #buscarCaracteristica
    ;

arithmetic_expression
    : arithmetic_expression MULT arithmetic_expression							#multExpression
    | arithmetic_expression DIV arithmetic_expression							#divExpression
    | arithmetic_expression PLUS arithmetic_expression							#plusExpression
    | arithmetic_expression MINUS arithmetic_expression							#minusExpression
    | LPAREN arithmetic_expression RPAREN										#parenthesisExpression
    | entity																	#entityExpression
    ;

tabela_caracteristica
    : text
    ;

descricao_caracteristica
    : text
    ;

exercicio_caracteristica
    : DECIMAL
    ;

    text
    : QUOTE IDENTIFIER QUOTE #stringEntity
    ;


entity
    : (TRUE | FALSE)            #boolEntity
    | DECIMAL                   #numberEntity
	| DATE						#dateEntity
    | IDENTIFIER                #variableEntity
    | VAR_TABLE_COLUNA          #varTableColunaEntity
    | NULL                      #nullEntity
    ;