# 01 - xUnit e Primeiros Testes

## 📖 Framework xUnit

xUnit é o framework de testes mais moderno e recomendado para .NET.

## 🎯 Configuração Inicial

### Criar Projeto de Testes

```bash
# Criar solução
dotnet new sln -n MeuProjeto

# Criar projeto principal
dotnet new classlib -n MeuProjeto.Core
dotnet sln add MeuProjeto.Core

# Criar projeto de testes
dotnet new xunit -n MeuProjeto.Tests
dotnet sln add MeuProjeto.Tests

# Adicionar referência
cd MeuProjeto.Tests
dotnet add reference ../MeuProjeto.Core/MeuProjeto.Core.csproj
```

### Pacotes NuGet

```xml
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.9.0" />
<PackageReference Include="xunit" Version="2.7.0" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.7" />
<PackageReference Include="coverlet.collector" Version="6.0.0" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
```

## 🧪 Anatomia de um Teste

### Attributes Básicos

```csharp
using Xunit;

public class CalculadoraTests {
    // [Fact] - Teste simples sem parâmetros
    [Fact]
    public void Somar_DoisNumerosPositivos_RetornaSomaCorreta() {
        // Arrange
        var calculadora = new Calculadora();

        // Act
        var resultado = calculadora.Somar(2, 3);

        // Assert
        Assert.Equal(5, resultado);
    }

    // [Theory] - Teste parametrizado
    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(0, 0, 0)]
    [InlineData(-1, 1, 0)]
    [InlineData(10, -5, 5)]
    public void Somar_VariosNumeros_RetornaSomaCorreta(int a, int b, int esperado) {
        var calc = new Calculadora();
        var resultado = calc.Somar(a, b);
        Assert.Equal(esperado, resultado);
    }
}
```

## ✅ Assertions Comuns

### Igualdade

```csharp
// Valores
Assert.Equal(5, resultado);
Assert.NotEqual(0, resultado);

// Strings
Assert.Equal("esperado", resultado);
Assert.StartsWith("ini", resultado);
Assert.EndsWith("fim", resultado);
Assert.Contains("meio", resultado);

// Coleções
Assert.Equal(new[] { 1, 2, 3 }, lista);
Assert.Contains(item, lista);
Assert.DoesNotContain(item, lista);
Assert.Empty(lista);
Assert.NotEmpty(lista);
```

### Tipos e Null

```csharp
Assert.Null(objeto);
Assert.NotNull(objeto);
Assert.IsType<Cliente>(objeto);
Assert.IsAssignableFrom<ICliente>(objeto);
```

### Booleanos

```csharp
Assert.True(condicao);
Assert.False(condicao);
```

### Exceções

```csharp
// Verifica se lança exceção
Assert.Throws<ArgumentNullException>(() => metodo(null));

// Com verificação de mensagem
var ex = Assert.Throws<ValidationException>(() => metodo(valor));
Assert.Equal("Mensagem esperada", ex.Message);
```

## 🎯 Exercício Completo

### Sistema de Validação

```csharp
// Código a ser testado
public class ValidadorEmail {
    public bool EmailEhValido(string email) {
        if (string.IsNullOrEmpty(email)) return false;
        if (!email.Contains("@")) return false;
        if (email.StartsWith("@") || email.EndsWith("@")) return false;
        
        var partes = email.Split('@');
        if (partes.Length != 2) return false;
        if (string.IsNullOrEmpty(partes[0]) || string.IsNullOrEmpty(partes[1])) return false;
        
        return true;
    }
}

// Testes
public class ValidadorEmailTests {
    private readonly ValidadorEmail _validador;

    public ValidadorEmailTests() {
        _validador = new ValidadorEmail();
    }

    [Fact]
    public void EmailEhValido_EmailValido_RetornaTrue() {
        var resultado = _validador.EmailEhValido("usuario@dominio.com");
        Assert.True(resultado);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void EmailEhValido_EmailVazioOuNulo_RetornaFalse(string email) {
        var resultado = _validador.EmailEhValido(email);
        Assert.False(resultado);
    }

    [Theory]
    [InlineData("semArroba")]
    [InlineData("@inicioCom")]
    [InlineData("terminaCom@")]
    [InlineData("dois@@arrobas")]
    [InlineData("@")]
    public void EmailEhValido_EmailInvalido_RetornaFalse(string email) {
        var resultado = _validador.EmailEhValido(email);
        Assert.False(resultado);
    }
}
```

## 🏃 Executando Testes

```bash
# Todos os testes
dotnet test

# Com detalhes
dotnet test --logger "console;verbosity=detailed"

# Teste específico
dotnet test --filter "FullyQualifiedName~Somar"

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Modo watch (reexecuta ao salvar)
dotnet watch test
```

## ✅ Verificação

- [ ] Criou projeto de testes com xUnit
- [ ] Escreveu pelo menos 5 testes com [Fact]
- [ ] Usou [Theory] com [InlineData]
- [ ] Testou casos válidos e inválidos
- [ ] Verificou exceções com Assert.Throws
- [ ] Todos os testes passando

---

[← Voltar: Introdução aos Testes](./00-introducao-testes.md) | [Próximo: Mocks e Stubs →](./02-mocks-stubs.md)