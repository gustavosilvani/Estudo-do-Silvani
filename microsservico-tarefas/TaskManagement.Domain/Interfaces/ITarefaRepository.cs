using TaskManagement.Domain.Entities;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain.Interfaces;

/// <summary>
/// Interface de Repositório (DDD + DIP/SOLID)
/// Clean Code: Interface Segregation, Dependency Inversion
/// </summary>
public interface ITarefaRepository {
    Task<TarefaTask?> BuscarPorIdAsync(Guid id);
    Task<List<TarefaTask>> BuscarTodosPorUsuarioAsync(Guid usuarioId);
    Task<List<TarefaTask>> BuscarPorStatusAsync(Guid usuarioId, StatusTarefa status);
    Task<List<TarefaTask>> BuscarVencidasAsync(Guid usuarioId);
    Task AdicionarAsync(TarefaTask tarefa);
    void Atualizar(TarefaTask tarefa);
    void Remover(TarefaTask tarefa);
}
