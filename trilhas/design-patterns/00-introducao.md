# 00 - Introdução aos Design Patterns

## 📘 Nível Básico

### O que são Design Patterns?

**Design Patterns** (Padrões de Projeto) são soluções **reutilizáveis** para problemas **recorrentes** no desenho de software. Eles não são código pronto para copiar e colar, e sim **modelos de solução** que você adapta ao seu contexto (linguagem, framework e domínio).

### Por que usar?

- **Linguagem comum**: "Usamos Repository e Unit of Work" comunica rapidamente a intenção da arquitetura
- **Menos erros**: Soluções já validadas pela comunidade
- **Manutenção**: Código que segue padrões conhecidos é mais fácil de entender e evoluir
- **Alinhamento com SOLID**: Boa parte dos padrões GoF reforça princípios como OCP e DIP

### Gang of Four (GoF)

O termo vem do livro **"Design Patterns: Elements of Reusable Object-Oriented Software"** (1994), de Erich Gamma, Richard Helm, Ralph Johnson e John Vlissides — o "Gang of Four". Eles catalogaram 23 padrões em três categorias:

| Categoria      | Foco                         | Exemplos                    |
|----------------|------------------------------|-----------------------------|
| **Criacionais** | Criação de objetos           | Singleton, Factory, Builder |
| **Estruturais** | Composição de classes/objetos| Adapter, Decorator, Facade  |
| **Comportamentais** | Comportamento e responsabilidades | Strategy, Observer, Command |

### Padrões fora do GoF (mas muito usados em .NET)

- **Repository**: Abstrai o acesso a dados como uma “coleção em memória”
- **Unit of Work**: Agrupa várias operações de persistência em uma única transação
- **Result / Result Pattern**: Representa sucesso ou falha sem usar exceções (usado no microsserviço de tarefas)

## 📗 Nível Intermediário

### Quando NÃO usar um padrão

- **Problema simples**: Não introduza Factory ou Strategy se um `if` ou um método direto resolver
- **Só “porque é best practice”**: O padrão deve resolver um problema real (complexidade de criação, variação de comportamento, desacoplamento de infraestrutura etc.)
- **Over-engineering**: Evite várias camadas e abstrações se o domínio ainda é pequeno

### Relação com SOLID

Muitos padrões ajudam a respeitar SOLID:

- **Strategy** → Open/Closed (novas estratégias sem alterar o código que usa)
- **Dependency Injection + interfaces** → Dependency Inversion
- **Repository** → Separa domínio da persistência (SRP, DIP)

## 📕 Nível Avançado

### Outras catalogações

Além do GoF, existem:

- **Padrões de arquitetura**: Camadas, Clean Architecture, CQRS
- **Padrões de integração**: Message Bus, Event-Driven
- **Padrões de domínio**: Aggregates, Domain Events (frequentemente vistos junto com DDD)

Nesta trilha o foco são os padrões clássicos GoF e os padrões de persistência (Repository, Unit of Work) usados nos projetos do repositório.

---

**Próximo**: [01 - Padrões Criacionais](./01-padroes-criacionais.md)
