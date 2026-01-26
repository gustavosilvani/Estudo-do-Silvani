namespace Ecommerce.Domain.Entities;

/// <summary>
/// Entity dentro do Aggregate: ItemPedido
/// </summary>
public class ItemPedido
{
    public Guid Id { get; private set; }
    public Guid ProdutoId { get; private set; }
    public int Quantidade { get; private set; }
    public ValueObjects.Dinheiro PrecoUnitario { get; private set; } = null!;
    public ValueObjects.Dinheiro Subtotal => new(Quantidade * PrecoUnitario.Valor);

    private ItemPedido() { } // EF Core

    public ItemPedido(Guid produtoId, int quantidade, ValueObjects.Dinheiro precoUnitario)
    {
        Id = Guid.NewGuid();
        ProdutoId = produtoId;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    public void AlterarQuantidade(int novaQuantidade)
    {
        if (novaQuantidade <= 0)
        {
            throw new ArgumentException("Quantidade deve ser maior que zero", nameof(novaQuantidade));
        }
        Quantidade = novaQuantidade;
    }
}