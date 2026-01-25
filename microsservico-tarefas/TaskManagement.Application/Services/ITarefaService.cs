using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Common;

namespace TaskManagement.Application.Services;

/// <summary>
/// Interface do Serviço de Aplicação (DIP/SOLID)
/// </summary>
public interface ITarefaService {
    Task<Result<TarefaDto>> CriarAsync(CriarTarefaDto dto, Guid usuarioId);
    Task<Result<TarefaDto>> BuscarPorIdAsync(Guid id, Guid usuarioId);
    Task<Result<List<TarefaDto>>> BuscarTodosPorUsuarioAsync(Guid usuarioId);
    Task<Result> ConcluirAsync(Guid id, Guid usuarioId);
    Task<Result> CancelarAsync(Guid id, Guid usuarioId);
    Task<Result> IniciarExecucaoAsync(Guid id, Guid usuarioId);
    Task<Result> AdicionarComentarioAsync(Guid id, Guid usuarioId, AdicionarComentarioDto dto);
    Task<Result> RemoverAsync(Guid id, Guid usuarioId);
}
