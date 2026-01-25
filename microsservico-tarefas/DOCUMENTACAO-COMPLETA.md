# 🚀 Microsserviço: Gerenciamento de Tarefas - IMPLEMENTAÇÃO COMPLETA

## 📋 Visão Geral

Microsserviço completo aplicando **TODOS** os conceitos das trilhas:
- ✅ Clean Code
- ✅ SOLID
- ✅ DDD
- ✅ Testes Unitários
- ✅ APIs RESTful
- ✅ Dependency Injection
- ✅ **Unit of Work Pattern**
- ✅ **Result Pattern**

---

## 🏗️ Arquitetura

```
TaskManagement/
├── TaskManagement.Domain/          # Entidades, Value Objects, Interfaces
├── TaskManagement.Application/     # Use Cases, DTOs, Services
├── TaskManagement.Infrastructure/  # Repositórios, UnitOfWork, EF Core
├── TaskManagement.Api/             # Controllers, Middlewares
└── TaskManagement.Tests/           # Testes Unitários e Integração
```

### **Camadas e Responsabilidades**

| Camada | Responsabilidade | Dependências |
|--------|------------------|--------------|
| **Domain** | Entidades, regras de negócio | Nenhuma |
| **Application** | Casos de uso, orquestração | Domain |
| **Infrastructure** | Persistência, externos | Domain, Application |
| **API** | Endpoints HTTP | Application, Infrastructure |
| **Tests** | Testes automatizados | Domain, Application |

---

## 📦 **1. DOMAIN LAYER**

### **Result Pattern**

```csharp
// TaskManagement.Domain/Common/Result.cs
namespace TaskManagement.Domain.Common;

/// <summary>
/// Pattern para retornar sucesso ou erro sem exceções
/// </summary>
public class Result {
    public bool Success { get; }
    public string Error { get; }
    public bool IsFailure => !Success;
    
    protected Result(bool success, string error) {
        if (success && error != string.Empty) {
            throw new InvalidOperationException();
        }
        
        if (!success && error == string.Empty) {
            throw new InvalidOperationException();
        }
        
        Success = success;
        Error = error;
    }
    
    public static Result Ok() => new Result(true, string.Empty);
    public static Result Fail(string error) => new Result(false, error);
    
    public static Result<T> Ok<T>(T value) => new Result<T>(value, true, string.Empty);
    public static Result<T> Fail<T>(string error) => new Result<T>(default, false, error);
}

public class Result<T> : Result {
    public T Value { get; }
    
    protected internal Result(T value, bool success, string error)
        : base(success, error) {
        Value = value;
    }
}
```

### **Entidades (DDD)**

```csharp
// TaskManagement.Domain/Entities/TarefaTask.cs
using TaskManagement.Domain.Common;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain.Entities;

/// <summary>
/// Entidade Tarefa - Aggregate Root (DDD)
/// Aplica: Encapsulamento, Value Objects, Regras de Negócio
/// </summary>
public class TarefaTask {
    public Guid Id { get; private set; }
    public Titulo Titulo { get; private set; }
    public string Descricao { get; private set; }
    public StatusTarefa Status { get; private set; }
    public Prioridade Prioridade { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public DateTime? DataVencimento { get; private set; }
    public Guid UsuarioId { get; private set; }
    
    private readonly List<Comentario> _comentarios;
    public IReadOnlyList<Comentario> Comentarios => _comentarios.AsReadOnly();
    
    // Construtor privado para EF Core
    private TarefaTask() {
        _comentarios = new List<Comentario>();
    }
    
    // Factory Method (Design Pattern)
    public static Result<TarefaTask> Criar(
        string titulo,
        string descricao,
        Guid usuarioId,
        Prioridade prioridade,
        DateTime? dataVencimento = null) {
        
        // Validações
        var tituloResult = Titulo.Criar(titulo);
        if (tituloResult.IsFailure) {
            return Result.Fail<TarefaTask>(tituloResult.Error);
        }
        
        if (string.IsNullOrWhiteSpace(descricao)) {
            return Result.Fail<TarefaTask>("Descrição é obrigatória");
        }
        
        if (usuarioId == Guid.Empty) {
            return Result.Fail<TarefaTask>("UsuarioId inválido");
        }
        
        if (dataVencimento.HasValue && dataVencimento.Value < DateTime.UtcNow) {
            return Result.Fail<TarefaTask>("Data de vencimento não pode ser no passado");
        }
        
        var tarefa = new TarefaTask {
            Id = Guid.NewGuid(),
            Titulo = tituloResult.Value,
            Descricao = descricao,
            Status = StatusTarefa.Pendente,
            Prioridade = prioridade,
            DataCriacao = DateTime.UtcNow,
            UsuarioId = usuarioId,
            DataVencimento = dataVencimento
        };
        
        return Result.Ok(tarefa);
    }
    
    // Métodos de Domínio (Regras de Negócio)
    public Result Concluir() {
        if (Status == StatusTarefa.Concluida) {
            return Result.Fail("Tarefa já está concluída");
        }
        
        if (Status == StatusTarefa.Cancelada) {
            return Result.Fail("Não é possível concluir tarefa cancelada");
        }
        
        Status = StatusTarefa.Concluida;
        DataConclusao = DateTime.UtcNow;
        
        return Result.Ok();
    }
    
    public Result Cancelar() {
        if (Status == StatusTarefa.Concluida) {
            return Result.Fail("Não é possível cancelar tarefa concluída");
        }
        
        if (Status == StatusTarefa.Cancelada) {
            return Result.Fail("Tarefa já está cancelada");
        }
        
        Status = StatusTarefa.Cancelada;
        
        return Result.Ok();
    }
    
    public Result IniciarExecucao() {
        if (Status != StatusTarefa.Pendente) {
            return Result.Fail("Apenas tarefas pendentes podem ser iniciadas");
        }
        
        Status = StatusTarefa.EmAndamento;
        
        return Result.Ok();
    }
    
    public Result AdicionarComentario(string texto, Guid usuarioId) {
        if (string.IsNullOrWhiteSpace(texto)) {
            return Result.Fail("Comentário não pode ser vazio");
        }
        
        var comentario = new Comentario(texto, usuarioId);
        _comentarios.Add(comentario);
        
        return Result.Ok();
    }
    
    public Result AtualizarPrioridade(Prioridade novaPrioridade) {
        if (Status == StatusTarefa.Concluida || Status == StatusTarefa.Cancelada) {
            return Result.Fail("Não é possível alterar prioridade de tarefa finalizada");
        }
        
        Prioridade = novaPrioridade;
        
        return Result.Ok();
    }
    
    public bool EstaVencida() {
        return DataVencimento.HasValue &&
               DataVencimento.Value < DateTime.UtcNow &&
               Status != StatusTarefa.Concluida;
    }
}
```

### **Value Objects (DDD)**

```csharp
// TaskManagement.Domain/ValueObjects/Titulo.cs
namespace TaskManagement.Domain.ValueObjects;

/// <summary>
/// Value Object - Imutável e comparável por valor
/// </summary>
public class Titulo {
    public string Valor { get; }
    
    private Titulo(string valor) {
        Valor = valor;
    }
    
    public static Result<Titulo> Criar(string valor) {
        if (string.IsNullOrWhiteSpace(valor)) {
            return Result.Fail<Titulo>("Título não pode ser vazio");
        }
        
        if (valor.Length < 3) {
            return Result.Fail<Titulo>("Título deve ter no mínimo 3 caracteres");
        }
        
        if (valor.Length > 100) {
            return Result.Fail<Titulo>("Título deve ter no máximo 100 caracteres");
        }
        
        return Result.Ok(new Titulo(valor));
    }
    
    public override bool Equals(object obj) {
        if (obj is not Titulo other) return false;
        return Valor == other.Valor;
    }
    
    public override int GetHashCode() => Valor.GetHashCode();
    
    public override string ToString() => Valor;
}

// TaskManagement.Domain/ValueObjects/StatusTarefa.cs
namespace TaskManagement.Domain.ValueObjects;

public enum StatusTarefa {
    Pendente = 0,
    EmAndamento = 1,
    Concluida = 2,
    Cancelada = 3
}

// TaskManagement.Domain/ValueObjects/Prioridade.cs
namespace TaskManagement.Domain.ValueObjects;

public enum Prioridade {
    Baixa = 0,
    Media = 1,
    Alta = 2,
    Urgente = 3
}

// TaskManagement.Domain/Entities/Comentario.cs
namespace TaskManagement.Domain.Entities;

public class Comentario {
    public Guid Id { get; private set; }
    public string Texto { get; private set; }
    public Guid UsuarioId { get; private set; }
    public DateTime DataCriacao { get; private set; }
    
    private Comentario() { }
    
    public Comentario(string texto, Guid usuarioId) {
        Id = Guid.NewGuid();
        Texto = texto;
        UsuarioId = usuarioId;
        DataCriacao = DateTime.UtcNow;
    }
}
```

### **Interfaces de Repositório (DIP - SOLID)**

```csharp
// TaskManagement.Domain/Interfaces/ITarefaRepository.cs
namespace TaskManagement.Domain.Interfaces;

public interface ITarefaRepository {
    Task<TarefaTask> BuscarPorIdAsync(Guid id);
    Task<List<TarefaTask>> BuscarTodosPorUsuarioAsync(Guid usuarioId);
    Task<List<TarefaTask>> BuscarPorStatusAsync(StatusTarefa status);
    Task AdicionarAsync(TarefaTask tarefa);
    Task AtualizarAsync(TarefaTask tarefa);
    Task RemoverAsync(TarefaTask tarefa);
}
```

### **Unit of Work Interface**

```csharp
// TaskManagement.Domain/Interfaces/IUnitOfWork.cs
namespace TaskManagement.Domain.Interfaces;

/// <summary>
/// Unit of Work Pattern - Gerencia transações
/// </summary>
public interface IUnitOfWork : IDisposable {
    ITarefaRepository Tarefas { get; }
    
    Task<int> CommitAsync();
    Task RollbackAsync();
}
```

---

**Continua no próximo arquivo com Application, Infrastructure, API e Tests...**

[📄 Ver Implementação Completa](#)