namespace Ecommerce.Domain.Entities;

/// <summary>
/// Aggregate Root: Pedido - Controla acesso ao aggregate
/// </summary>
public class Pedido
{
    public Guid Id { get; private set; }
    public Guid ClienteId { get; private set; }
    public StatusPedido Status { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataFinalizacao { get; private set; }

    private readonly List<ItemPedido> _itens = new();
    public IReadOnlyList<ItemPedido> Itens => _itens.AsReadOnly();

    public ValueObjects.Dinheiro Total => new(_itens.Sum(i => i.Subtotal.Valor));

    private Pedido() { } // EF Core

    public Pedido(Guid clienteId)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
        Status = StatusPedido.Rascunho;
        DataCriacao = DateTime.UtcNow;
    }

    public void AdicionarItem(Guid produtoId, int quantidade, ValueObjects.Dinheiro precoUnitario)
    {
        ValidarPodeAlterar();

        if (quantidade <= 0)
        {
            throw new ArgumentException("Quantidade deve ser maior que zero", nameof(quantidade));
        }

        var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (itemExistente != null)
        {
            itemExistente.AlterarQuantidade(itemExistente.Quantidade + quantidade);
        }
        else
        {
            var novoItem = new ItemPedido(produtoId, quantidade, precoUnitario);
            _itens.Add(novoItem);
        }
    }

    public void RemoverItem(Guid itemId)
    {
        ValidarPodeAlterar();

        var item = _itens.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _itens.Remove(item);
        }
    }

    public void Finalizar()
    {
        if (Status != StatusPedido.Rascunho)
        {
            throw new InvalidOperationException("Apenas pedidos em rascunho podem ser finalizados");
        }

        if (!_itens.Any())
        {
            throw new InvalidOperationException("Pedido não pode estar vazio");
        }

        Status = StatusPedido.Finalizado;
        DataFinalizacao = DateTime.UtcNow;
    }

    private void ValidarPodeAlterar()
    {
        if (Status != StatusPedido.Rascunho)
        {
            throw new InvalidOperationException("Pedido já finalizado, não pode ser alterado");
        }
    }
}

public enum StatusPedido
{
    Rascunho = 1,
    Finalizado = 2,
    Cancelado = 3
}