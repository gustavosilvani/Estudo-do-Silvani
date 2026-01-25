# 🎯 C#/.NET - Linguagem e Framework

## 📖 Sobre

Esta trilha ensina C# e .NET desde o básico até construção de APIs RESTful profissionais. Aprenda a linguagem mais usada no ecossistema Microsoft e construa aplicações modernas.

## 🎯 Objetivos de Aprendizado

Ao final desta trilha, você será capaz de:

- Dominar sintaxe e recursos da linguagem C#
- Construir aplicações console e web com .NET
- Criar APIs RESTful com ASP.NET Core
- Implementar injeção de dependência
- Trabalhar com programação assíncrona
- Construir aplicações escaláveis e manuteníveis

## 📋 Pré-requisitos

- Conhecimento básico de programação
- Git (da trilha anterior)
- Editor de código (VS Code recomendado)
- .NET 8.0+ instalado

## 📚 Módulos

### 📘 Nível Básico
1. **[00 - Introdução ao C#](./00-introducao.md)** - Sintaxe básica, tipos, variáveis
2. **[01 - Orientação a Objetos](./01-orientacao-objetos.md)** - Classes, herança, polimorfismo
3. **[02 - Collections e LINQ](./02-collections-linq.md)** - Listas, dicionários, consultas

### 📗 Nível Intermediário
4. **[03 - Programação Assíncrona](./03-programacao-assincrona.md)** - async/await, Task, Threading
5. **[04 - ASP.NET Core Básico](./04-aspnet-core-basico.md)** - MVC, Razor, middleware
6. **[05 - APIs RESTful](./05-apis-restful.md)** - HTTP, JSON, OpenAPI/Swagger

### 📕 Nível Avançado
7. **[06 - Dependency Injection](./06-dependency-injection.md)** - IoC, containers, lifetime
8. **[07 - Testes de Integração](./07-testes-integracao.md)** - WebApplicationFactory, TestServer

## 🎓 Progressão Sugerida

Siga os módulos em ordem. Comece com conceitos fundamentais e progrida para aplicações web completas.

**Tempo estimado**: 20-25 horas

## ✅ Checklist de Progresso

- [ ] Módulo 00 - Introdução ao C#
- [ ] Módulo 01 - Orientação a Objetos
- [ ] Módulo 02 - Collections e LINQ
- [ ] Módulo 03 - Programação Assíncrona
- [ ] Módulo 04 - ASP.NET Core Básico
- [ ] Módulo 05 - APIs RESTful
- [ ] Módulo 06 - Dependency Injection
- [ ] Módulo 07 - Testes de Integração

## 🛠️ Ambiente de Desenvolvimento

### Instalação do .NET

**Windows:**
```bash
# Via Chocolatey
choco install dotnet

# Ou baixar do site oficial
# https://dotnet.microsoft.com/download
```

**macOS:**
```bash
# Via Homebrew
brew install dotnet

# Ou instalar SDK diretamente
```

**Linux:**
```bash
# Ubuntu/Debian
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
```

### Verificar Instalação

```bash
# Verificar versão
dotnet --version

# Deve mostrar algo como: 8.0.100

# Verificar SDKs disponíveis
dotnet --list-sdks

# Verificar runtimes disponíveis
dotnet --list-runtimes
```

### IDE Recomendada

**Visual Studio Code** (Recomendado para aprendizes):
- Instale as extensões: C#, C# Dev Kit, .NET Install Tool
- Suporte nativo para .NET

**Visual Studio 2022** (Alternativa completa):
- IDE completa da Microsoft
- Melhor para desenvolvimento .NET avançado

## 📝 Estrutura de Projetos

### Aplicação Console
```
MeuConsoleApp/
├── MeuConsoleApp.csproj
├── Program.cs
└── Models/
    └── Pessoa.cs
```

### API Web ASP.NET Core
```
MinhaApi/
├── MinhaApi.csproj
├── Program.cs
├── Controllers/
│   └── PessoasController.cs
├── Models/
│   └── Pessoa.cs
└── Services/
    └── PessoaService.cs
```

### Criando Novos Projetos

```bash
# Aplicação console
dotnet new console -n MinhaConsoleApp

# API Web
dotnet new webapi -n MinhaApi

# Biblioteca de classes
dotnet new classlib -n MinhaBiblioteca

# Testes unitários
dotnet new xunit -n MeusTestes
```

## 🚀 Executando Projetos

```bash
# Navegar para pasta do projeto
cd MeuProjeto

# Restaurar dependências
dotnet restore

# Compilar
dotnet build

# Executar
dotnet run

# Executar em modo watch (recompila automaticamente)
dotnet watch run
```

## 📚 Conceitos Fundamentais

### Linguagem C#
- **Fortemente tipada**: Todos os tipos são verificados em tempo de compilação
- **Orientada a objetos**: Tudo é objeto (até tipos primitivos)
- **Gerenciada**: Garbage collector automático
- **Multi-paradigma**: OOP, funcional, procedural

### Framework .NET
- **Cross-platform**: Windows, Linux, macOS
- **Open source**: Código disponível no GitHub
- **Alta performance**: JIT compilation, AOT opcional
- **Ecossistema rico**: NuGet packages, ferramentas

### ASP.NET Core
- **Framework web moderno**: Construído do zero para nuvem
- **Alta performance**: Mais rápido que Node.js, Go
- **Flexível**: Microserviços, monólitos, serverless
- **Cross-platform**: Mesmo código roda em qualquer OS

## 🎯 Boas Práticas

### Nomenclatura
- **Classes**: PascalCase (`Pessoa`, `ClienteService`)
- **Métodos**: PascalCase (`CalcularTotal()`, `BuscarPorId()`)
- **Propriedades**: PascalCase (`Nome`, `DataNascimento`)
- **Variáveis**: camelCase (`nomeCliente`, `totalPedido`)
- **Constantes**: SCREAMING_SNAKE_CASE (`MAX_ITENS`)

### Estrutura de Código
- Use namespaces organizados
- Mantenha classes pequenas (máximo 200 linhas)
- Métodos pequenos (máximo 20 linhas)
- Uma responsabilidade por classe/método

### Tratamento de Erros
- Use exceptions apropriadas
- Evite try-catch genérico
- Valide entrada de dados
- Log de erros importantes

## 🔧 Ferramentas Essenciais

### NuGet Packages Importantes
- **Microsoft.Extensions.DependencyInjection**: DI container
- **Microsoft.Extensions.Logging**: Sistema de logging
- **Microsoft.EntityFrameworkCore**: ORM para bancos
- **Swashbuckle.AspNetCore**: Documentação OpenAPI

### Extensões VS Code
- C# (Microsoft)
- C# Dev Kit (Microsoft)
- NuGet Package Manager
- .NET Install Tool

## 📊 Debugging e Profiling

### Debugging Básico
```csharp
// Breakpoints
Console.WriteLine("Antes do breakpoint");

// Conditional breakpoints
if (cliente.Idade > 18) {
    Console.WriteLine("Maior de idade");
}

// Watch expressions
var total = itens.Sum(i => i.Preco);
```

### Logging
```csharp
using Microsoft.Extensions.Logging;

public class MeuService {
    private readonly ILogger<MeuService> _logger;

    public MeuService(ILogger<MeuService> logger) {
        _logger = logger;
    }

    public void FazerAlgo() {
        _logger.LogInformation("Iniciando operação");
        try {
            // código...
            _logger.LogInformation("Operação concluída");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Erro na operação");
        }
    }
}
```

## 🎓 Exemplos de Carreira

### Júnior (0-2 anos)
- Aplicações console simples
- CRUD básico com Entity Framework
- APIs RESTful simples

### Pleno (2-5 anos)
- Arquiteturas em camadas
- Microsserviços
- Integração com APIs externas
- Testes automatizados

### Sênior (5+ anos)
- Arquiteturas complexas
- Performance e escalabilidade
- Design patterns avançados
- Mentoria e code review

## 📚 Recursos Adicionais

### Documentação Oficial
- [Documentação .NET](https://learn.microsoft.com/pt-br/dotnet/)
- [C# Guide](https://learn.microsoft.com/pt-br/dotnet/csharp/)
- [ASP.NET Core Docs](https://learn.microsoft.com/pt-br/aspnet/core/)

### Cursos Online
- [C# para Iniciantes - Microsoft Learn](https://learn.microsoft.com/pt-br/training/paths/get-started-c-sharp-part-1/)
- [ASP.NET Core - Pluralsight](https://www.pluralsight.com/courses/aspdotnet-core-fundamentals)

### Comunidades
- [Stack Overflow - C#](https://stackoverflow.com/questions/tagged/c%23)
- [Reddit - r/csharp](https://www.reddit.com/r/csharp/)
- [Discord .NET](https://discord.gg/dotnet)

## 🔗 Integração com Outras Trilhas

C#/.NET é base para todas as outras trilhas:

- **Clean Code**: Aplicação prática dos princípios
- **Testes**: Framework xUnit, mocks com Moq
- **SOLID**: Padrões aplicados em C#
- **DDD**: Implementação em C# com EF Core
- **SQL Server**: Entity Framework Core
- **Projeto Final**: Tecnologia principal

## 🚨 Dicas para Aprendizes

### Comece Pequeno
- Não tente aprender tudo de uma vez
- Foque em aplicações console primeiro
- Construa projetos incrementais

### Pratique Diariamente
- Dedique 1-2 horas por dia
- Construa projetos pessoais
- Contribua para open source

### Aprenda com Erros
- Debugging é fundamental
- Leia mensagens de erro atentamente
- Use breakpoints para entender fluxo

### Construa Portfólio
- GitHub é seu currículo
- Projetos bem documentados
- README detalhados

---

**🚀 Pronto para começar sua jornada em C#/.NET? Vamos construir aplicações incríveis!**

[← Voltar ao índice principal](../../INDEX.md)