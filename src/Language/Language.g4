grammar Language;


/* Lexical rules */

COMMENT: '//' .+? ('\n'|EOF) -> skip;
WS: [ \r\t\u000C\n]+ -> skip;

IF   : 'se' ;
ELSE : 'senao';
SWITCH: 'parametro';
CASE: 'caso';
DEFAULT: 'padrao';
WHILE: 'enquanto';
BREAK: 'parar';

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
CARACTERISTICA: '_CARACTERISTICA';
PARAMETRO: '_PARAMETRO';
PARAMETRO_CODIGO: '_PARAMETROCODIGO';
PARAMETRO_INTERVALO: '_PARAMETROINTERVALO';
ROUND: '_ARREDONDAR';
DATE_FUNCTION: '_DATA';
TODAY: '_HOJE';
NOW: '_AGORA';
DATE_DIF: '_DATADIF';
GET_YEAR: '_ANO';
GET_MONTH: '_MES';
GET_DAY: '_DIA';
GET_HOUR: '_HORA';
GET_MINUTE: '_MINUTO';
ADD_DAY: '_DIA_ADICIONAR';
ADD_MONTH: '_MES_ADICIONAR';
ADD_YEAR: '_ANO_ADICIONAR';
TRIM: '_ARRUMAR';

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
POW_ASSIGNMENT: '^=';

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
VAR_OBJECT: [@][a-zA-Z_][a-zA-Z_0-9]*[.][a-zA-Z_][a-zA-Z_0-9]*;
VAR_ARRAY: [@][a-zA-Z_][a-zA-Z_0-9]*(LBRACKET (NUMBER | IDENTIFIER) RBRACKET)'.'[a-zA-Z_][a-zA-Z_0-9]+;
IDENTIFIER_ARRAY: [a-zA-Z_][a-zA-Z_0-9]*(LBRACKET (NUMBER | IDENTIFIER) RBRACKET)'.'[a-zA-Z_][a-zA-Z_0-9]+;

SEMI : ';';
COLON : ':';

ERRORCHAR: .;

/* Grammar rules */

rule_set
	: rule_block* return_value?
	;

rule_block
    : variable_declaration
    | switch_expression
    | arithmetic_expression
    | conditional
    | loop
    | break_statement
    | assignment
    ;

variable_declaration
    : VAR IDENTIFIER ATRIB arithmetic_expression SEMI #arithmeticDeclaration
    | VAR IDENTIFIER ATRIB comparison_expression SEMI #comparisonDeclaration
    | LISTA IDENTIFIER_ARRAY ATRIB arithmetic_expression SEMI #arrayAssignment
    ;

assignment
    : IDENTIFIER assignment_operator arithmetic_expression SEMI #arithmeticAssignment
    | IDENTIFIER ATRIB comparison_expression SEMI #comparisonAssignment
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

switch_expression
    : SWITCH arithmetic_expression LBRACE case_statement+ default_statement? RBRACE #switchExpression
    ;

case_statement
    : CASE arithmetic_expression COLON (LBRACE rule_block+ RBRACE | rule_block) #caseStatement
    ;

default_statement
    : DEFAULT COLON (LBRACE rule_block+ RBRACE | rule_block) #defaultStatement
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
    | POW_ASSIGNMENT
    ;

loop
    : WHILE LPAREN if_expression RPAREN LBRACE rule_block* RBRACE #whileExpression
    ;

break_statement
    : BREAK SEMI
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
    | ROUND LPAREN arithmetic_expression (COMMA arithmetic_expression)? RPAREN #roundFunction
    | COALESCE LPAREN entity (COMMA entity)+ RPAREN #coalesceFunction
    | SQRT LPAREN arithmetic_expression RPAREN #sqrtFunction
    | ABS LPAREN arithmetic_expression RPAREN #absFunction
    | SUM_IF LPAREN VAR_OBJECT COMMA arithmetic_expression comparison_operator arithmetic_expression RPAREN #sumIfFunction
    | COUNT_IF LPAREN VAR_PRIMARY COMMA arithmetic_expression comparison_operator arithmetic_expression RPAREN #countIfFunction
    | ISNULL LPAREN arithmetic_expression RPAREN #isNullFunction
    | DATE_FUNCTION LPAREN arithmetic_expression COMMA arithmetic_expression COMMA arithmetic_expression RPAREN #dateFunction
    | TODAY LPAREN RPAREN #todayFunction
    | NOW LPAREN RPAREN #nowFunction
    | DATE_DIF LPAREN arithmetic_expression COMMA arithmetic_expression COMMA date_unit RPAREN #dateDifFunction
    | GET_YEAR LPAREN arithmetic_expression RPAREN #getYearFunction
    | GET_MONTH LPAREN arithmetic_expression RPAREN #getMonthFunction
    | GET_DAY LPAREN arithmetic_expression RPAREN #getDayFunction
    | GET_HOUR LPAREN arithmetic_expression RPAREN #getHourFunction
    | GET_MINUTE LPAREN arithmetic_expression RPAREN #getMinuteFunction
    | ADD_YEAR LPAREN arithmetic_expression COMMA arithmetic_expression RPAREN #addYearFunction
    | ADD_MONTH LPAREN arithmetic_expression COMMA arithmetic_expression RPAREN #addMonthFunction
    | ADD_DAY LPAREN arithmetic_expression COMMA arithmetic_expression RPAREN #addDayFunction
    | TRIM LPAREN arithmetic_expression RPAREN #trimFunction
    ;

arithmetic_expression
    : arithmetic_expression MULT arithmetic_expression					  #multExpression
    | arithmetic_expression DIV arithmetic_expression							#divExpression
    | arithmetic_expression PLUS arithmetic_expression						#plusExpression
    | arithmetic_expression MINUS arithmetic_expression						#minusExpression
    | arithmetic_expression POW arithmetic_expression							#powExpression
    | LPAREN arithmetic_expression RPAREN										      #parenthesisExpression
    | function_signature                                          #ifFunctionSignature
    | entity																	                    #entityExpression
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
    | text                      #textEntity
	  | DATE						          #dateEntity
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

date_unit
    : YEAR
    | MONTH
    | DAY
    ;
