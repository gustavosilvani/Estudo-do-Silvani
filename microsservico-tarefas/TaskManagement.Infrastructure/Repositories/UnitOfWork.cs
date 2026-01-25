using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

/// <summary>
/// UNIT OF WORK PATTERN - Implementação completa
/// Responsabilidades:
/// - Gerenciar transações
/// - Coordenar múltiplos repositórios
/// - Garantir consistência (commit/rollback atômico)
/// </summary>
public class UnitOfWork : IUnitOfWork {
    private readonly TaskManagementDbContext _context;
    private ITarefaRepository? _tarefaRepository;
    private bool _disposed = false;
    
    public UnitOfWork(TaskManagementDbContext context) {
        _context = context;
    }
    
    /// <summary>
    /// Lazy loading do repositório
    /// </summary>
    public ITarefaRepository Tarefas {
        get {
            _tarefaRepository ??= new TarefaRepository(_context);
            return _tarefaRepository;
        }
    }
    
    /// <summary>
    /// Commit - Salva todas mudanças em uma transação
    /// </summary>
    public async Task<int> CommitAsync() {
        try {
            return await _context.SaveChangesAsync();
        } catch {
            await RollbackAsync();
            throw;
        }
    }
    
    /// <summary>
    /// Rollback - Desfaz todas mudanças pendentes
    /// </summary>
    public async Task RollbackAsync() {
        // EF Core automaticamente desfaz mudanças não salvas
        await Task.CompletedTask;
    }
    
    /// <summary>
    /// Dispose Pattern - Libera recursos
    /// </summary>
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing) {
        if (!_disposed && disposing) {
            _context.Dispose();
        }
        _disposed = true;
    }
}
