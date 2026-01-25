using MicrosservicoLogistica.Domain;

namespace MicrosservicoLogistica.Repositories;

// ============================================
// Implementação concreta do repositório (poderia ser SQL Server, MongoDB, etc.)
// DIP: Esta implementação depende da interface, não o contrário
// ============================================
public class RepositorioPedidoBD : IRepositorioPedido
{
    // Simulação de banco de dados em memória para exemplo
    private readonly Dictionary<string, Pedido> _pedidos = new Dictionary<string, Pedido>();

    public Pedido BuscarPorId(string id)
    {
        _pedidos.TryGetValue(id, out var pedido);
        return pedido;
    }

    public Pedido BuscarPorCodigoRastreamento(string codigoRastreamento)
    {
        return _pedidos.Values.FirstOrDefault(p => p.CodigoRastreamento == codigoRastreamento);
    }

    public void Salvar(Pedido pedido)
    {
        _pedidos[pedido.Id] = pedido;
    }

    public void Atualizar(Pedido pedido)
    {
        if (_pedidos.ContainsKey(pedido.Id))
        {
            _pedidos[pedido.Id] = pedido;
        }
    }

    public List<Pedido> ListarPorCliente(string clienteId)
    {
        return _pedidos.Values.Where(p => p.ClienteId == clienteId).ToList();
    }
}