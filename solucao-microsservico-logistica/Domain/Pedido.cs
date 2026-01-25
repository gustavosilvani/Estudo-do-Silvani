namespace MicrosservicoLogistica.Domain;

// Entidade principal de pedido
public class Pedido
{
    public string Id { get; set; } = string.Empty;
    public string ClienteId { get; set; } = string.Empty;
    public List<ItemPedido> Itens { get; set; }
    public Endereco EnderecoEntrega { get; set; } = new Endereco();
    public StatusEntrega Status { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataEntrega { get; set; }
    public string CodigoRastreamento { get; set; } = string.Empty;
    public decimal ValorFrete { get; set; }

    public Pedido()
    {
        Itens = new List<ItemPedido>();
        Status = StatusEntrega.Pendente;
        DataCriacao = DateTime.Now;
    }
}