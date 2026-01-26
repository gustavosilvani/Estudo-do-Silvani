namespace Ecommerce.Domain.Entities;

/// <summary>
/// Entity: Cliente - Tem identidade única
/// </summary>
public class Cliente
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public ValueObjects.Email Email { get; private set; } = null!;
    public DateTime DataCriacao { get; private set; }

    private readonly List<Pedido> _pedidos = new();
    public IReadOnlyList<Pedido> Pedidos => _pedidos.AsReadOnly();

    private Cliente() { } // EF Core

    public Cliente(string nome, ValueObjects.Email email)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        DataCriacao = DateTime.UtcNow;
    }

    public void AlterarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
        {
            throw new ArgumentException("Nome não pode ser vazio", nameof(novoNome));
        }
        Nome = novoNome;
    }
}