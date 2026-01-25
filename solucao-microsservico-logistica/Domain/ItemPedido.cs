namespace MicrosservicoLogistica.Domain;

// Entidade de item do pedido
public class ItemPedido
{
    public string ProdutoId { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}