using MicrosservicoLogistica.Domain;

namespace MicrosservicoLogistica.Repositories;

// ============================================
// DIP: Interface de repositório de pedidos
// Serviços dependem desta abstração, não de implementação concreta
// ============================================
public interface IRepositorioPedido
{
    Pedido BuscarPorId(string id);
    Pedido BuscarPorCodigoRastreamento(string codigoRastreamento);
    void Salvar(Pedido pedido);
    void Atualizar(Pedido pedido);
    List<Pedido> ListarPorCliente(string clienteId);
}