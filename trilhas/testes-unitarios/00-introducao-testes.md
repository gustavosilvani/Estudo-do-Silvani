# 00 - Introdução aos Testes

## 🎯 Por que Testar Código?

Testes não são opcionais. São **essenciais** para desenvolvimento profissional de software. Vamos entender o porquê.

## 📊 Cenário Real

### Sem Testes ❌

```csharp
public decimal CalcularFrete(Pedido pedido) {
    return pedido.Peso * 1.5m; // Bug: deveria ser 0.5m
}

// Deploy para produção...
// Clientes reclamando de frete caro...
// Reverter código às pressas...
// Perda de confiança...
```

### Com Testes ✅

```csharp
[Test]
public void CalcularFrete_Peso5Kg_Retorna2Reais50() {
    var pedido = new Pedido { Peso = 5 };
    var frete = new CalculadoraFrete();
    var resultado = frete.Calcular(pedido);

    Assert.Equal(2.5m, resultado);
}

// Antes do deploy: dotnet test ✅
// Bug descoberto antes de chegar em produção!
```

## 🧪 O que são Testes Unitários?

### Definição
Teste unitário verifica o comportamento de **uma unidade pequena** de código (método ou classe) de forma **isolada**.

### Características
- ✅ **Rápidos**: Executam em milissegundos
- ✅ **Isolados**: Não dependem de banco, API, arquivo
- ✅ **Repetíveis**: Mesmo resultado sempre
- ✅ **Independentes**: Ordem não importa
- ✅ **Automáticos**: Passam ou falham sozinhos

## 🎯 Benefícios dos Testes

### 1. **Detecção Precoce de Bugs**
```csharp
// Código novo quebra funcionalidade antiga
public void AdicionarItem(Pedido pedido, Item item) {
    pedido.Itens.Add(item);
    pedido.Total += item.Preco; // Esqueceu de atualizar total!
}

// Teste falha: ❌ Total não foi atualizado
```

### 2. **Refatoração Segura**
```csharp
// Quer melhorar performance
public List<Cliente> BuscarAtivos() {
    return _clientes.Where(c => c.Ativo).ToList(); // LINQ
}

// Torna mais eficiente
public List<Cliente> BuscarAtivos() {
    var ativos = new List<Cliente>();
    foreach (var cliente in _clientes) {
        if (cliente.Ativo) ativos.Add(cliente);
    }
    return ativos;
}

// Testes garantem: mesma funcionalidade ✅
```

### 3. **Documentação Viva**
```csharp
[Test]
public void AplicarDesconto_VipComValorAlto_Retorna20Porcento() {
    // Documenta regra de negócio
    var cliente = new Cliente { Tipo = "VIP" };
    var resultado = _desconto.Aplicar(1000, cliente);

    Assert.Equal(800, resultado); // 20% desconto
}
```

### 4. **Confiança para Deploy**
```bash
# CI/CD pipeline
dotnet test --no-build
# ✅ Todos testes passaram
# 🚀 Deploy seguro
```

## 📋 Tipos de Testes

### Pirâmide de Testes

```
     /\
    /E2E\
   /------\
  /Integração\
 /------------\
/  Unitários  \
---------------
```

- **Unitários (70-80%)**: Base da pirâmide, mais numerosos
- **Integração (15-20%)**: Testam colaboração entre unidades
- **E2E (5-10%)**: Testam sistema completo

## 🛠️ Primeiro Teste Prático

### 1. Criar Projeto de Testes

```bash
# Criar solução
dotnet new sln -n CalculadoraApp

# Criar projeto principal
dotnet new classlib -n CalculadoraApp
dotnet sln add CalculadoraApp/CalculadoraApp.csproj

# Criar projeto de testes
dotnet new xunit -n CalculadoraApp.Tests
dotnet sln add CalculadoraApp.Tests/CalculadoraApp.Tests.csproj

# Referência entre projetos
dotnet add CalculadoraApp.Tests/CalculadoraApp.Tests.csproj reference CalculadoraApp/CalculadoraApp.csproj
```

### 2. Implementar Classe

```csharp
// CalculadoraApp/Calculadora.cs
namespace CalculadoraApp;

public class Calculadora {
    public int Somar(int a, int b) => a + b;
    public int Subtrair(int a, int b) => a - b;
    public int Multiplicar(int a, int b) => a * b;
    public double Dividir(int a, int b) => b == 0 ? throw new DivideByZeroException() : (double)a / b;
}
```

### 3. Escrever Primeiro Teste

```csharp
// CalculadoraApp.Tests/CalculadoraTests.cs
using Xunit;

namespace CalculadoraApp.Tests;

public class CalculadoraTests {
    [Fact]
    public void Somar_DoisNumerosPositivos_RetornaSomaCorreta() {
        // Arrange
        var calculadora = new Calculadora();
        const int a = 5;
        const int b = 3;

        // Act
        var resultado = calculadora.Somar(a, b);

        // Assert
        Assert.Equal(8, resultado);
    }
}
```

### 4. Executar Teste

```bash
# Na pasta da solução
dotnet test

# Resultado esperado:
# ✅ CalculadoraTests.Somar_DoisNumerosPositivos_RetornaSomaCorreta
# Testes executados: 1, Aprovados: 1, Falhas: 0
```

## 🎯 Padrões de Teste

### AAA Pattern (Arrange, Act, Assert)

```csharp
[Fact]
public void Metodo_Condicao_ResultadoEsperado() {
    // Arrange - Preparar cenário
    var sut = new SystemUnderTest(); // System Under Test
    var input = "dados de entrada";

    // Act - Executar ação
    var result = sut.Method(input);

    // Assert - Verificar resultado
    Assert.Equal("resultado esperado", result);
}
```

### Nomenclatura
```
MétodoTestado_CondiçãoCenário_ResultadoEsperado
```

Exemplos:
- `Somar_DoisNumerosPositivos_RetornaSomaCorreta`
- `Dividir_DivisorZero_LancaExcecao`
- `BuscarCliente_IdExistente_RetornaClienteCorreto`

## 📊 Testes Parametrizados

### InlineData
```csharp
[Theory]
[InlineData(2, 3, 5)]
[InlineData(-1, 1, 0)]
[InlineData(0, 0, 0)]
public void Somar_DoisNumeros_RetornaSomaCorreta(int a, int b, int esperado) {
    var calc = new Calculadora();
    var resultado = calc.Somar(a, b);
    Assert.Equal(esperado, resultado);
}
```

### MemberData
```csharp
public static IEnumerable<object[]> DadosSoma =>
    new List<object[]>
    {
        new object[] { 2, 3, 5 },
        new object[] { -1, 1, 0 },
        new object[] { 10, -5, 5 }
    };

[Theory]
[MemberData(nameof(DadosSoma))]
public void Somar_VariosCasos_RetornaSomaCorreta(int a, int b, int esperado) {
    // Mesmo teste...
}
```

## 🚨 Quando NÃO Testar

### Não teste:
- **Frameworks**: `List.Add()`, `string.ToUpper()`
- **Bibliotecas externas**: Entity Framework, HttpClient
- **Getters/Setters**: Propriedades simples
- **Privates**: Métodos privados (teste via públicos)

### Foque em:
- **Lógica de negócio**: Regras específicas da aplicação
- **Cenários complexos**: Condicionais, loops, cálculos
- **Casos especiais**: Exceções, validações, edge cases

## 🎯 Exercício Prático

### Implemente testes para a Calculadora

Adicione testes para os métodos restantes:

```csharp
[Test]
public void Subtrair_DoisNumeros_RetornaDiferencaCorreta() {
    // ✅ Implemente
}

[Test]
public void Multiplicar_DoisNumeros_RetornaProdutoCorreto() {
    // ✅ Implemente
}

[Test]
public void Dividir_DivisorValido_RetornaQuocienteCorreto() {
    // ✅ Implemente
}

[Test]
public void Dividir_DivisorZero_LancaExcecao() {
    // ✅ Implemente - usar Assert.Throws
}
```

### Dicas:
- Use `[Fact]` para testes simples
- Use `[Theory]` com `[InlineData]` para múltiplos casos
- Teste casos normais E casos especiais
- Use nomes descritivos

## ✅ Verificação

Para prosseguir, certifique-se de:

- [ ] Projeto de testes criado e configurado
- [ ] Pelo menos 3 testes implementados
- [ ] Todos testes passando (`dotnet test`)
- [ ] Entendido padrão AAA
- [ ] Conhece diferença entre Fact e Theory

## 🎯 Próximos Passos

No próximo módulo, vamos aprender **xUnit avançado** e técnicas de teste mais sofisticadas!

---

[← Voltar à trilha](../README.md) | [Próximo: xUnit e Primeiros Testes →](./01-xunit-primeiros-testes.md)