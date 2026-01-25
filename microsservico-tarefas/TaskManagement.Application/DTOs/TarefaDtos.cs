using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.DTOs;

/// <summary>
/// DTO para criar tarefa
/// Clean Code: Nomes descritivos, validação de dados
/// </summary>
public class CriarTarefaDto {
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public Prioridade Prioridade { get; set; }
    public DateTime? DataVencimento { get; set; }
}

public class AtualizarTarefaDto {
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public Prioridade? Prioridade { get; set; }
    public DateTime? DataVencimento { get; set; }
}

public class TarefaDto {
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Prioridade { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public DateTime? DataVencimento { get; set; }
    public bool EstaVencida { get; set; }
    public int TotalComentarios { get; set; }
}

public class ComentarioDto {
    public Guid Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public Guid UsuarioId { get; set; }
    public DateTime DataCriacao { get; set; }
}

public class AdicionarComentarioDto {
    public string Texto { get; set; } = string.Empty;
}
