grammar Language;


/* Lexical rules */

COMMENT: '//' .+? ('\n'|EOF) -> skip;
WS: [ \r\t\u000C\n]+ -> skip;

IF   : 'se' ;
ELSE : 'senao';
SWITCH: 'switch';
CASE: 'case';
DEFAULT: 'default';

AND : '&&';
OR  : '||';
NOT : '!';

TRUE  : 'true' ;
FALSE : 'false' ;
NULL : 'nulo';
MARKER: 'marker';

ISNULL: '_NULO';
SUM_IF: '_SOMASE';
COUNT_IF: '_CONTSE';
ABS: '_ABS';
SQRT: '_RAIZ';
SUM: '_SOMA';
MAX: '_MAXIMO';
MIN: '_MINIMO';
AVERAGE: '_MEDIA';
LENGTH: '_CONT';
COALESCE: '_COALESCE';
CARACTERISTICA_TABELA: '_CARACTERISTICATABELA';
ATIVIDADE_TABELA: '_ATIVIDADETABELA';
CARACTERISTICA: '_CARACTERISTICA';
PARAMETRO: '_PARAMETRO';
PARAMETRO_CODIGO: '_PARAMETROCODIGO';
PARAMETRO_INTERVALO: '_PARAMETROINTERVALO';
ROUND: '_ARREDONDAR';
TODAY: '_HOJE';
NOW: '_AGORA';
DATE_DIF: '_DATADIF';
GET_MONTH: '_MES';
GET_DAY: '_DIA';
GET_YEAR: '_ANO';

LOOKUP_FUNC: 'lookupFunction';
BASE_FUNC: 'baseFunction';
TOTAL_PAYMENTS: 'totalPayments';
TOTAL_DISCOUNTS: 'totalDiscounts';
DIRECT_RECALCULATION: 'directRecalculation';
PROPORTIONAL_RECALCULATION: 'proportionalRecalculation';
CLEAR_VALUES_FUNCTION: 'clearValuesFunction';
CLEAR_DISCOUNTS_FUNCTION: 'clearDiscountsFunction';
CLEAR_PAYMENTS_FUNCTION: 'clearPaymentsFunction';
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

LBRACE: '{';
RBRACE: '}';

LPAREN : '(' ;
RPAREN : ')' ;

MULT  : '*' ;
DIV   : '/' ;
PLUS  : '+' ;
MINUS : '-' ;
POW: '^';

GT : '>' ;
GE : '>=' ;
LT : '<' ;
LE : '<=' ;
EQ : '==' ;
NEQ : '!=' ;

YEAR: 'ANO';
MONTH: 'MES';
DAY: 'DIA';

ATRIB: '=';
PLUS_ASSIGNMENT: '+=';
MINUS_ASSIGNMENT: '-=';
MULT_ASSIGNMENT: '*=';
DIV_ASSIGNMENT: '/=';

VAR: 'var';
LISTA: 'lista';

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

SEMI : ';';
COLON : ':';

/* Grammar rules */

rule_set
	: rule_block* return_value? 
	;

rule_block
    : assignment
    | variable_declaration
    | arithmetic_expression
    | conditional
    | loop
    ;


variable_declaration
    : VAR IDENTIFIER ATRIB arithmetic_expression SEMI #arithmeticDeclaration
    | VAR IDENTIFIER ATRIB comparison_expression SEMI #comparisonDeclaration
    | LISTA IDENTIFIER ATRIB arithmetic_expression SEMI #declareList
    | LISTA IDENTIFIER ATRIB LBRACKET RBRACKET SEMI #declareListAll
    ;

assignment
    : IDENTIFIER ATRIB comparison_expression SEMI #comparisonAssignment
    | IDENTIFIER assignment_operator arithmetic_expression SEMI #arithmeticAssignment
    | IDENTIFIER LBRACKET (IDENTIFIER | number_integer) RBRACKET DOT IDENTIFIER assignment_operator arithmetic_expression SEMI #listAssignment
    | VAR_PRIMARY DOT IDENTIFIER assignment_operator arithmetic_expression SEMI #varMemoryValueAssignment
    | VAR_PRIMARY LBRACKET (IDENTIFIER | number_integer) RBRACKET DOT IDENTIFIER assignment_operator arithmetic_expression SEMI #listMemoryGlobalValueAssignment
    | VAR_PRIMARY DOT IDENTIFIER LBRACKET (IDENTIFIER | number_integer) RBRACKET DOT IDENTIFIER assignment_operator arithmetic_expression SEMI #listMemoryValueAssignment
	;

return_value
	: RETURN arithmetic_expression? SEMI #returnValue
	;

conditional
    : IF if_expression LBRACE then_block RBRACE (ELSE LBRACE else_block RBRACE)?
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
    | NOT LPAREN if_expression RPAREN   #notParenthesisIfExpression
    | LPAREN if_expression RPAREN       #parenthesisIfExpression
    | NOT entity                        #notIfEntity
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

assignment_operator
    : ATRIB
    | PLUS_ASSIGNMENT
    | MINUS_ASSIGNMENT
    | MULT_ASSIGNMENT
    | DIV_ASSIGNMENT
    ;

loop
    : WHILE LPAREN if_expression RPAREN LBRACE rule_block* RBRACE #whileExpression
    ;

function_signature
    : ATIVIDADE_TABELA LPAREN tabela_caracteristica COMMA descricao_caracteristica COMMA coluna_caracteristica COMMA exercicio_caracteristica (COMMA coluna_filtro)? (COMMA LBRACKET (text+COMMA*)+ RBRACKET)?  RPAREN  #atividadeTabela
	| CARACTERISTICA_TABELA LPAREN tabela_caracteristica COMMA descricao_caracteristica COMMA coluna_caracteristica COMMA exercicio_caracteristica (COMMA valor_fator_caracteristica)? RPAREN  #caracteristicaTabela
    | CARACTERISTICA LPAREN descricao_caracteristica COMMA codigo_caracteristica COMMA valor_fator_caracteristica (COMMA exercicio_caracteristica)? RPAREN  #caracteristica
    | PARAMETRO LPAREN text COMMA number_integer (COMMA number_integer)? RPAREN #parametroFunction
   
    | SUM LPAREN VAR_PRIMARY DOT IDENTIFIER (DOT IDENTIFIER)? RPAREN #sumDatabase
    | SUM LPAREN IDENTIFIER DOT IDENTIFIER RPAREN #sumListLocal
    | SUM LPAREN IDENTIFIER RPAREN #sumVariable
   
    | MAX LPAREN VAR_PRIMARY DOT IDENTIFIER (DOT IDENTIFIER)? RPAREN #maxDatabase
    | MAX LPAREN IDENTIFIER DOT IDENTIFIER RPAREN #maxListLocal
    | MAX LPAREN IDENTIFIER RPAREN #maxVariable
    
    | MIN LPAREN VAR_PRIMARY DOT IDENTIFIER (DOT IDENTIFIER)? RPAREN #minDatabase
    | MIN LPAREN IDENTIFIER DOT IDENTIFIER RPAREN #minListLocal
    | MIN LPAREN IDENTIFIER RPAREN #minVariable
    
    | AVERAGE LPAREN VAR_PRIMARY DOT IDENTIFIER (DOT IDENTIFIER)? RPAREN #averageDatabase
    | AVERAGE LPAREN IDENTIFIER DOT IDENTIFIER RPAREN #averageListLocal
    | AVERAGE LPAREN IDENTIFIER RPAREN #averageVariable

    | LENGTH LPAREN VAR_PRIMARY (DOT IDENTIFIER)? RPAREN #lengthDatabase
    | LENGTH LPAREN IDENTIFIER RPAREN #lengthVariable
    
    | ROUND LPAREN arithmetic_expression (COMMA arithmetic_expression)? RPAREN #roundFunction
    | COALESCE LPAREN entity (COMMA entity)* RPAREN #coalesceFunction
    | SQRT LPAREN arithmetic_expression RPAREN #sqrtFunction
    | ABS LPAREN arithmetic_expression RPAREN #absFunction

    | SUM_IF LPAREN VAR_PRIMARY DOT IDENTIFIER (DOT IDENTIFIER)? COMMA arithmetic_expression comparison_operator arithmetic_expression RPAREN #sumIfFunction           
    | SUM_IF LPAREN IDENTIFIER DOT IDENTIFIER COMMA arithmetic_expression comparison_operator arithmetic_expression RPAREN #sumIfListLocal           

    | COUNT_IF LPAREN VAR_PRIMARY (DOT IDENTIFIER)? COMMA arithmetic_expression comparison_operator arithmetic_expression RPAREN #countIfFunction
    | COUNT_IF LPAREN IDENTIFIER COMMA arithmetic_expression comparison_operator arithmetic_expression RPAREN #countIfListLocal 
    
    | ISNULL LPAREN arithmetic_expression RPAREN #isNullFunction
    | TODAY LPAREN RPAREN #todayFunction
    | NOW LPAREN RPAREN #nowFunction
    | DATE_DIF LPAREN entity COMMA entity COMMA date_unit RPAREN #dateDifFunction
    | GET_YEAR LPAREN entity RPAREN #getYearFunction
    | GET_MONTH LPAREN entity RPAREN #getMonthFunction
    | GET_DAY LPAREN entity RPAREN #getDayFunction
    ;

arithmetic_expression
    : arithmetic_expression MULT arithmetic_expression							#multExpression
    | arithmetic_expression DIV arithmetic_expression							#divExpression
    | arithmetic_expression PLUS arithmetic_expression							#plusExpression
    | arithmetic_expression MINUS arithmetic_expression							#minusExpression
    | arithmetic_expression POW arithmetic_expression							#powExpression
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

    coluna_filtro                                                       
    : text
    ;

    coluna_valor_caracteristica                                                       
    : text
    ;

    entity
    : (TRUE | FALSE)            #boolEntity
    | number_decimal            #numberDecimalEntity
    | number_integer            #numberIntegerEntity
    | text                      #stringEntity    
	| DATE						#dateEntity
    | IDENTIFIER                #variableEntity
    | list                      #listEntity
    | VAR_PRIMARY               #varPrimaryEntity
    | varMemory                 #varMemoryEntity
    | listMemory                #listMemoryEntity
    | listMemoryGlobal          #listMemoryGlobalEntity
    | NULL                      #nullEntity
    ;

    varMemory
    : VAR_PRIMARY DOT IDENTIFIER #varMemoryValue
    ;
    
    listMemoryGlobal
    : VAR_PRIMARY LBRACKET (IDENTIFIER | number_integer) RBRACKET DOT IDENTIFIER #listMemoryGlobalValue
    ;

    listMemory
    : VAR_PRIMARY DOT IDENTIFIER LBRACKET (IDENTIFIER | number_integer) RBRACKET DOT IDENTIFIER #listMemoryValue
    ;

    list
    : IDENTIFIER LBRACKET (IDENTIFIER | number_integer) RBRACKET (DOT IDENTIFIER)? #listValue
    ;

    text
    : TEXT #string
    ;

    number_integer
    : NUMBER #numberInteger
    ;

    number_decimal
    : DECIMAL #numberDecimal
    ;

date_unit
    : YEAR
    | MONTH
    | DAY;

