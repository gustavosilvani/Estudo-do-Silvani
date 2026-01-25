using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.ValueObjects;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

/// <summary>
/// Implementação do Repositório (DDD + Repository Pattern)
/// Aplica: DIP (depende de abstração), SRP (apenas persistência)
/// </summary>
public class TarefaRepository : ITarefaRepository {
    private readonly TaskManagementDbContext _context;
    
    public TarefaRepository(TaskManagementDbContext context) {
        _context = context;
    }
    
    public async Task<TarefaTask?> BuscarPorIdAsync(Guid id) {
        return await _context.Tarefas
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    
    public async Task<List<TarefaTask>> BuscarTodosPorUsuarioAsync(Guid usuarioId) {
        return await _context.Tarefas
            .Where(t => t.UsuarioId == usuarioId)
            .OrderByDescending(t => t.DataCriacao)
            .ToListAsync();
    }
    
    public async Task<List<TarefaTask>> BuscarPorStatusAsync(Guid usuarioId, StatusTarefa status) {
        return await _context.Tarefas
            .Where(t => t.UsuarioId == usuarioId && t.Status == status)
            .OrderByDescending(t => t.DataCriacao)
            .ToListAsync();
    }
    
    public async Task<List<TarefaTask>> BuscarVencidasAsync(Guid usuarioId) {
        var hoje = DateTime.UtcNow;
        
        return await _context.Tarefas
            .Where(t => t.UsuarioId == usuarioId &&
                       t.DataVencimento.HasValue &&
                       t.DataVencimento.Value < hoje &&
                       t.Status != StatusTarefa.Concluida)
            .OrderBy(t => t.DataVencimento)
            .ToListAsync();
    }
    
    public async Task AdicionarAsync(TarefaTask tarefa) {
        await _context.Tarefas.AddAsync(tarefa);
    }
    
    public void Atualizar(TarefaTask tarefa) {
        _context.Tarefas.Update(tarefa);
    }
    
    public void Remover(TarefaTask tarefa) {
        _context.Tarefas.Remove(tarefa);
    }
}
