# 00 - Introdução ao C#

## 📖 O que é C#?

C# (pronunciado "C Sharp") é uma linguagem de programação moderna, orientada a objetos, desenvolvida pela Microsoft. É a linguagem principal do ecossistema .NET e uma das mais utilizadas no mundo.

### Por que aprender C#?

- **Linguagem moderna**: Sintaxe clara e expressiva
- **Alta produtividade**: Muitos recursos built-in
- **Ecossistema rico**: .NET, NuGet, ferramentas
- **Multiplataforma**: Windows, Linux, macOS, mobile
- **Mercado de trabalho**: Alta demanda por profissionais C#

## 🎯 O que você vai aprender

- Sintaxe básica da linguagem
- Tipos de dados e variáveis
- Operadores e expressões
- Estruturas de controle (if, loops)
- Métodos e funções
- Trabalhar com console

## 📝 Primeiro Programa

### Criando seu primeiro projeto

```bash
# Criar nova aplicação console
dotnet new console -n MeuPrimeiroPrograma

# Entrar na pasta
cd MeuPrimeiroPrograma

# Executar
dotnet run
```

**Resultado esperado:**
```
Hello, World!
```

### Modificando o código

Abra `Program.cs` e substitua o conteúdo:

```csharp
// Program.cs
Console.WriteLine("Olá, mundo!");
Console.WriteLine("Bem-vindo ao C#!");
```

Execute novamente:
```bash
dotnet run
```

**Saída:**
```
Olá, mundo!
Bem-vindo ao C#!
```

## 📊 Tipos de Dados

### Tipos Primitivos

```csharp
// Números inteiros
int idade = 25;
long populacao = 7800000000;

// Números decimais
float preco = 19.99f;
double salario = 3500.50;
decimal dinheiro = 1500.75m;

// Texto
string nome = "João Silva";
char letra = 'A';

// Booleano
bool ativo = true;
bool aprovado = false;

// Data e hora
DateTime nascimento = new DateTime(1990, 5, 15);
```

### Conversão de Tipos

```csharp
// Conversão implícita (segura)
int numero = 42;
double numeroDouble = numero; // OK

// Conversão explícita (pode perder dados)
double valor = 123.45;
int valorInteiro = (int)valor; // 123

// Parse de string
string texto = "456";
int numeroParse = int.Parse(texto);

// TryParse (mais seguro)
if (int.TryParse(texto, out int resultado)) {
    Console.WriteLine($"Convertido: {resultado}");
}
```

## 🔧 Variáveis e Constantes

```csharp
// Variáveis (podem mudar)
int contador = 0;
contador = contador + 1; // contador = 1

// Constantes (não podem mudar)
const double PI = 3.14159;
const string EMPRESA = "Minha Empresa";

// Inferência de tipo (var)
var nome = "Maria";        // string
var idade = 30;           // int
var altura = 1.75;        // double
var ativo = true;         // bool
```

## 🧮 Operadores

### Aritméticos
```csharp
int a = 10;
int b = 3;

Console.WriteLine(a + b);  // 13 (soma)
Console.WriteLine(a - b);  // 7 (subtração)
Console.WriteLine(a * b);  // 30 (multiplicação)
Console.WriteLine(a / b);  // 3 (divisão inteira)
Console.WriteLine(a % b);  // 1 (resto)
```

### Comparação
```csharp
int x = 5;
int y = 10;

Console.WriteLine(x == y); // false (igual)
Console.WriteLine(x != y); // true (diferente)
Console.WriteLine(x < y);  // true (menor)
Console.WriteLine(x > y);  // false (maior)
Console.WriteLine(x <= y); // true (menor ou igual)
Console.WriteLine(x >= y); // false (maior ou igual)
```

### Lógicos
```csharp
bool condicao1 = true;
bool condicao2 = false;

Console.WriteLine(condicao1 && condicao2); // false (E)
Console.WriteLine(condicao1 || condicao2); // true (OU)
Console.WriteLine(!condicao1);             // false (NÃO)
```

## 🎛️ Estruturas de Controle

### Condicional if-else

```csharp
int idade = 18;

if (idade >= 18) {
    Console.WriteLine("Maior de idade");
} else {
    Console.WriteLine("Menor de idade");
}

// if-else aninhado
if (idade < 13) {
    Console.WriteLine("Criança");
} else if (idade < 18) {
    Console.WriteLine("Adolescente");
} else {
    Console.WriteLine("Adulto");
}
```

### Switch Expression (C# 8+)

```csharp
int diaSemana = 3;
string nomeDia = diaSemana switch {
    1 => "Domingo",
    2 => "Segunda-feira",
    3 => "Terça-feira",
    4 => "Quarta-feira",
    5 => "Quinta-feira",
    6 => "Sexta-feira",
    7 => "Sábado",
    _ => "Dia inválido"
};

Console.WriteLine(nomeDia); // Terça-feira
```

### Loops

```csharp
// for loop
for (int i = 0; i < 5; i++) {
    Console.WriteLine($"Contador: {i}");
}

// while loop
int contador = 0;
while (contador < 3) {
    Console.WriteLine($"While: {contador}");
    contador++;
}

// do-while loop
int numero = 0;
do {
    Console.WriteLine($"Do-While: {numero}");
    numero++;
} while (numero < 2);

// foreach (para arrays/coleções)
int[] numeros = { 1, 2, 3, 4, 5 };
foreach (int num in numeros) {
    Console.WriteLine($"Número: {num}");
}
```

## 📞 Métodos e Funções

### Sintaxe Básica

```csharp
// Método sem retorno
void Saudacao() {
    Console.WriteLine("Olá!");
}

// Método com parâmetros
void SaudacaoPersonalizada(string nome) {
    Console.WriteLine($"Olá, {nome}!");
}

// Método com retorno
int Somar(int a, int b) {
    return a + b;
}

// Método com múltiplos parâmetros
double CalcularMedia(double n1, double n2, double n3) {
    return (n1 + n2 + n3) / 3;
}
```

### Chamando Métodos

```csharp
// Chamadas
Saudacao();                           // Olá!
SaudacaoPersonalizada("João");       // Olá, João!
int resultado = Somar(5, 3);         // resultado = 8
double media = CalcularMedia(7, 8, 9); // media = 8.0
```

## 💻 Trabalhando com Console

### Entrada e Saída

```csharp
// Saída
Console.WriteLine("Texto com quebra de linha");
Console.Write("Texto sem quebra de linha");

// Entrada
Console.Write("Digite seu nome: ");
string nome = Console.ReadLine();

Console.Write("Digite sua idade: ");
string idadeTexto = Console.ReadLine();
int idade = int.Parse(idadeTexto);

// Entrada segura
Console.Write("Digite um número: ");
if (int.TryParse(Console.ReadLine(), out int numero)) {
    Console.WriteLine($"Número digitado: {numero}");
} else {
    Console.WriteLine("Número inválido!");
}
```

### Formatação de Strings

```csharp
string nome = "Maria";
int idade = 30;
double altura = 1.65;

// Concatenação
Console.WriteLine("Nome: " + nome + ", Idade: " + idade);

// Interpolação (recomendado)
Console.WriteLine($"Nome: {nome}, Idade: {idade}, Altura: {altura:F2}");

// Formatação composta
Console.WriteLine("Nome: {0}, Idade: {1}", nome, idade);
```

## 🎯 Exercício Prático

### Calculadora Simples

Crie uma calculadora que:

1. Peça ao usuário dois números
2. Peça a operação (+, -, *, /)
3. Realize o cálculo
4. Mostre o resultado

```csharp
// Program.cs
Console.Write("Digite o primeiro número: ");
double num1 = double.Parse(Console.ReadLine());

Console.Write("Digite o segundo número: ");
double num2 = double.Parse(Console.ReadLine());

Console.Write("Digite a operação (+, -, *, /): ");
string operacao = Console.ReadLine();

double resultado = 0;
bool operacaoValida = true;

if (operacao == "+") {
    resultado = num1 + num2;
} else if (operacao == "-") {
    resultado = num1 - num2;
} else if (operacao == "*") {
    resultado = num1 * num2;
} else if (operacao == "/") {
    if (num2 != 0) {
        resultado = num1 / num2;
    } else {
        Console.WriteLine("Erro: Divisão por zero!");
        operacaoValida = false;
    }
} else {
    Console.WriteLine("Operação inválida!");
    operacaoValida = false;
}

if (operacaoValida) {
    Console.WriteLine($"Resultado: {num1} {operacao} {num2} = {resultado:F2}");
}
```

### Exercício Bônus: Validação

Melhore a calculadora adicionando validação de entrada:

```csharp
// Versão melhorada com validação
static double LerNumero(string mensagem) {
    while (true) {
        Console.Write(mensagem);
        if (double.TryParse(Console.ReadLine(), out double numero)) {
            return numero;
        }
        Console.WriteLine("Por favor, digite um número válido!");
    }
}

static string LerOperacao() {
    while (true) {
        Console.Write("Digite a operação (+, -, *, /): ");
        string op = Console.ReadLine();
        if (op == "+" || op == "-" || op == "*" || op == "/") {
            return op;
        }
        Console.WriteLine("Operação inválida! Use +, -, * ou /");
    }
}
```

## ✅ Verificação

Para prosseguir, certifique-se de:

- [ ] Criou e executou seu primeiro programa C#
- [ ] Entende tipos de dados e variáveis
- [ ] Sabe usar operadores e estruturas de controle
- [ ] Criou e chamou métodos
- [ ] Conseguiu fazer a calculadora funcionar
- [ ] Completou o exercício bônus

## 🎯 Próximos Passos

No próximo módulo, vamos aprender **Orientação a Objetos** - classes, objetos, herança e muito mais!

---

[← Voltar à trilha](../README.md) | [Próximo: Orientação a Objetos →](./01-orientacao-objetos.md)