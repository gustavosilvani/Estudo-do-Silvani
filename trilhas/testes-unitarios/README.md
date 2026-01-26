# 🧪 Testes Unitários - Qualidade Automatizada

## 📖 Sobre

Esta trilha ensina testes unitários automatizados, fundamentais para desenvolvimento de software de qualidade. Aprenda TDD, mocks, e boas práticas de testabilidade.

## 🎯 Objetivos de Aprendizado

Ao final desta trilha, você será capaz de:

- Escrever testes unitários eficazes com xUnit
- Aplicar Test-Driven Development (TDD)
- Usar mocks e stubs com Moq
- Criar código testável desde o início
- Implementar testes de integração
- Medir e melhorar cobertura de testes

## 📋 Pré-requisitos

- C#/.NET básico
- Clean Code (princípios básicos)
- SOLID (conceitos fundamentais)

## 📚 Módulos

### 📘 Fundamentos
1. **[00 - Introdução aos Testes](./00-introducao-testes.md)** ✅ - Por que testar?
2. **[01 - xUnit e Primeiros Testes](./01-xunit-primeiros-testes.md)** ✅ - Framework básico
3. **[02 - Mocks e Stubs](./02-mocks-stubs.md)** ✅ - Isolamento de dependências

### 📗 Técnicas Avançadas
4. **[03 - TDD - Test-Driven Development](./03-tdd.md)** ✅ - Desenvolvimento orientado por testes
5. **[04 - Testes de Integração](./04-testes-integracao.md)** ✅ - Testando componentes
6. **[05 - Cobertura e Métricas](./05-cobertura-metricas.md)** ✅ - Qualidade dos testes

## 🎓 Progressão Sugerida

**Tempo estimado**: 15-18 horas

## ✅ Checklist de Progresso

- [x] Módulo 00 - Introdução aos Testes
- [x] Módulo 01 - xUnit e Primeiros Testes
- [x] Módulo 02 - Mocks e Stubs
- [x] Módulo 03 - TDD
- [x] Módulo 04 - Testes de Integração
- [x] Módulo 05 - Cobertura e Métricas

**Status: ✅ Trilha Completa (6/6 módulos)**

## 🎯 Por que Testes Unitários?

### Benefícios

- **🐛 Menos Bugs**: Detecta problemas cedo
- **🔄 Refatoração Segura**: Mudanças sem medo
- **📚 Documentação Viva**: Código auto-documentado
- **🚀 Confiança**: Deploy com segurança
- **👥 Colaboração**: Interfaces claras entre módulos

### Cenário Real

**Sem Testes:**
```csharp
// Mudança simples quebra tudo
public void CalcularDesconto(decimal valor) {
    return valor * 0.9m; // Quebrou sistema de frete
}
```

**Com Testes:**
```csharp
[Test]
public void CalcularDesconto_Valor100_Retorna90() {
    var calc = new CalculadoraDesconto();
    var resultado = calc.Calcular(100m);
    Assert.Equal(90m, resultado);
}
```

## 🛠️ Ambiente de Testes

### Frameworks Essenciais

```xml
<!-- .csproj -->
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.9.0" />
<PackageReference Include="xunit" Version="2.7.0" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.7" />
<PackageReference Include="Moq" Version="4.20.70" />
<PackageReference Include="coverlet.collector" Version="6.0.0" />
```

### Estrutura de Projeto

```
MeuProjeto/
├── src/
│   └── MeuProjeto.csproj
├── tests/
│   ├── MeuProjeto.Tests.csproj
│   └── UnitTests/
│       └── CalculadoraTests.cs
```

### Executando Testes

```bash
# Executar todos os testes
dotnet test

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Teste específico
dotnet test --filter "NomeDoTeste"

# Modo watch
dotnet watch test
```

## 📊 Pirâmide de Testes

```
     /\
    /  \
   / E2E \
  /--------\
 / Integração \
/--------------\
/   Unitários   \
-----------------
```

- **Unitários (80%)**: Rápidos, isolados, muitos
- **Integração (15%)**: Testam colaboração entre componentes
- **E2E (5%)**: Testam fluxo completo, lentos

## 🧪 Conceitos Fundamentais

### Teste Unitário
- Testa **uma unidade** de código (método/classe)
- **Isolado** - não depende de externos
- **Rápido** - executa em milissegundos
- **Repetível** - mesmo resultado sempre
- **Independente** - ordem não importa

### AAA Pattern
```csharp
[Test]
public void Metodo_Situacao_ResultadoEsperado() {
    // Arrange - Preparar
    var sut = new SystemUnderTest();

    // Act - Executar
    var result = sut.Method();

    // Assert - Verificar
    Assert.Equal(expected, result);
}
```

### Mocks vs Stubs
- **Stub**: Fornece dados/controla indireto (estado)
- **Mock**: Verifica interações/comportamento (comportamento)

## 🎯 Boas Práticas

### Nomenclatura
```csharp
// ✅ BOM
[Fact]
public void CalcularTotal_ItensValidos_RetornaSomaCorreta() {
    // ...
}

[Theory]
[InlineData(10, 20, 30)]
[InlineData(0, 0, 0)]
public void Somar_DoisNumeros_RetornaSomaCorreta(int a, int b, int esperado) {
    // ...
}
```

### Estrutura de Teste
```csharp
public class CalculadoraTests : IDisposable {
    private Calculadora _sut;

    public CalculadoraTests() {
        _sut = new Calculadora();
    }

    [Fact]
    public void Somar_DoisNumerosPositivos_RetornaSomaCorreta() {
        // Arrange
        const int a = 5;
        const int b = 3;

        // Act
        var resultado = _sut.Somar(a, b);

        // Assert
        Assert.Equal(8, resultado);
    }

    public void Dispose() {
        // Cleanup
    }
}
```

## 📈 Cobertura de Código

### Métricas Importantes

- **Linhas**: Porcentagem de linhas executadas
- **Branches**: Caminhos condicionais testados
- **Métodos**: Funções chamadas
- **Classes**: Tipos instanciados

### Ferramentas

```xml
<!-- Para cobertura -->
<PackageReference Include="coverlet.msbuild" Version="6.0.0" />
```

```bash
# Relatório de cobertura
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
```

## 🚨 Sinais de Testes Ruins

### ❌ Testes Frágeis
```csharp
[Test]
public void ProcessarPedido_ApiExternaFunciona_EnviaEmail() {
    // Depende de API externa - quebrou quando API caiu
}
```

### ❌ Testes Lentos
```csharp
[Test]
public void SalvarDados_BancoReal_Demoram5Segundos() {
    // Teste lento - CI fica lento
}
```

### ❌ Testes Complexos
```csharp
[Test]
public void MetodoComplexo_FazTudo_EsperaTudo() {
    // 50 linhas de setup - difícil manter
}
```

## ✅ Testes de Qualidade

### FIRST Principles
- **F**ast: Executam rápido
- **I**solated: Independentes
- **R**epeatable: Resultado consistente
- **S**elf-validating: Passam/falham automaticamente
- **T**imely: Escritos antes ou junto com código

### Características
- **Legíveis**: Qualquer dev entende
- **Manuteníveis**: Fáceis de atualizar
- **Confiáveis**: Não falham aleatoriamente
- **Rápidos**: Executam em segundos

## 📚 Recursos

### Documentação Oficial
- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq)
- [Testing .NET Apps](https://learn.microsoft.com/en-us/dotnet/core/testing/)

### Cursos
- [Testing .NET Core Applications](https://app.pluralsight.com/library/courses/dotnet-core-testing-applications/)
- [Unit Testing in C#](https://www.udemy.com/course/unit-testing-in-c/)

## 🔗 Integração com Outras Trilhas

Testes são essenciais para:
- **C#/.NET**: Testes garantem qualidade do código
- **Clean Code**: Código testável é código limpo
- **SOLID**: Princípios facilitam testabilidade
- **DDD**: Domínio deve ser testável
- **SQL**: Persistência precisa de testes
- **Projeto Final**: Qualidade através de testes

## 🎯 Dicas para Iniciantes

### Comece Pequeno
- Teste métodos simples primeiro
- Foque em lógica de negócio
- Evite testar frameworks/bibliotecas

### Use TDD
- Escreva teste ANTES do código
- Teste deve falhar primeiro (Red)
- Implemente mínimo para passar (Green)
- Refatore mantendo testes (Refactor)

### Foque na Qualidade
- Cobertura não é tudo (>80% é bom)
- Testes devem ser manutenção baixa
- Testes documentam comportamento esperado

---

**🧪 "Código sem testes é código quebrado esperando para acontecer."**

[← Voltar ao índice principal](../../INDEX.md)