namespace TaskManagement.Domain.Interfaces;

/// <summary>
/// Unit of Work Pattern - Gerencia transações atômicas
/// Garante consistência: commit ou rollback de todas operações
/// </summary>
public interface IUnitOfWork : IDisposable {
    ITarefaRepository Tarefas { get; }
    
    /// <summary>
    /// Confirma todas alterações pendentes
    /// </summary>
    Task<int> CommitAsync();
    
    /// <summary>
    /// Desfaz todas alterações pendentes
    /// </summary>
    Task RollbackAsync();
}
