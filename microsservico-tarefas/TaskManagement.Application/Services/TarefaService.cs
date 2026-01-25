using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services;

/// <summary>
/// Serviço de Aplicação - Use Cases (DDD)
/// Aplica: SOLID, Clean Code, Unit of Work, Result Pattern
/// Responsabilidade: Orquestrar operações de domínio
/// </summary>
public class TarefaService : ITarefaService {
    private readonly IUnitOfWork _unitOfWork;
    
    public TarefaService(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }
    
    /// <summary>
    /// Use Case: Criar Tarefa
    /// </summary>
    public async Task<Result<TarefaDto>> CriarAsync(CriarTarefaDto dto, Guid usuarioId) {
        // Criar entidade de domínio (regras de negócio aplicadas)
        var tarefaResult = TarefaTask.Criar(
            dto.Titulo,
            dto.Descricao,
            usuarioId,
            dto.Prioridade,
            dto.DataVencimento
        );
        
        if (tarefaResult.IsFailure) {
            return Result.Fail<TarefaDto>(tarefaResult.Error);
        }
        
        // Persistir usando Unit of Work
        await _unitOfWork.Tarefas.AdicionarAsync(tarefaResult.Value);
        await _unitOfWork.CommitAsync();
        
        return Result.Ok(MapToDto(tarefaResult.Value));
    }
    
    /// <summary>
    /// Use Case: Buscar Tarefa por ID
    /// </summary>
    public async Task<Result<TarefaDto>> BuscarPorIdAsync(Guid id, Guid usuarioId) {
        var tarefa = await _unitOfWork.Tarefas.BuscarPorIdAsync(id);
        
        if (tarefa == null) {
            return Result.Fail<TarefaDto>("Tarefa não encontrada");
        }
        
        if (tarefa.UsuarioId != usuarioId) {
            return Result.Fail<TarefaDto>("Acesso negado");
        }
        
        return Result.Ok(MapToDto(tarefa));
    }
    
    /// <summary>
    /// Use Case: Listar Tarefas do Usuário
    /// </summary>
    public async Task<Result<List<TarefaDto>>> BuscarTodosPorUsuarioAsync(Guid usuarioId) {
        var tarefas = await _unitOfWork.Tarefas.BuscarTodosPorUsuarioAsync(usuarioId);
        var dtos = tarefas.Select(MapToDto).ToList();
        
        return Result.Ok(dtos);
    }
    
    /// <summary>
    /// Use Case: Concluir Tarefa
    /// </summary>
    public async Task<Result> ConcluirAsync(Guid id, Guid usuarioId) {
        var tarefa = await _unitOfWork.Tarefas.BuscarPorIdAsync(id);
        
        if (tarefa == null) {
            return Result.Fail("Tarefa não encontrada");
        }
        
        if (tarefa.UsuarioId != usuarioId) {
            return Result.Fail("Acesso negado");
        }
        
        // Aplicar regra de negócio
        var resultado = tarefa.Concluir();
        if (resultado.IsFailure) {
            return resultado;
        }
        
        // Persistir mudança
        _unitOfWork.Tarefas.Atualizar(tarefa);
        await _unitOfWork.CommitAsync();
        
        return Result.Ok();
    }
    
    /// <summary>
    /// Use Case: Cancelar Tarefa
    /// </summary>
    public async Task<Result> CancelarAsync(Guid id, Guid usuarioId) {
        var tarefa = await _unitOfWork.Tarefas.BuscarPorIdAsync(id);
        
        if (tarefa == null) {
            return Result.Fail("Tarefa não encontrada");
        }
        
        if (tarefa.UsuarioId != usuarioId) {
            return Result.Fail("Acesso negado");
        }
        
        var resultado = tarefa.Cancelar();
        if (resultado.IsFailure) {
            return resultado;
        }
        
        _unitOfWork.Tarefas.Atualizar(tarefa);
        await _unitOfWork.CommitAsync();
        
        return Result.Ok();
    }
    
    /// <summary>
    /// Use Case: Iniciar Execução
    /// </summary>
    public async Task<Result> IniciarExecucaoAsync(Guid id, Guid usuarioId) {
        var tarefa = await _unitOfWork.Tarefas.BuscarPorIdAsync(id);
        
        if (tarefa == null) {
            return Result.Fail("Tarefa não encontrada");
        }
        
        if (tarefa.UsuarioId != usuarioId) {
            return Result.Fail("Acesso negado");
        }
        
        var resultado = tarefa.IniciarExecucao();
        if (resultado.IsFailure) {
            return resultado;
        }
        
        _unitOfWork.Tarefas.Atualizar(tarefa);
        await _unitOfWork.CommitAsync();
        
        return Result.Ok();
    }
    
    /// <summary>
    /// Use Case: Adicionar Comentário
    /// </summary>
    public async Task<Result> AdicionarComentarioAsync(Guid id, Guid usuarioId, AdicionarComentarioDto dto) {
        var tarefa = await _unitOfWork.Tarefas.BuscarPorIdAsync(id);
        
        if (tarefa == null) {
            return Result.Fail("Tarefa não encontrada");
        }
        
        if (tarefa.UsuarioId != usuarioId) {
            return Result.Fail("Acesso negado");
        }
        
        var resultado = tarefa.AdicionarComentario(dto.Texto, usuarioId);
        if (resultado.IsFailure) {
            return resultado;
        }
        
        _unitOfWork.Tarefas.Atualizar(tarefa);
        await _unitOfWork.CommitAsync();
        
        return Result.Ok();
    }
    
    /// <summary>
    /// Use Case: Remover Tarefa
    /// </summary>
    public async Task<Result> RemoverAsync(Guid id, Guid usuarioId) {
        var tarefa = await _unitOfWork.Tarefas.BuscarPorIdAsync(id);
        
        if (tarefa == null) {
            return Result.Fail("Tarefa não encontrada");
        }
        
        if (tarefa.UsuarioId != usuarioId) {
            return Result.Fail("Acesso negado");
        }
        
        _unitOfWork.Tarefas.Remover(tarefa);
        await _unitOfWork.CommitAsync();
        
        return Result.Ok();
    }
    
    /// <summary>
    /// Mapper: Entidade → DTO
    /// Clean Code: Método privado reutilizável
    /// </summary>
    private static TarefaDto MapToDto(TarefaTask tarefa) {
        return new TarefaDto {
            Id = tarefa.Id,
            Titulo = tarefa.Titulo,
            Descricao = tarefa.Descricao,
            Status = tarefa.Status.ToString(),
            Prioridade = tarefa.Prioridade.ToString(),
            DataCriacao = tarefa.DataCriacao,
            DataConclusao = tarefa.DataConclusao,
            DataVencimento = tarefa.DataVencimento,
            EstaVencida = tarefa.EstaVencida(),
            TotalComentarios = tarefa.Comentarios.Count
        };
    }
}
