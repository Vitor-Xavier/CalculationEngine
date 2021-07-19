# 🧮 Linguagem Cálculo Tributário

## 🔰 Introdução

A linguagem do cálculo do Sistema Tributário foi arquitetada para auxiliar a computação de valores de tributos e taxas através da utilização da ferramenta ANTLR4, provendo funções e recursos específicos para facilitar o acesso a dados, com sintaxe em português e de forma simplificada.

Se trata de uma linguagem simples, sem tipagem de variáveis, e com foco em disponibilizar recursos para facilitar o trabalho de implantação, contando com operações aritméticas e condicionais, instruções lógicas e de repetição, funções comuns para o tratamento de valores e específicas da regra do Sistema Tributário.

O sistema provém também acesso simplificado aos dados do registro em execução, que disponibiliza os dados através do uso do caractere '@' seguido do nome da tabela, e então o caractere '.', e em seguida o nome da coluna, estrutura essa que pode sofrer alterações conforme o tipo de acesso realizado, representado no item de [constantes de suporte](#constantes-de-suporte).

Os demais itens neste documento descrevem as funcionalidades essenciais da linguagem de cálculo, com exemplos de sua utilização, dentre outros recursos.

## 📑 Estrutura

A estrutura definida para a linguagem de cálculo dita que todas as fórmulas devem se encerrar com uma [instrução `retorno`](#instrução-retorno), opcionalmente informando um valor de retorno, sendo assim, após esse nenhum comando será executado. Antes da finalização no entanto, é possível a utilização de todos os demais recursos definidos na linguagem presentes neste documento.

Exemplo de estrutura simples de uma fórmula na linguagem de cálculo, utilizando comentários, variáveis locais, constantes de suporte, e uma estrutura lógica, além da instrução retorno, obrigatória:

```csharp
// Definição de tamanho máximo
var tamanhoMaximo = 100.5;
var resultado = 0;

// Verificação de tamanho máximo para aplicar ao resultado
se (@Fisico.AreaTerreno == tamanhoMaximo) {
    resultado = 3.5;
} senao {
    resultado = 3.0;
}
retorno resultado;
```

## 🔤 Palavras-chave

### Instrução Parâmetro

Uma instrução `parametro` inclui um ou mais casos para comparação. Cada seção `parametro` contém uma ou mais instruções `caso`, e uma ou nenhuma instrução `padrao` seguidos por um bloco de uma ou mais instruções.
Em uma instrução `parametro` pode conter qualquer quantidade de casos, no entanto dois casos não podem conter a mesma expressão. No exemplo abaixo a única correspondência ocorre no terceiro caso, e a variável resultado recebe o valor 0.375.

```csharp
var numero = 3;
var resultado = 0;
parametro (numero) {
    caso 1: {
        resultado = 0.1;
        parar;
    }
    caso 2: {
        resultado = 0.25;
        parar;
    }
    caso 3: {
        resultado = 0.375;
        parar;
    }
}
retorno resultado; // Retorna 0.375
```


### Instrução Caso

A instrução `caso` é utilizada exclusivamente dentro do bloco de uma instrução `parametro`, para informações sobre [instrução parâmetro](#instrução-parâmetro) na seção de Palavras-chave.

Cada instrução `caso` especifica um padrão a ser comparado com a expressão do `parametro`. Se houver correspondência, o bloco de código equivalente ao primeiro caso correspondente. Se nenhuma correspondência de caso for encontrada, o bloco de código da instrução `padrao` será executado, caso existir. No exemplo abaixo, como nenhum dos casos definidos no `parametro` existe correspondência, o bloco da instrução `padrao` será executado. No exemplo a seguir o valor da variável `numero` é comparado aos valores 1, 2 e 3 definidos nas instruções `caso`, e ao atingir o valor correspondente executa o bloco de código equivalente e então o comando `parar` para terminar as comparações da instrução `parametro`.

```csharp
var numero = 2;
var resultado = 0;
parametro (numero) {
    caso 1: {
        resultado = 0.1;
        parar;
    }
    caso 2: {
        resultado = 0.25;
        parar;
    }
    caso 3: {
        resultado = 0.375;
        parar;
    }
    padrao: {
        resultado = 0.05;
    }
}
retorno resultado; // Retorna 0.25
```

### Instrução Padrão

A instrução `padrao` é utilizada exclusivamente dentro do bloco de uma instrução `parametro`, para informações sobre [instrução parâmetro](#Instrução-Parâmetro) na seção de Palavras-chave.

Uma instrução `padrao` especifica a seção `parametro` a ser executada se a expressão de correspondência não corresponder a nenhum outro caso especificado. Se um `padrao` não estiver presente e a expressão de correspondência não corresponder a nenhum outro caso, nenhum bloco da expressão `parametro` será executado. No exemplo a seguir, como não há correspondência entre a expressão de `parametro` e os valores definidos nos casos de comparação, o trecho de código da instrução `padrao` será executado.

```csharp
var numero = 0;
var resultado = 0;
parametro (numero) {
    caso 1: {
        resultado = 0.1;
        parar;
    }
    caso 2: {
        resultado = 0.25;
        parar;
    }
    caso 3: {
        resultado = 0.375;
        parar;
    }
    padrao: {
        resultado = 0.05;
    }
}
retorno resultado; // Retorna 0.05
```

### Instrução Enquanto

A instrução `enquanto` executa uma instrução ou um bloco de instruções enquanto a expressão especificada é avaliada como `verdadeiro`. Como essa expressão é avaliada antes de cada execução do laço de repetição, uma instrução `enquanto` é executado zero ou mais vezes.

É possível sair de uma instrução `enquanto`, utilizando a palavra-chave `parar`, ou quando a condição especificada ser avaliada como `falso`.

No exemplo a seguir é exibido o funcionamento de uma instrução `enquanto`.

```csharp
var contador = 0;
enquanto (contador < 3) {
    contador += 1;
}
retorno contador; // Retorna 3
```

### Instrução Parar

A instrução `parar` termina o laço de repetição `enquanto`, e também a execução de blocos de código referentes a instruções `se`, `senao` e `parametro`, dentro do bloco de código de um `caso`. No exemplo abaixo a instrução `parar` interrompe a execução de `enquanto` quando a condição é avaliada para `verdadeiro`.

```csharp
var i = 0;
enquanto (i < 4) {
    se (i == 2) {
        parar;
    }
    i += 1;
}
retorno i; // Retorna 2
```

### Instrução Retorno

A instrução `retorno` finaliza a execução de uma fórmula, assim sinalizando o término do bloco de código a ser executado e opcionalmente pode informar um valor a ser armazenado dentre os resultados das fórmulas conforme o roteiro a qual se enquadra. No exemplo a seguir é ilustrado o retorno do cálculo de uma variável ao final do bloco de código.

```csharp
var numero = 0.75;
retorno numero * 100; // Retorna 75
```

### Instrução Se

Uma instrução `se` identifica qual instrução executar com base no valor de uma expressão condicional. No exemplo a seguir, a comparação da variável `numero` com o valor 0, utilizando a operação de maior que ou igual, resulta em `verdadeiro`, então a variável resultado recebe o valor 1.5.

```csharp
var numero = 0;
var resultado = 0;
se (numero >= 0) {
    resultado = 1.5;
}
retorno resultado; // Retorna 1.5
```

### Instrução Se-Senão

Uma instrução `senao` deve acompanhar uma instrução `se`, e é executada no caso da condição definida na instrução `se` for avaliada como `falso`, como uma condição não pode ser simultaneamente `verdadeiro` e `falso` somente um dos blocos de código será executado. No exemplo a seguir a comparação da variável `numero` com o valor -2, utilizando a operação de maior que ou igual, resulta em `falso`, então a variável resultado recebe o valor 2, e o bloco de código referente a condição `se` não será executado.

```csharp
var numero = -2;
var resultado = 0;
se (numero >= 0) {
    resultado = 1.5;
} senao {
    resultado = 2;
}
retorno resultado; // Retorna 2
```

### Instrução Senão-Se

Uma instrução `se` pode ocorrer após uma instrução `senao`, e é executada no caso da condição definida na primeira instrução `se` for avaliada como `falso`, e a condição definida na segunda instrução `se` for avaliada como `verdadeiro` somente o segundo do bloco de código será executado. No exemplo a seguir a comparação da variável `numero` com o valor 2, utilizando a operação de maior que ou igual, resulta em `falso`, então a segunda condição é avalida e produz `verdadeiro`, então a variável resultado recebe o valor 2, e somente o bloco de código referente a segunda condição `se` será executado.

```csharp
var numero = 2;
var resultado = 0;
se (numero < 2) {
    resultado = 1.5;
} senao se (numero == 2) {
    resultado = 2;
}
retorno resultado; // Retorna 2
```

### Instrução Nulo

A instrução `nulo`, representa a ausência de valor, e portanto não pode ser utilizado como identificador.

```csharp
var teste = nulo;
var valor = 0;
se (teste == nulo) {
    valor = 1;
}
retorno valor; // Retorna 1
```

### Instrução Var

A instrução `var` permite a declaração de variáveis, que são representações de valores mutáveis, necessariamente inicializados com um valor.

#### Exemplo de variável

Exemplos da utilização do palavra chave `var` em diferentes cenários.

##### Declaração de variável

O exemplo a seguir demonstra a declaração de variáveis 

```csharp
var numero = 10; // Numérico inteiro                 
var valorDecimal = 10.5; // Numérico decimal                 
var texto = "texto"; // Texto                            
var fisicoId = @Fisico.IdFisico; // Dado do registro sendo executado
var taxa = @Variavel.Taxa; // Valor externo                    
var booleano = verdadeiro; // Booleano                         
var soma = 10 + 10; // Resultado de operação aritmética 
var comparacao = 10 == 10; // Resultado de operação condicional
var valor = nulo; // Sem valor                        
var tamanho = _CONT(@FisicoOutros); // Resultado de função              
var objeto = {}; // Declaração de objeto             
var lista = []; // Declaração de lista         
var var1 = numero; // Valor de outra variável
retorno;
```

##### Escopo

As variáveis não possuem um tipo definido, portanto é possível declarar uma variável como um número decimal, e posteriormente atribuir a ela um texto, conforme demonstra exemplo abaixo.

```csharp
var teste = 10.5;
teste = "tst";
retorno teste; // Retorna "tst"
```

Mesmo usada dentro de um bloco, a instrução `var` não limita o escopo das variáveis para aquele bloco, portanto seu valor irá persistir na memória de execução até o final da fórmula.

```csharp
var a = 10;
se (a == 10) {
    var b = 20;
}
retorno b; // Retorna 20;
```

##### Precedência de uso

A utilização de variáveis não pode ocorrer antes de sua declaração, por exemplo o código abaixo apresenta erro de variável não declarada.

```csharp
a = 2; // Gera erro "Variável 'a' não foi declarada"
var a = 10;
retorno a;
```

Assim, a declaração utilizando a palavra chave `var` deve sempre preceder seus demais usos.

```csharp
var a = 10;
a = 2;
retorno a; // Retorna 2;
```

##### Redeclaração

Também não é permitida a redeclaração de variáveis dentro de uma mesma fórmula, onde é apresentado o erro de variável já declarada.

```csharp
var a = 10;
var a = 2; // Gera erro Variável 'a' já foi declarada
retorno a;
```

___

## ➗ Operadores

### Operadores Aritméticos

Operadores Aritméticos são utilizados para realizar operações matemáticas básicas, tais como adição, subtração, multiplicação e divisão, e produzir resultados numéricos.

#### Operador de soma `+`

O operador de soma `+` calcula a soma dos operandos.

#### Operador de subtração `-`

O operador de subtração `-` subtrai o operando à direita do operando à esquerda.

#### Operador de multiplicação `*`

O operador de multiplicação `*` calcula o produto dos operandos.

#### Operador de divisão `/`

O operador de divisão `/` divide o operando à esquerda pelo operando à direita.

### Operadores Booleanos

#### Operador de negação `!`

O operador `!` calcula a negação lógica de seu operando. Ou seja, ele produz `verdadeiro`, se o operando for avaliado como `falso`, e `falso`, se o operando for avaliado como `verdadeiro`.

#### Operador lógico E `&&`

O operador lógico condicional E `&&`, computa o E lógico de seus operandos, então o resultado de `x && y` será `verdadeiro` se ambos x e y forem avaliados como `verdadeiro`. Caso contrário, o resultado será `falso`. Se x for avaliado como `falso`, y não será avaliado.

#### Operador lógico OU `||`

O operador lógico condicional OU `||`, computa o OU lógico de seus operandos, então o resultado de `x || y` será `verdadeiro` se x ou y for avaliado como `verdadeiro`. Caso contrário, o resultado será `falso`. Se x for avaliado como `verdadeiro`, y não será avaliado.

### Operadores de Comparação

#### Operador de Igualdade `==`

O operador `==` retornará `verdadeiro` se o operando à esquerda for igual ao operando à direita, caso contrário, `falso`.

#### Operador de Desigualdade `!=`

O operador `!=` retornará `verdadeiro` se o operando à esquerda for diferente ao operando à direita, caso contrário, `falso`.

#### Operador Maior que `>`

O operador `>` retornará `verdadeiro` se o operando à esquerda for maior que o operando à direita, caso contrário, `falso`.

#### Operador Menor que `<`

O operador `<` retornará `verdadeiro` se o operando à esquerda for menor que o operando à direita, caso contrário, `falso`.

#### Operador Maior ou igual `>=`

O operador `>=` retornará `verdadeiro` se o operando à esquerda for maior ou igual ao operando à direita, caso contrário, `falso`.

#### Operador Menor ou igual `<=`

O operador `<=` retornará `verdadeiro` se o operando à esquerda for menor ou igual ao operando à direita, caso contrário, `falso`.

### Operadores de Atribuição

#### Operador de atribuição `=`

O operador `=` atribui o operando à direita à variável à esquerda.

#### Operador de atribuição de soma `+=`

O operador `+=` atribui à variável à esquerda a soma do valor da variável à esquerda e o operando à direita.

#### Operador de atribuição de subtração `-=`

O operador `-=` atribui à variável à esquerda a subtrai do valor do operando à direita da variável à esquerda.

#### Operador de atribuição de multiplicação `*=`

O operador `*=` atribui à variável à esquerda a multiplicação do valor da variável à esquerda e o operando à direita.

#### Operador de atribuição de divisão `/=`

O operador `/=` atribui à variável à esquerda a divisão do valor da variável à esquerda pelo operando à direita.

### Erros de arredondamento

Devido às limitações gerais da representação de pontos flutuantes de números reais e aritmética de ponto flutuante, erros de arredondamento podem ocorrer em cálculos com tipos de pontos flutuantes.

___

## 🟪 Funções de Suporte

As funções de suporte fornecem recursos previamente definidos para auxiliar na utilização da linguagem.

### Funções aritméticas

#### _ABS

A função `_ABS`, recebe como parâmetro um número decimal e retorna o valor absoluto de um número.

```csharp
_ABS(numero)
```

**Parâmetros**

`numero` O número a ser achar a valor absoluto.

**Retornos**

Número, cujo valor é equivalente ao valor absoluto do número fornecido.

**Exemplos**

```csharp
var numero = -9.97;
var numeroAbsoluto = _ABS(numero);
retorno numeroAbsoluto; // Retorna 9.97
```

#### _ARREDONDAR

A função `_ARREDONDAR`, recebe como parâmetro um número decimal e o arredonda para a quantidade de casas decimais informada, ou sem casas decimais caso não seja fornecida.

```csharp
_ARREDONDAR(entidade, casasDecimais)
```

**Parâmetros**

`entidade` Número a ser arredondado.  
`casasDecimais` Número de casas decimais.

**Retornos**

Número, cujo valor é equivalente ao número fornecido, arredondado para a quantidade de casas decimais fornecidas.

**Exemplos**

Arredondamento para cima

```csharp
var numero = 10.9415;
var numeroArredondado = _ARREDONDAR(numero, 3);
retorno numeroArredondado; // Retorna 10.942
```

Arredondamento para baixo

```csharp
var numero = 10.9414;
var numeroArredondado = _ARREDONDAR(numero, 3);
retorno numeroArredondado; // Retorna 10.941
```

#### _FATORIAL

A função `_FATORIAL`, recebe como parâmetro um número e calcula a sua fatorial.

```csharp
_FATORIAL(numero)
```

**Parâmetros**

`numero` Número a se calcular a fatorial.

**Retornos**

Número, cujo valor é equivalente a fatorial do número fornecido.

**Exemplos**

```csharp
var numero = 5;
var numeroFatorial = _FATORIAL(numero);
retorno numeroFatorial; // Retorna 120
```

#### _MOD

A função `_MOD`, recebe como parâmetro um número e um divisor e calcula o resto da divisão.

```csharp
_MOD(numero, divisor)
```

**Parâmetros**

`numero` Número a ser dividido.  
`divisor` Divisor do número.

**Retornos**

Número, cujo valor é equivalente ao resto da divisão do número fornecido com o divisor especificado.

**Exemplos**

```csharp
var numero = 3;
var numeroMod = _MOD(numero, 2);
retorno numeroMod; // Retorna 1
```

#### _POTENCIA

A função `_POTENCIA`, recebe como parâmetro um número e a potência a qual este deve ser elevado.

```csharp
_POTENCIA(numero, potencia)
```

**Parâmetros**

`numero` Número a ser elevado.  
`potencia` Potência a se elevar o número.

**Retornos**

Número, cujo valor é equivalente ao número fornecido elevado a potência especificada.

**Exemplos**

```csharp
var numero = 3;
var numeroPotencia = _POTENCIA(numero, 3);
retorno numeroPotencia; // Retorna 27
```

#### _RAIZ

A função `_RAIZ`, recebe como parâmetro um número decimal e calcula a sua raiz quadrada.

```csharp
_RAIZ(numero)
```

**Parâmetros**

`numero` Número a se encontrar a raiz quadrada.

**Retornos**

Número, cujo valor é equivalente a raiz quadrada do número fornecido.

**Exemplos**

```csharp
var numero = 9;
var numeroRaiz = _RAIZ(numero);
retorno numeroRaiz; // Retorna 3
```

#### _TRUNCAR

A função `_TRUNCAR`, recebe como parâmetro um número decimal e calcula a sua raiz quadrada.

```csharp
_TRUNCAR(numero)
```

**Parâmetros**

`entidade` Número a ser truncado.

**Retornos**

Número, cujo valor é equivalente ao número fornecido, sem suas casas decimais.

**Exemplos**

```csharp
var numero = 5.9;
var numeroTruncado = _TRUNCAR(numero);
retorno numeroTruncado; // Retorna 5
```

### Funções auxiliares

#### _ARRUMAR

A função `_ARRUMAR`, recebe como parâmetro um texto e remove espaços em branco do começo, final e espaços seguidos entre as palavras.

```csharp
_ARRUMAR(texto)
```

**Parâmetros**

`texto` Texto a ser arrumado.

**Retornos**

Texto, cujo valor é equivalente ao texto fornecido, sem espaços no início, final e espaços seguidos entre as palavras.

**Exemplos**

```csharp
var texto = "  Um   texto    ";
var novoTexto = _ARRUMAR(texto);
retorno novoTexto; // Retorna "Um texto"
```

#### _COALESCE

A função `_COALESCE`, recebe como parâmetro valores, e retorna o primeiro que não seja nulo.

```csharp
_COALESCE(entidade [, entidade])
```

**Parâmetros**

`entidade` Valor a ser verificado.

**Retornos**

Valor, equivalente ao primeiro que não seja nulo.

**Exemplos**

```csharp
var numero = nulo;
var valorNaoNulo = _COALESCE(numero, 9);
retorno valorNaoNulo; // Retorna 9
```

#### _LISTA

A função `_LISTA`, recebe como parâmetro uma entidade, e identifica se é uma lista.

```csharp
_LISTA(entidade)
```

**Parâmetros**

`entidade` Valor a ser verificado.

**Retornos**

Booleano, equivalente ao valor se tratar de uma lista.

**Exemplos**

```csharp
var lista = [];
var valorEhLista = _LISTA(lista);
retorno valorEhLista; // Retorna verdadeiro
```

#### _NULO

A função `_NULO`, recebe como parâmetro uma entidade, e identifica se é nula.

```csharp
_NULO(entidade)
```

**Parâmetros**

`entidade` Valor a ser verificado.

**Retornos**

Booleano, equivalente a comparação do valor com nulo.

**Exemplos**

```csharp
var numero = nulo;
var valorEhNulo = _NULO(numero);
retorno valorEhNulo; // Retorna verdadeiro
```

#### _NUMERO

A função `_NUMERO`, recebe como parâmetro uma entidade, e identifica se é numérica.

```csharp
_NUMERO(entidade)
```

**Parâmetros**

`entidade` Valor a ser verificado.

**Retornos**

Booleano, equivalente a verificação numérica do valor.

**Exemplos**

```csharp
var numero = 10.6;
var valorEhNumero = _NUMERO(numero);
retorno valorEhNumero; // Retorna verdadeiro
```

#### _TERNARIO

A função `_TERNARIO`, avalia a condição do primeiro parâmetro e em caso positivo retorna o segundo parâmetro, caso contrário retorna o terceiro

```csharp
_TERNARIO(condicao, casoPositivo, casoNegativo)
```

**Parâmetros**

`condicao` Condição a ser avaliada.  
`casoPositivo` Valor caso a comparação seja verdadeira.  
`casoNegativo` Valor caso a comparação seja falsa.

**Retornos**

Valor, retorna o parâmetro `casoPositivo` caso a condição for avaliada como verdadeira, ou `casoNegativo`, caso seja avaliada como falsa.

**Exemplos**

```csharp
var numero = 10;
var valorMinimo = _TERNARIO(numero >= 10, numero, 9);
retorno valorMinimo; // Retorna 10
```

### Funções de data

#### _AGORA

A função `_AGORA`, não recebe parâmetros, e retorna a data e hora atual do sistema.

```csharp
_AGORA();
```

**Retornos**

Data, cujo valor é equivalente ao momento da sua execução.

**Exemplos**

```csharp
var dataHora = _AGORA();
retorno dataHora; // Retorna "2021-06-04T11:52:38.5523565-03:00" quando executado em 04/06/2021 ás 11:52
```

#### _ANO

A função `_ANO`, recebe como parâmetro a data da qual se deseja extrair o ano.

```csharp
_ANO(data)
```

**Parâmetros**

`data` Data para se realizar a operação.

**Retornos**

Número, cujo valor é equivalente ao ano da data fornecida.

**Exemplos**

```csharp
var data = _DATA(01, 04, 2020);
var ano = _ANO(data);
retorno ano; // Retorna 2020
```

#### _ANO_ADICIONAR

A função `_ANO_ADICIONAR`, recebe como parâmetro a data a se realizar a operação, e a quantidade de anos a se adicionar.

```csharp
_ANO_ADICIONAR(data, anos)
```

**Parâmetros**

`data` Data para se realizar a operação.  
`anos` Quantidade de anos a se adicionar.

**Retornos**

Data, cujo valor é equivalente a data fornecida, acrescida do número de anos fornecido.

**Exemplos**

```csharp
var data = _DATA(01, 04, 2020);
var novaData = _ANO_ADICIONAR(data, 2);
retorno novaData; // Retorna "2022-04-01T00:00:00"
```

#### _DATA

A função `_DATA`, recebe os parâmetro dia, mês e ano, e opcionalmente hora e minuto, para formar uma data.

```csharp
_DATA(dia, mes, ano, hora, minuto);
```

**Parâmetros**

`dia` Dia, referente ao dia do mês (entre 1 e 31).  
`mes` Mês, referente ao mês do ano (entre 1 e 12).  
`ano` Ano (entre 1 e 9999).  
`hora` (Opcional) Hora (entre 0 e 23).  
`minuto` (Opcional) Minuto (entre 0 e 59).

**Retornos**

Data, cujo valor é equivalente ao dia, mês, ano e opcionalmente hora e minuto, especificados.

**Exemplos**

```csharp
var data = _DATA(30, 03, 2020);
retorno data; // Retorna "2020-03-30T00:00:00"
```

#### _DATADIF

A função `_DATADIF`, recebe os parâmetros data inicial, data final e unidade, para calcular a diferença entre as duas datas informadas, e utiliza a unidade para determinar como será devolvido o resultado, são aceitas as opções `DIA`, `MES` ou `ANO`.

```csharp
_DATADIF(dataInicial, dataFinal, unidade)
```

**Parâmetros**

`dataInicial` Data inicial para comparação.  
`dataFinal` Data final para comparação.  
`unidade` Unidade de retorno. (DIA, MES ou ANO)

**Retornos**

Número, cujo valor é equivalente a diferença entre as duas datas fornecidas, na unidade escolhida.

**Exemplos**

```csharp
var data1 = _DATA(01, 04, 2020);
var data2 = _DATA(21, 04, 2020);
var dif = _DATADIF(data1, data2, DIA);
retorno dif; // Retorna 20
```

#### _DIA

A função `_DIA`, recebe como parâmetro a data da qual se deseja extrair o dia.

```csharp
_DIA(data)
```

**Parâmetros**

`data` Data para se realizar a operação.

**Retornos**

Número, cujo valor é equivalente ao dia da data fornecida.

**Exemplos**

```csharp
var data = _DATA(01, 04, 2020);
var dia = _DIA(data);
retorno dia; // Retorna 1
```

#### _DIA_ADICIONAR

A função `_DIA_ADICIONAR`, recebe como parâmetro a data a se realizar a operação, e a quantidade de dias a se adicionar.

```csharp
_DIA_ADICIONAR(data, dias)
```

**Parâmetros**

`data` Data para se realizar a operação.  
`dias` Quantidade de dias a se adicionar.

**Retornos**

Data, cujo valor é equivalente a data fornecida, acrescida do número de dias fornecido.

**Exemplos**

```csharp
var data = _DATA(01, 04, 2020);
var novaData = _DIA_ADICIONAR(data, 8);
retorno novaData; // Retorna "2020-04-09T00:00:00"
```

#### _HOJE

A função `_HOJE`, adquire a data atual do sistema sem especificar a hora, minuto ou segundo.

```csharp
_HOJE();
```

**Retornos**

Data, cujo valor é equivalente ao momento da sua execução, sem hora, minuto e segundo.

**Exemplos**

```csharp
var data = _HOJE();
retorno data; // Retorna "2021-06-04T00:00:00-03:00" quando executado em 04/06/2021
```

#### _HORA

A função `_HORA`, recebe como parâmetro a data da qual se deseja extrair a hora.

```csharp
_HORA(dataHora)
```

**Parâmetros**

`data` Data para se realizar a operação.

**Retornos**

Número, cujo valor é equivalente a hora da data fornecida.

**Exemplos**

```csharp
var dataHora = _DATA(01, 04, 2020, 10, 50);
var hora = _HORA(dataHora);
retorno hora; // Retorna 10
```

#### _MES

A função `_MES`, recebe como parâmetro a data da qual se deseja extrair o mês.

```csharp
_MES(data)
```

**Parâmetros**

`data` Data para se realizar a operação.

**Retornos**

Número, cujo valor é equivalente ao mês da data fornecida.

**Exemplos**

```csharp
var data = _DATA(01, 04, 2020);
var mes = _MES(data);
retorno mes; // Retorna 4
```

#### _MES_ADICIONAR

A função `_MES_ADICIONAR`, recebe como parâmetro a data a se realizar a operação, e a quantidade de meses a se adicionar.

```csharp
_MES_ADICIONAR(data, meses)
```

**Parâmetros**

`data` Data para se realizar a operação.  
`meses` Quantidade de meses a se adicionar.

**Retornos**

Data, cujo valor é equivalente a data fornecida, acrescida do número de meses fornecido.

**Exemplos**

```csharp
var data = _DATA(01, 04, 2020);
var novaData = _MES_ADICIONAR(data, 8);
retorno novaData; // Retorna "2020-12-01T00:00:00"
```

#### _MINUTO

A função `_MINUTO`, recebe como parâmetro a data da qual se deseja extrair o minuto.

```csharp
_MINUTO(dataHora)
```

**Parâmetros**

`data` Data para se realizar a operação.

**Retornos**

Número, cujo valor é equivalente ao minuto da data fornecida.

**Exemplos**

```csharp
var dataHora = _DATA(01, 04, 2020, 10, 50);
var minuto = _MINUTO(dataHora);
retorno minuto; // Retorna 50
```

### Funções de lista

#### _CONT

A função `_CONT`, recebe como parâmetro uma lista, e conta a quantidade de itens da lista.

```csharp
_CONT(lista)
```

**Parâmetros**

`lista` Lista de valores a serem verificados.

**Retornos**

Número, equivalente a quantidade de itens na lista.

**Exemplos**

```csharp
var lista = [];
lista[0].Valor = 10;
lista[1].Valor = 12;
var total = _CONT(lista);
retorno total; // Retorna 2
```

#### _CONTSE

A função `_CONTSE`, recebe como parâmetro uma lista, e conta a quantidade de itens da lista que satisfazem a condição.

```csharp
_CONTSE(lista, condicao)
```

**Parâmetros**

`lista` Lista de valores a serem verificados.  
`condicao` Condição para se realizar a operação.

**Retornos**

Número, equivalente a quantidade de itens na lista que satisfazem a condição.

**Exemplos**

```csharp
var lista = [];
lista[0].Valor = 10;
lista[1].Valor = 12;
lista[2].Valor = 14;
var total = _CONTSE(lista, x => x.Valor > 10);
retorno total; // Retorna 2
```

#### _FILTRAR

A função `_FILTRAR`, recebe como parâmetro uma lista, e retorna somente os itens que satisfazem a condição informada.

```csharp
_FILTRAR(lista, condicao)
```

**Parâmetros**

`lista` Lista de valores a serem verificados.  
`condicao` Condição para se realizar a operação.

**Retornos**

Lista, equivalente aos itens que satisfazem a condição.

**Exemplos**

```csharp
var lista = [];
lista[0].Valor = 10;
lista[1].Valor = 12;
lista[2].Valor = 14;
var total = _FILTRAR(lista, x => x.Valor > 10);
retorno total; // Retorna 0: { Valor: 12 }, 1: { Valor: 14 }
```

#### _MAXIMO

A função `_MAXIMO`, recebe como parâmetro uma lista, e extrai o máximo entre os valores da propriedade especificada da lista.

```csharp
_MAXIMO(lista)
```

**Parâmetros**

`lista` Lista de valores a serem verificados.

**Retornos**

Número, equivalente ao valor máximo dos itens da propriedade especificada da lista.

**Exemplos**

```csharp
var lista = [];
lista[0].Valor = 10;
lista[1].Valor = 12;
var max = _MAXIMO(lista.Valor);
retorno max; // Retorna 12
```

#### _MEDIA

A função `_MEDIA`, recebe como parâmetro uma lista, e calcula a média entre os valores da propriedade especificada da lista.

```csharp
_MEDIA(lista)
```

**Parâmetros**

`lista` Lista de valores a serem verificados.

**Retornos**

Número, equivalente a média dos itens da propriedade especificada da lista.

**Exemplos**

```csharp
var lista = [];
lista[0].Valor = 10;
lista[1].Valor = 12;
var media = _MEDIA(lista.Valor);
retorno media; // Retorna 11
```

#### _MINIMO

A função `_MINIMO`, recebe como parâmetro uma lista, e extrai o mínimo entre os valores da propriedade especificada da lista.

```csharp
_MINIMO(lista)
```

**Parâmetros**

`lista` Lista de valores a serem verificados.

**Retornos**

Número, equivalente ao valor mínimo dos itens da propriedade especificada da lista.

**Exemplos**

```csharp
var lista = [];
lista[0].Valor = 10;
lista[1].Valor = 12;
var min = _MINIMO(lista.Valor);
retorno min; // Retorna 10
```

#### _PRIMEIRO

A função `_PRIMEIRO`, recebe como parâmetro uma lista, e retorna somente o primeiro item que satisfaz a condição informada.

```csharp
_PRIMEIRO(lista, condicao)
```

**Parâmetros**

`lista` Lista de valores a serem verificados.  
`condicao` Condição para se realizar a operação.

**Retornos**

Objeto, equivalente ao item que satisfaz a condição.

**Exemplos**

```csharp
var lista = [];
lista[0].Valor = 10;
lista[1].Valor = 12;
lista[2].Valor = 14;
var total = _PRIMEIRO(lista, x => x.Valor > 10);
retorno total; // Retorna { Valor: 12 }
```

#### _SOMA

A função `_SOMA`, recebe como parâmetro uma lista, e soma todos os valores da propriedade especificada da lista informada.

```csharp
_SOMA(lista)
```

**Parâmetros**

`lista` Lista de valores a serem verificado.

**Retornos**

Número, equivalente a soma dos itens da propriedade especificada da lista.

**Exemplos**

```csharp
var lista = [];
lista[0].Valor = 10;
lista[1].Valor = 12;
var soma = _SOMA(lista.Valor);
retorno soma; // Retorna 22
```

#### _SOMASE

A função `_SOMASE`, recebe como parâmetro uma lista, e soma todos os valores da propriedade especificada da lista que satisfazem a condição.

```csharp
_SOMASE(lista, condicao)
```

**Parâmetros**

`lista` Lista de valores a serem verificado.  
`condicao` Condição para se realizar a operação.

**Retornos**

Número, equivalente a soma dos itens da propriedade especificada da lista que condizem com a condição informada.

**Exemplos**

```csharp
var lista = [];
lista[0].Valor = 10;
lista[1].Valor = 12;
lista[2].Valor = 14;
var soma = _SOMASE(lista.Valor, x => x.Valor > 10);
retorno soma; // Retorna 10
```

### Funções específicas Tributário

#### _ATIVIDADETABELA

A função `_ATIVIDADETABELA`, busca os dados de valor da atividade conforme tabela, tipo, e exercício informados, ou caso o exercício não seja informado retorna os dados da tabela informada.

```csharp
_ATIVIDADETABELA(tabela, tipo [, exercicio])
```

**Parâmetros**

`tabela` Tabela a consultar a atividade.  
`tipo` Tipo da atividade.  
`exercicio` (Opcional) Exercício da atividade, caso não informado retorna os dados da tabela informada.

**Retornos**

Lista de objetos, contendo IdAtividade, TpAtividade, Exercicio, VlrAtividade, Aliquota, Valor e DtInicio, ou caso o exercício seja informado, retorna um objeto contendo as colunas personalizadas da tabela consultada.

**Exemplos**

Consulta sem informar exercício.

```csharp
var atividades = _ATIVIDADETABELA("CcmAtividades", "ALVARA");
retorno atividades; // Retorna 0: { CcmAtividades.IdOrigem: 1, CcmAtividades.IdAtividade: 4, CcmAtividades.Vlr: 38.5, CcmAtividades.TpCalculo: Não Tributavel, CcmAtividades.EhPrincipal: N, CcmAtividades.Quantidade: 1, CcmAtividades.DtInicio: 2020-01-01T00:00:00, CcmAtividades.DtFim: undefined, CcmAtividades.AliquotaEspecial: undefined, Atividades.Situacao: A, Atividades.TpAtividade: ALVARA, Atividades.TpVlr: undefined, Atividades.VlrAtividade: 0, Atividades.Aliquota: 0, Atividades.Limite: undefined, Atividades.VlrExcesso: undefined }
```

Consulta sem informar exercício, extraindo propriedade.

```csharp
var atividades = _ATIVIDADETABELA("CcmAtividades", "ALVARA");
retorno atividades[0]["CcmAtividades.Vlr"]; // Retorna 38.5
```

Consulta informando exercício.

```csharp
var atividades = _ATIVIDADETABELA("CcmAtividades", "ALVARA", 2020);
retorno atividades; // Retorna 0: { IdAtividade: 4, TpAtividade: ALVARA, Exercicio: 2020, VlrAtividade: 0, Aliquota: 0, Valor: 35.0462, DtInicio: undefined },  1: { IdAtividade: 14, TpAtividade: ALVARA, Exercicio: 2020, VlrAtividade: 0, Aliquota: 0, Valor: 56.9427, DtInicio: undefined }
```

Consulta informando exercício, extraindo propriedade.

```csharp
var atividades = _ATIVIDADETABELA("CcmAtividades", "ALVARA", 2020);
retorno atividades[0].Valor; // Retorna 35.0462
```

#### _CARACTERISTICA

A função `_CARACTERISTICA`, busca os dados de valor da característica conforme descrição, módulo, e opcionalmente exercício, informados.

```csharp
_CARACTERISTICA(descricao, modulo [, exercicio])
```

**Parâmetros**

`descricao` Descrição da característica.  
`modulo` Módulo da característica.  
`exercicio` (Opcional) Exercício do valor da característica, caso não informado utiliza o exercício atual.

**Retornos**

Lista de objetos, contendo IdCaracteristica, DescrCaracteristica, Modulo, CodItem, DescrItem, Exercicio, Valor e Fator.

**Exemplos**

```csharp
var descricao = "ESCOLA";
var modulo = "Terreno";
var caracteristicas = _CARACTERISTICA(descricao, modulo, 2019);
retorno caracteristicas; // Retorna 0: { IdCaracteristica: 7, DescrCaracteristica: ESCOLA, Modulo: Terreno, CodItem: 01, DescrItem: SIM, Exercicio: 2019, Valor: undefined, Fator: 1.15 }, 1: { IdCaracteristica: 7, DescrCaracteristica: ESCOLA, Modulo: Terreno, CodItem: 02, DescrItem: NÃO, Exercicio: 2019, Valor: undefined, Fator: 1.15 }
```

```csharp
var descricao = "ESCOLA";
var modulo = "Terreno";
var caracteristicas = _CARACTERISTICA(descricao, modulo, 2019);
retorno caracteristicas[0].Fator; // Retorna 1.15
```

#### _CARACTERISTICA_CODIGO

A função `_CARACTERISTICA_CODIGO`, busca os dados de valor da característica conforme descrição, módulo, código, e opcionalmente exercício, informados.

```csharp
_CARACTERISTICA_CODIGO(descricao, modulo, codigo [, exercicio])
```

**Parâmetros**

`descricao` Descrição da característica.  
`modulo` Módulo da característica.  
`codigo` Código do valor da característica.  
`exercicio` (Opcional) Exercício do valor da característica, caso não informado utiliza o exercício atual.

**Retornos**

Lista de objetos, com cada item contendo IdCaracteristica, DescrCaracteristica, Modulo, CodItem, DescrItem, Exercicio, Valor e Fator.

**Exemplos**

```csharp
var caracteristicas = _CARACTERISTICA_CODIGO("ESCOLA", "Terreno", "01", 2019);
retorno caracteristicas; // Retorna 0: { IdCaracteristica: 7, DescrCaracteristica: ESCOLA, Modulo: Terreno, CodItem: 01, DescrItem: SIM, Exercicio: 2019, Valor: undefined, Fator: 1.15 }
```

```csharp
var caracteristicas = _CARACTERISTICA_CODIGO("ESCOLA", "Terreno", "01", 2019);
retorno caracteristicas[0].Fator; // Retorna 1.15
```

#### _CARACTERISTICATABELA

A função `_CARACTERISTICATABELA`, busca os dados de valor da característica conforme tabela, descrição, e exercício informados, ou caso o exercício não seja informado retorna os dados da tabela informada.

```csharp
_CARACTERISTICATABELA(tabela, descricao [, exercicio])
```

**Parâmetros**

`tabela` Tabela a consultar a característica.  
`descricao` Descrição da característica.  
`exercicio` (Opcional) Exercício da característica, caso não informado retorna os dados da tabela informada.

**Retornos**

Objeto, contendo IdCaracteristica, DescrCaracteristica, Modulo, CodItem, DescrItem, Exercicio, Valor e Fator, ou caso o exercício seja informado, retorna um objeto contendo as colunas da tabela consultada.

**Exemplos**

Consulta sem informar exercício.

```csharp
var caracteristica = _CARACTERISTICATABELA("FisicoCaracteristicas", "ESCOLA");
retorno caracteristica; // Retorna { FisicoCaracteristicas.IdOrigem: 1, FisicoCaracteristicas.IdCaracteristica: 7, FisicoCaracteristicas.Modulo: Terreno, FisicoCaracteristicas.DescrCaracteristica: ESCOLA, FisicoCaracteristicas.Vlr: 01 }
```

Consulta sem informar exercício, extraindo propriedade.

```csharp
var caracteristica = _CARACTERISTICATABELA("FisicoCaracteristicas", "ESCOLA");
retorno caracteristica["FisicoCaracteristicas.Vlr"]; // Retorna "01"
```

Consulta informando exercício.

```csharp
var caracteristica = _CARACTERISTICATABELA("FisicoCaracteristicas", "ESCOLA", 2019);
retorno caracteristica; // Retorna { IdCaracteristica: 7, DescrCaracteristica: ESCOLA, Modulo: Terreno, CodItem: 01, DescrItem: SIM, Exercicio: 2019, Valor: Sem valor, Fator: 1.15 }
```

Consulta informando exercício, extraindo propriedade.

```csharp
var caracteristica = _CARACTERISTICATABELA("FisicoCaracteristicas", "ESCOLA", 2019);
retorno caracteristica.Fator; // Retorna 1.15
```

#### _PARAMETRO

A função `_PARAMETRO`, busca os dados de valor do parâmetro conforme nome, descrição, e opcionalmente exercício, informados.

```csharp
_PARAMETRO(nome, descricao [, exercicio])
```

**Parâmetros**

`nome` Nome do parâmetro.  
`descricao`  Descrição do parâmetro.  
`exercicio` (Opcional) Exercício referente ao valor do parâmetro, caso não informado utiliza o exercício atual.

**Retornos**

Lista de objetos, com cada item contendo NomeParam, Descricao, Modulo, Codigo, Exercicio, Valor.

**Exemplos**

```csharp
var parametros = _PARAMETRO("AliquotaINSS", "AliquotaINSS", 2016);
retorno parametros; // Retorna 0: { NomeParam: AliquotaINSS, Descricao: AliquotaINSS, Codigo: undefined, Exercicio: 2016, Valor: 2,00 }
```

```csharp
var parametros = _PARAMETRO("AliquotaINSS", "AliquotaINSS", 2016);
retorno parametros[0].Valor; // Retorna "2,00"
```

#### _PARAMETRO_CODIGO

A função `_PARAMETRO_CODIGO`, busca os dados de valor do parâmetro conforme nome, descrição, código, e opcionalmente exercício, informados.

```csharp
_PARAMETRO_CODIGO(nome, descricao, codigo [, exercicio])
```

**Parâmetros**

`nome` Nome do parâmetro.  
`descricao` Descrição do parâmetro.  
`codigo` Código do valor do parâmetro.  
`exercicio` (Opcional) Exercício referente ao valor do parâmetro, caso não informado utiliza o exercício atual.

**Retornos**

Lista de objetos, com cada item contendo NomeParam, Descricao, Modulo, Codigo, Exercicio, Valor.

**Exemplos**

```csharp
var parametros = _PARAMETRO_CODIGO("LicencaVlrPorEmpregado", "Licenca Vlr Por Empregado", "1", 2015);
retorno parametros; // Retorna 0: { NomeParam: LicencaVlrPorEmpregado, Descricao: Licenca Vlr Por Empregado, Codigo: 1, Exercicio: 2015, Valor: 1.0285575 }
```

```csharp
var parametros = _PARAMETRO_CODIGO("LicencaVlrPorEmpregado", "Licenca Vlr Por Empregado", "1", 2015);
retorno parametros[0].Valor; // Retorna "1.0285575"
```

#### _RETORNOVALOR

A função `_RETORNOVALOR`, disponibiliza para o processamento pós execução o valor informado, junto a sua identificação, caso a mesma identificação seja utilizada nas fórmulas de um roteiro seu valor é sobrescrito pelo mais recente.

```csharp
_RETORNOVALOR(nome, valor)
```

**Parâmetros**

`nome` Identificação do valor.  
`valor` Valor a ser retornado.

**Retornos**

Valor, equivalente ao valor especificado.

**Exemplos**

```csharp
_RETORNOVALOR("Nome", "Nome teste");
retorno; // Retorno valor de { "Nome": "Nome teste" }
```

___

## 🔷 Constantes de Suporte

As constantes de suporte são valores referentes ao registro sendo processado, assim se uma fórmula é feita para o Setor Origem Imobiliário, por exemplo, terá disponível os valores de Fisico, FisicoAreas e FisicoOutros, entre outros, para realizar o cálculo, além dos resultados das fórmulas anteriores na ordem de execução, identificados por `@Roteiro` e valores externos referentes a execução identificados por `@Variavel`.

Constantes de suporte são identificadas pelo caractere '@' no início do identificador, e podem conter uma lista de dados, como por exemplo a utilização de dados de FisicoAreas no Setor Origem Imobiliário, já que um físico pode possuir diversas áreas associadas ao seu registro.

**Exemplos**

Utilizando propriedade Área Comum da tabela Físico.

```csharp
retorno @Fisico.AreaComum;
```

Utilizando tabela relacionada Físico Áreas, na qual possui relação de muitos para um com Físico, portanto consulta através de lista.

```csharp
var resultado = 0.25;
var indice = 0;
enquanto (indice < _CONT(@FisicoAreas)) {
    se (@FisicoAreas[indice].Area < 100) {
        resultado += @FisicoAreas[indice].Area;
    }
    indice++;
}

retorno resultado;
```

Utilizando valor externo.

```csharp
retorno @Variavel.Teste;
```

### Resultado de Fórmulas anteriores

É possível também utilizar o resultado de fórmulas anteriores, para apurar o resultado da fórmula atual, onde o acesso é realizado através da palavra chave `@Roteiro`.

**Exemplos**

Primeira Fórmula, nomeada de `FormulaDecimal`.

```csharp
var resultado = 10.5;
retorno resultado;
```

Segunda Fórmula.

```csharp
retorno @Roteiro.FormulaDecimal * 2; // Retorna 21
```

Utilizando retorno de lista.

Primeira Fórmula, nomeada de `FormulaLista`:

```csharp
lista resultado = [];
resultado[0].Taxa = 2.1;
resultado[1].Taxa = 3.4;
resultado[2].Taxa = 4.3;
retorno resultado;
```

Segunda Fórmula.

```csharp
var resultado = 0;
se (_CONT(@Roteiro.FormulaLista) > 0) { // Verificação se a lista contém itens
    resultado = 10 * @Roteiro.FormulaLista[0].Taxa;
}
retorno resultado; // Retorna 21
```
___

## 📘 Referências

Esta definição teve como base para estruturação e definições a referência de linguagem do C#, disponível em <https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/>.
