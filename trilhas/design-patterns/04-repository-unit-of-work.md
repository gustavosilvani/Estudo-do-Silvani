# 04 - Repository e Unit of Work

Estes padrões não fazem parte do catálogo GoF, mas são **muito usados em .NET** e em projetos com DDD/Clean Architecture para abstrair persistência e transações.

## Repository

O **Repository** abstrai o acesso a dados: a aplicação trabalha com uma “coleção em memória” de entidades, e o repositório traduz isso em operações de persistência (banco, API, etc.).

### Benefícios

- **Desacoplamento**: Domain e Application não dependem de SQL ou de um ORM específico
- **Testabilidade**: Fácil trocar por um repositório em memória nos testes
- **Linguagem ubíqua**: “Repositório de Tarefas” soa natural no domínio

### Exemplo em C#

```csharp
public interface ITarefaRepository
{
    Task<Tarefa?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Tarefa>> ListarAsync(CancellationToken ct = default);
    void Adicionar(Tarefa tarefa);
    void Atualizar(Tarefa tarefa);
    void Remover(Tarefa tarefa);
}

public class TarefaRepository : ITarefaRepository
{
    private readonly TaskManagementDbContext _context;

    public TarefaRepository(TaskManagementDbContext context) => _context = context;

    public async Task<Tarefa?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Tarefas.FindAsync(new object[] { id }, ct);

    public async Task<IReadOnlyList<Tarefa>> ListarAsync(CancellationToken ct = default)
        => await _context.Tarefas.ToListAsync(ct);

    public void Adicionar(Tarefa tarefa) => _context.Tarefas.Add(tarefa);
    public void Atualizar(Tarefa tarefa) => _context.Tarefas.Update(tarefa);
    public void Remover(Tarefa tarefa) => _context.Tarefas.Remove(tarefa);
}
```

O domínio e a aplicação dependem apenas de `ITarefaRepository`; a implementação com EF Core fica na camada de infraestrutura.

---

## Unit of Work

O **Unit of Work** agrupa **várias operações de persistência** em uma **única transação**. Um único “commit” aplica todas as mudanças; em caso de erro, nada é persistido.

### Benefícios

- Transação explícita em cenários com mais de um repositório ou operação
- Um único `SaveChangesAsync()` (ou equivalente) por caso de uso

### Exemplo em C#

```csharp
public interface IUnitOfWork : IDisposable
{
    ITarefaRepository Tarefas { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly TaskManagementDbContext _context;
    private ITarefaRepository? _tarefas;

    public UnitOfWork(TaskManagementDbContext context) => _context = context;

    public ITarefaRepository Tarefas =>
        _tarefas ??= new TarefaRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public void Dispose() => _context.Dispose();
}
```

Uso no serviço de aplicação:

```csharp
public async Task<Result<TarefaDto>> CriarAsync(CriarTarefaDto dto, CancellationToken ct)
{
    var tarefa = Tarefa.Criar(dto.Titulo, dto.Descricao);
    _unitOfWork.Tarefas.Adicionar(tarefa);
    await _unitOfWork.SaveChangesAsync(ct);
    return Result.Success(Mapear(tarefa));
}
```

---

**Onde ver no repositório**

- **Microsserviço de Tarefas**: `TaskManagement.Infrastructure` — `TarefaRepository`, `UnitOfWork`; uso em `TarefaService`.
- **Microsserviço de Logística**: Uso de repositórios e transação no fluxo de pedidos.
- **Projeto Final E-commerce**: Repositórios e Unit of Work na camada de infraestrutura.

---

**Próximo**: [05 - Aplicação Prática](./05-aplicacao-pratica.md)
