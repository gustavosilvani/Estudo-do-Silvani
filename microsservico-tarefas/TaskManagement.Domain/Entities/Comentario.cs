namespace TaskManagement.Domain.Entities;

/// <summary>
/// Entity Comentário (DDD)
/// </summary>
public class Comentario {
    public Guid Id { get; private set; }
    public string Texto { get; private set; }
    public Guid UsuarioId { get; private set; }
    public DateTime DataCriacao { get; private set; }
    
    // Construtor para EF Core
    private Comentario() {
        Texto = string.Empty;
    }
    
    public Comentario(string texto, Guid usuarioId) {
        Id = Guid.NewGuid();
        Texto = texto;
        UsuarioId = usuarioId;
        DataCriacao = DateTime.UtcNow;
    }
}
