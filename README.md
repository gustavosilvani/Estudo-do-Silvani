# 🎓 Escola de Estudos - Desenvolvimento de Software

Bem-vindo à Escola de Estudos! Este repositório contém trilhas de aprendizado completas e progressivas sobre conceitos fundamentais de desenvolvimento de software, organizadas para uso em PDI (Plano de Desenvolvimento Individual).

## 📚 Sobre

Esta escola foi criada para fornecer um caminho estruturado de aprendizado, do básico ao avançado, sobre os principais tópicos de engenharia de software. Cada trilha é independente, mas complementar, permitindo que você construa conhecimento de forma progressiva.

Todas as trilhas usam **C#** como linguagem de exemplo, seguindo as melhores práticas de Clean Code e SOLID.

## 🗺️ Trilhas Disponíveis

### ✅ Completas (100%)

#### **[Git](./trilhas/git/README.md)** - Controle de Versão
- ✅ Introdução e conceitos básicos
- ✅ Comandos essenciais (init, add, commit, push, pull)
- ✅ Branches e merges
- ✅ Colaboração e trabalho em equipe
- ✅ GitFlow e estratégias de branching

#### **[C#/.NET](./trilhas/csharp-net/README.md)** - Fundamentos da Linguagem
- ✅ Introdução ao C# e .NET
- ✅ Orientação a objetos em C#
- ✅ Collections e LINQ
- ✅ Programação assíncrona (async/await)
- ✅ ASP.NET Core básico
- ✅ APIs RESTful
- ✅ Dependency Injection
- ✅ Testes de integração

#### **[Clean Code](./trilhas/clean-code/README.md)** - Código Limpo e Boas Práticas
- ✅ Introdução ao Clean Code
- ✅ Nomes significativos
- ✅ Funções pequenas e coesas
- ✅ Comentários úteis vs desnecessários
- ✅ Formatação e organização
- ✅ Objetos e estruturas de dados
- ✅ Tratamento de erros

#### **[Testes Unitários](./trilhas/testes-unitarios/README.md)** - TDD e Testes Automatizados
- ✅ Introdução aos testes
- ✅ xUnit e primeiros testes
- ✅ Mocks e Stubs com Moq
- ✅ TDD (Test-Driven Development)
- ✅ Testes de integração
- ✅ Cobertura e métricas

#### **[SOLID](./trilhas/solid/README.md)** - Princípios de Design OO
- ✅ Introdução aos princípios SOLID
- ✅ Single Responsibility Principle (SRP)
- ✅ Open/Closed Principle (OCP)
- ✅ Liskov Substitution Principle (LSP)
- ✅ Interface Segregation Principle (ISP)
- ✅ Dependency Inversion Principle (DIP)
- ✅ Aplicação prática completa
- ✅ Anti-padrões e como evitá-los
- ✅ Exercícios práticos
- ✅ **Microsserviço de exemplo** (Logística)

#### **[DDD](./trilhas/ddd/README.md)** - Domain-Driven Design
- ✅ Introdução ao DDD
- ✅ Linguagem Ubíqua
- ✅ Entidades e Value Objects
- ✅ Aggregates e Repositories
- ✅ Domain Services
- ✅ Bounded Contexts
- ✅ CQRS e Eventos
- ✅ Aplicação Prática

#### **[SQL Server](./trilhas/sql-server/README.md)** - Banco de Dados Relacional
- ✅ Introdução ao SQL
- ✅ Consultas Básicas (SELECT, INSERT, UPDATE, DELETE)
- ✅ Relacionamentos (JOINs, Foreign Keys)
- ✅ Entity Framework Core
- ✅ Migrations e Versionamento
- ✅ Otimização e Performance

### 🔵 Estruturadas (Aguardando Conteúdo)

- **[Projeto Final](./trilhas/projeto-final/README.md)** - Integração de Conceitos

## 🚀 Projetos de Exemplo

### **Microsserviço de Logística** (SOLID + Testes)
Localização: `solucao-microsservico-logistica/`
- Aplicação completa dos princípios SOLID
- Testes unitários com xUnit e Moq
- Padrões Strategy, Repository, Factory
- Unit of Work Pattern
- Dependency Injection

### **Microsserviço de Tarefas** (HATEOAS + Result Pattern)
Localização: `microsservico-tarefas/`
- **Richardson Maturity Model - Nível 3 (HATEOAS)**
- Result Pattern (sem exceções)
- Unit of Work Pattern
- Domain-Driven Design (DDD)
- Clean Architecture
- API RESTful com links dinâmicos
- Testes unitários completos

## 🚀 Como Usar

1. **Escolha uma trilha** que se alinha com seus objetivos de aprendizado
2. **Siga a ordem dos módulos** - cada trilha é numerada (00, 01, 02...) para guiar seu progresso
3. **Complete os exercícios** - pratique o que aprendeu em cada módulo
4. **Analise os microsserviços** - veja os conceitos aplicados em projetos reais
5. **Use os recursos** - consulte referências e exemplos extras quando necessário

## 📖 Estrutura de Cada Trilha

Cada trilha segue uma estrutura padronizada:

```
trilha/
├── README.md          # Índice e visão geral da trilha
├── 00-introducao.md   # Introdução aos conceitos
├── 01-*.md            # Módulos numerados sequencialmente
├── exercicios/        # Exercícios práticos
└── recursos/          # Referências e materiais extras
```

## 🎯 Progressão de Aprendizado

Cada módulo é organizado em três níveis:

- **📘 Básico**: Conceitos fundamentais e exemplos simples
- **📗 Intermediário**: Aplicação prática e casos de uso reais
- **📕 Avançado**: Padrões relacionados, integração e otimizações

## 📋 Pré-requisitos

- Conhecimento básico de programação
- Familiaridade com orientação a objetos
- Ambiente .NET 8/9 configurado (para executar exemplos)

## 🗺️ Roadmap Sugerido

Para desenvolvedores iniciando na jornada, recomendamos seguir esta ordem:

1. **Git** ✅ - Fundamentos de controle de versão
2. **C#/.NET** ✅ - Linguagem e framework
3. **Clean Code** ✅ - Fundamentos de código limpo
4. **SOLID** ✅ - Princípios de design
5. **Testes Unitários** ✅ - TDD e testes automatizados
6. **DDD** ✅ - Design orientado a domínio
7. **SQL Server** ✅ - Persistência de dados
8. **Projeto Final** 🔵 - Integração de todos os conceitos

## 📚 Recursos Compartilhados

- [Glossário](./recursos-compartilhados/glossario.md) - Termos técnicos explicados
- [Boas Práticas](./recursos-compartilhados/boas-praticas.md) - Práticas gerais recomendadas
- [Ferramentas](./recursos-compartilhados/ferramentas.md) - Ferramentas úteis para desenvolvimento
- [Índice Completo](./INDEX.md) - Visão detalhada de todas as trilhas

## 🎓 Conceitos Avançados Implementados

Os microsserviços de exemplo demonstram:

- ✅ Clean Code
- ✅ SOLID (todos os 5 princípios)
- ✅ Domain-Driven Design (DDD)
- ✅ Clean Architecture
- ✅ Repository Pattern
- ✅ Unit of Work Pattern
- ✅ Result Pattern
- ✅ Strategy Pattern
- ✅ Factory Pattern
- ✅ Dependency Injection
- ✅ Testes Unitários (xUnit + Moq)
- ✅ APIs RESTful (Nível 3 - HATEOAS)
- ✅ Richardson Maturity Model

## 🤝 Contribuindo

Quer adicionar uma nova trilha ou melhorar o conteúdo existente? Consulte o [guia de contribuição](./CONTRIBUTING.md).

## 📄 Licença

Este material é livre para uso educacional e em PDIs pessoais.

---

**Bons estudos! 🚀**
