# 05 - Cobertura e Métricas

## 📖 O que é Cobertura de Código?

Cobertura de código mede **quanto do seu código é executado pelos testes**. É uma métrica importante, mas não é o único indicador de qualidade.

## 🎯 Por que Medir Cobertura?

### Benefícios

- **Identifica código não testado**: Descobre áreas sem testes
- **Guia refatoração**: Mostra onde adicionar testes
- **Confiança**: Maior cobertura = maior confiança
- **Métricas objetivas**: Números claros sobre qualidade

### Limitações

- **Não mede qualidade**: 100% de cobertura não garante bons testes
- **Pode enganar**: Testes ruins podem ter alta cobertura
- **Não é meta final**: Foco deve ser em testes úteis

## 📊 Métricas Importantes

### 1. Cobertura de Linhas
Porcentagem de linhas de código executadas.

```csharp
// Código
public int Calcular(int a, int b) {
    if (a > 0) {          // Linha 1 - executada ✅
        return a + b;    // Linha 2 - executada ✅
    }
    return 0;            // Linha 3 - NÃO executada ❌
}

// Teste
[Fact]
public void Calcular_Positivo_RetornaSoma() {
    var resultado = Calcular(5, 3);
    Assert.Equal(8, resultado);
}

// Cobertura: 2/3 linhas = 66.7%
```

### 2. Cobertura de Branches
Porcentagem de caminhos condicionais testados.

```csharp
// Código
public string Classificar(int idade) {
    if (idade < 18) {        // Branch 1 - testado ✅
        return "Menor";
    } else if (idade < 65) { // Branch 2 - testado ✅
        return "Adulto";
    } else {                 // Branch 3 - NÃO testado ❌
        return "Idoso";
    }
}

// Cobertura de branches: 2/3 = 66.7%
```

### 3. Cobertura de Métodos
Porcentagem de métodos chamados pelos testes.

```csharp
public class Calculadora {
    public int Somar(int a, int b) {      // Testado ✅
        return a + b;
    }
    
    public int Subtrair(int a, int b) {   // NÃO testado ❌
        return a - b;
    }
}

// Cobertura de métodos: 1/2 = 50%
```

### 4. Cobertura de Classes
Porcentagem de classes instanciadas.

## 🔧 Ferramentas

### Coverlet (Recomendado)

```xml
<!-- .csproj -->
<ItemGroup>
  <PackageReference Include="coverlet.collector" Version="6.0.2">
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
</ItemGroup>
```

### ReportGenerator (Relatórios HTML)

```xml
<PackageReference Include="ReportGenerator" Version="5.2.0" />
```

## 📈 Como Medir Cobertura

### Comando Básico

```bash
# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Resultado salvo em: TestResults/[guid]/coverage.cobertura.xml
```

### Com Relatório HTML

```bash
# 1. Executar testes
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage

# 2. Gerar relatório HTML
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"./coverage/**/coverage.cobertura.xml" -targetdir:"./coverage/report" -reporttypes:Html
```

### Configuração no .csproj

```xml
<PropertyGroup>
  <CollectCoverage>true</CollectCoverage>
  <CoverletOutputFormat>cobertura</CoverletOutputFormat>
  <CoverletOutput>./coverage/</CoverletOutput>
</PropertyGroup>
```

## 📊 Interpretando Relatórios

### Relatório HTML

```
Cobertura Total: 78.5%
├── Linhas: 82.3%
├── Branches: 71.2%
├── Métodos: 85.0%
└── Classes: 90.0%
```

### Análise

- **>80%**: Excelente ✅
- **60-80%**: Bom ✅
- **40-60%**: Aceitável ⚠️
- **<40%**: Precisa melhorar ❌

## 🎯 Metas Recomendadas

### Por Tipo de Código

| Tipo | Meta Mínima | Meta Ideal |
|------|-------------|------------|
| **Lógica de Negócio** | 90% | 95%+ |
| **Services** | 80% | 90%+ |
| **Controllers** | 70% | 80%+ |
| **Infrastructure** | 60% | 70%+ |
| **DTOs/Models** | 0% | 0% (não precisa) |

### Regra Geral

**80%+ é uma boa meta**, mas:
- Foque em **código crítico** primeiro
- **Qualidade > Quantidade**
- Não force 100% (pode ser contraproducente)

## 💡 Melhorando Cobertura

### Identificar Gaps

```bash
# Ver relatório HTML
# Identifique arquivos com baixa cobertura
# Foque nos mais críticos primeiro
```

### Exemplo: Aumentar Cobertura

```csharp
// ❌ ANTES - Código não testado
public class ValidadorEmail {
    public bool Validar(string email) {
        if (string.IsNullOrEmpty(email)) {
            return false;  // Não testado
        }
        
        if (!email.Contains("@")) {
            return false;  // Não testado
        }
        
        return true;  // Testado
    }
}

// Cobertura: 33% (apenas 1/3 caminhos)

// ✅ DEPOIS - Adicionar testes
[Theory]
[InlineData(null, false)]
[InlineData("", false)]
[InlineData("email", false)]
[InlineData("email@", false)]
[InlineData("@dominio.com", false)]
[InlineData("email@dominio.com", true)]
public void Validar_VariosEmails_RetornaResultadoEsperado(string email, bool esperado) {
    var validador = new ValidadorEmail();
    var resultado = validador.Validar(email);
    Assert.Equal(esperado, resultado);
}

// Cobertura: 100% (todos caminhos testados)
```

## ⚠️ Cobertura vs Qualidade

### ❌ Alta Cobertura, Baixa Qualidade

```csharp
// Teste que executa código mas não valida nada
[Fact]
public void Processar_QualquerCoisa_Passa() {
    var service = new Service();
    service.Processar();  // Executa, mas não valida resultado
    Assert.True(true);    // Sempre passa
}

// Cobertura: 100% ✅
// Qualidade: 0% ❌
```

### ✅ Boa Cobertura, Alta Qualidade

```csharp
// Teste que valida comportamento
[Fact]
public void Processar_DadosValidos_ProcessaCorretamente() {
    // Arrange
    var service = new Service();
    var dados = new DadosValidos();
    
    // Act
    var resultado = service.Processar(dados);
    
    // Assert
    Assert.NotNull(resultado);
    Assert.Equal(Estado.Processado, resultado.Estado);
    Assert.True(resultado.DataProcessamento > DateTime.MinValue);
}

// Cobertura: 100% ✅
// Qualidade: Alta ✅
```

## 🔍 Análise de Cobertura

### O que Excluir?

```xml
<!-- Excluir da cobertura -->
[ExcludeFromCodeCoverage]
public class Dto {
    public string Propriedade { get; set; }
}

// Ou no .csproj
<ItemGroup>
  <ExcludeFromCoverage Include="**/DTOs/**" />
  <ExcludeFromCoverage Include="**/Models/**" />
</ItemGroup>
```

### O que Incluir?

- ✅ Lógica de negócio
- ✅ Validações
- ✅ Transformações de dados
- ✅ Regras complexas

### O que Pode Excluir?

- ❌ DTOs simples (apenas propriedades)
- ❌ Mapeamentos automáticos
- ❌ Configurações
- ❌ Código gerado

## 🚀 Integração com CI/CD

### GitHub Actions

```yaml
name: Testes e Cobertura

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '9.0.x'
      
      - name: Restore dependencies
        run: dotnet restore
      
      - name: Build
        run: dotnet build --no-restore
      
      - name: Test with coverage
        run: dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
      
      - name: Generate report
        run: |
          dotnet tool install -g dotnet-reportgenerator-globaltool
          reportgenerator -reports:"./coverage/**/coverage.cobertura.xml" -targetdir:"./coverage/report" -reporttypes:Html
      
      - name: Upload coverage
        uses: codecov/codecov-action@v3
        with:
          files: ./coverage/**/coverage.cobertura.xml
```

### Azure DevOps

```yaml
- task: DotNetCoreCLI@2
  displayName: 'Test with Coverage'
  inputs:
    command: 'test'
    arguments: '--collect:"XPlat Code Coverage"'

- task: PublishCodeCoverageResults@1
  inputs:
    codeCoverageTool: 'Cobertura'
    summaryFileLocation: '$(Agent.TempDirectory)/**/coverage.cobertura.xml'
```

## 📊 Exemplo Prático Completo

### Projeto com Cobertura

```bash
# Estrutura
MeuProjeto/
├── src/
│   └── MeuProjeto.csproj
└── tests/
    └── MeuProjeto.Tests.csproj
```

### Configuração

```xml
<!-- tests/MeuProjeto.Tests.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.9.0" />
    <PackageReference Include="xunit" Version="2.7.0" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.7" />
    <PackageReference Include="coverlet.collector" Version="6.0.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\src\MeuProjeto.csproj" />
  </ItemGroup>
</Project>
```

### Executar e Ver Relatório

```bash
# 1. Executar testes
dotnet test --collect:"XPlat Code Coverage"

# 2. Gerar relatório HTML
reportgenerator -reports:"./TestResults/**/coverage.cobertura.xml" -targetdir:"./coverage/report" -reporttypes:Html

# 3. Abrir relatório
start ./coverage/report/index.html
```

## ✅ Verificação

- [ ] Entendeu o que é cobertura de código
- [ ] Conhece métricas importantes (linhas, branches, métodos)
- [ ] Configurou coverlet no projeto
- [ ] Executou testes com cobertura
- [ ] Gerou relatório HTML
- [ ] Interpretou resultados
- [ ] Identificou gaps de cobertura
- [ ] Melhorou cobertura de código crítico
- [ ] Entendeu diferença entre cobertura e qualidade
- [ ] Configurou cobertura no CI/CD

---

**🎉 Parabéns! Você completou a trilha de Testes Unitários!**

[← Voltar: Testes de Integração](./04-testes-integracao.md) | [Voltar à Trilha](./README.md)