using TaskManagement.Domain.Common;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain.Entities;

/// <summary>
/// Aggregate Root - Tarefa (DDD)
/// Aplica: SOLID (SRP, OCP), Encapsulamento, Regras de Negócio
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
    
    private List<Comentario> _comentarios;
    public IReadOnlyList<Comentario> Comentarios => _comentarios.AsReadOnly();
    
    // Construtor privado para EF Core
    private TarefaTask() {
        _comentarios = new List<Comentario>();
        Descricao = string.Empty;
        Titulo = null!;
    }
    
    /// <summary>
    /// Factory Method (Design Pattern)
    /// </summary>
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
            DataVencimento = dataVencimento,
            _comentarios = new List<Comentario>()
        };
        
        return Result.Ok(tarefa);
    }
    
    /// <summary>
    /// Regra de negócio: Concluir tarefa
    /// </summary>
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
    
    /// <summary>
    /// Regra de negócio: Cancelar tarefa
    /// </summary>
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
    
    /// <summary>
    /// Regra de negócio: Iniciar execução
    /// </summary>
    public Result IniciarExecucao() {
        if (Status != StatusTarefa.Pendente) {
            return Result.Fail("Apenas tarefas pendentes podem ser iniciadas");
        }
        
        Status = StatusTarefa.EmAndamento;
        
        return Result.Ok();
    }
    
    /// <summary>
    /// Adicionar comentário
    /// </summary>
    public Result AdicionarComentario(string texto, Guid usuarioId) {
        if (string.IsNullOrWhiteSpace(texto)) {
            return Result.Fail("Comentário não pode ser vazio");
        }
        
        if (texto.Length > 500) {
            return Result.Fail("Comentário deve ter no máximo 500 caracteres");
        }
        
        var comentario = new Comentario(texto, usuarioId);
        _comentarios.Add(comentario);
        
        return Result.Ok();
    }
    
    /// <summary>
    /// Atualizar prioridade
    /// </summary>
    public Result AtualizarPrioridade(Prioridade novaPrioridade) {
        if (Status == StatusTarefa.Concluida || Status == StatusTarefa.Cancelada) {
            return Result.Fail("Não é possível alterar prioridade de tarefa finalizada");
        }
        
        Prioridade = novaPrioridade;
        
        return Result.Ok();
    }
    
    /// <summary>
    /// Verifica se tarefa está vencida
    /// </summary>
    public bool EstaVencida() {
        return DataVencimento.HasValue &&
               DataVencimento.Value < DateTime.UtcNow &&
               Status != StatusTarefa.Concluida;
    }
}
