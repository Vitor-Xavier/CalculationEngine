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

SUM: '_SOMA';
MAX: '_MAXIMO';
MIN: '_MINIMO';
AVERAGE: '_MEDIA';
LENGTH: '_TAMANHO';
COALESCE: '_COALESCE';
CARACTERISTICA_TABELA: '_CARACTERISTICATABELA';
CARACTERISTICA: '_CARACTERISTICA';
PARAMETRO: '_PARAMETRO';
PARAMETRO_CODIGO: '_PARAMETROCODIGO';
PARAMETRO_INTERVALO: '_PARAMETROINTERVALO';
ROUND: '_ARREDONDAR';
LOOKUP_FUNC: 'lookupFunction';
BASE_FUNC: 'baseFunction';
TOTAL_PAYMENTS: 'totalPayments';
TOTAL_DISCOUNTS: 'totalDiscounts';
DIRECT_RECALCULATION: 'directRecalculation';
PROPORTIONAL_RECALCULATION: 'proportionalRecalculation';
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

WHILE: 'enquanto';

LBRACKET: '[';
RBRACKET: ']';

LBRACES: '{';
RBRACES: '}';

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
LISTA: 'lista';
CONST: 'const';
RETURN: 'retorno';

COMMA: ',';
DOT: '.';
QUOTE : '"' ;

NUMBER : '-'?[0-9]+;
DECIMAL : '-'?[0-9]+'.'[0-9]+ ;
DATE : ([0-9])+'/'([0-9])+'/'([0-9])+;
IDENTIFIER : [a-zA-Z_][a-zA-Z_0-9]* ;
TEXT: QUOTE (~["\\] | '\\' .)* QUOTE;
VAR_PRIMARY: [@][a-zA-Z_][a-zA-Z_0-9]*;
VAR_OBJECT: [@][a-zA-Z_][a-zA-Z_0-9]*[.][a-zA-Z_][a-zA-Z_0-9]*;
VAR_ARRAY: [@][a-zA-Z_][a-zA-Z_0-9]*(LBRACKET (NUMBER | IDENTIFIER) RBRACKET)'.'[a-zA-Z_][a-zA-Z_0-9]+;
IDENTIFIER_ARRAY: [a-zA-Z_][a-zA-Z_0-9]*(LBRACKET (NUMBER | IDENTIFIER) RBRACKET)'.'[a-zA-Z_][a-zA-Z_0-9]+;



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
    | arithmetic_expression
    | conditional
    | loop
    ;

assignment
    : (VAR)? IDENTIFIER ATRIB arithmetic_expression SEMI #arithmeticAssignment
    | (VAR)? IDENTIFIER ATRIB comparison_expression SEMI #comparisonAssignment
    | (LISTA)? IDENTIFIER_ARRAY ATRIB arithmetic_expression SEMI #arrayAssignment
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

loop
    : WHILE LPAREN if_expression RPAREN LBRACES rule_block* RBRACES #whileExpression
    ;

function_signature
	: CARACTERISTICA_TABELA LPAREN tabela_caracteristica COMMA descricao_caracteristica COMMA coluna_caracteristica COMMA exercicio_caracteristica (COMMA valor_fator_caracteristica)? RPAREN  #caracteristicaTabela
    | CARACTERISTICA LPAREN descricao_caracteristica COMMA codigo_caracteristica COMMA valor_fator_caracteristica (COMMA exercicio_caracteristica)? RPAREN  #caracteristica
    | PARAMETRO LPAREN text (COMMA number_integer)? RPAREN #parametroFunction
    | PARAMETRO_CODIGO LPAREN text COMMA text (COMMA number_integer)? RPAREN #parametroCodigoFunction
    | PARAMETRO_INTERVALO LPAREN text COMMA text (COMMA number_integer)? RPAREN #parametroIntervaloFunction
    | SUM LPAREN VAR_OBJECT RPAREN #sumFunction
    | MAX LPAREN VAR_OBJECT RPAREN #maxFunction
    | MIN LPAREN VAR_OBJECT RPAREN #minFunction
    | AVERAGE LPAREN VAR_OBJECT RPAREN #averageFunction
    | LENGTH LPAREN VAR_PRIMARY RPAREN #lengthFunction
    | ROUND LPAREN number_decimal (COMMA number_integer)? RPAREN #roundFunction
    | COALESCE LPAREN entity (COMMA entity)* RPAREN #coalesceFunction
    ;

arithmetic_expression
    : arithmetic_expression MULT arithmetic_expression							#multExpression
    | arithmetic_expression DIV arithmetic_expression							#divExpression
    | arithmetic_expression PLUS arithmetic_expression							#plusExpression
    | arithmetic_expression MINUS arithmetic_expression							#minusExpression
    | LPAREN arithmetic_expression RPAREN										#parenthesisExpression
    | function_signature                                                        #ifFunctionSignature
    | entity																	#entityExpression
    ;

    tabela_caracteristica
    : text
    ;

    descricao_caracteristica
    : text
    ;

    valor_fator_caracteristica
    : text
    ;

    codigo_caracteristica
    : text
    ;
    
    exercicio_caracteristica                                                       
    : number_integer
    ;

    coluna_caracteristica                                                       
    : text
    ;

    coluna_valor_caracteristica                                                       
    : text
    ;

    entity
    : (TRUE | FALSE)            #boolEntity
    | number_decimal            #numberDecimalEntity
    | number_integer            #numberIntegerEntity
	| DATE						#dateEntity
    | IDENTIFIER                #variableEntity
    | IDENTIFIER_ARRAY          #variableArrayEntity
    | VAR_PRIMARY               #varPrimaryEntity
    | VAR_ARRAY                 #varArrayEntity
    | VAR_OBJECT                #varObjectEntity
    | NULL                      #nullEntity
    ;

 

    text
    : TEXT #stringEntity
    ;

    number_integer
    : NUMBER #numberInteger
    ;

    number_decimal
    : DECIMAL #numberDecimal
    ;
