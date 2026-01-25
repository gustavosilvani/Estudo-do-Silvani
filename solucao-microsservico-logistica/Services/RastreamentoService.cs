using MicrosservicoLogistica.Domain;
using MicrosservicoLogistica.Repositories;

namespace MicrosservicoLogistica.Services;

// ============================================
// SRP: RastreamentoService - Responsabilidade única: Rastreamento de pedidos
// ============================================
public class RastreamentoService
{
    private readonly IRepositorioPedido _repositorio;
    private readonly INotificacaoService _notificacaoService;

    // DIP: Depende de abstrações (interfaces), não de implementações concretas
    public RastreamentoService(
        IRepositorioPedido repositorio,
        INotificacaoService notificacaoService)
    {
        _repositorio = repositorio;
        _notificacaoService = notificacaoService;
    }

    // Responsabilidade única: Criar rastreamento de pedido
    public string CriarRastreamento(string pedidoId)
    {
        var pedido = _repositorio.BuscarPorId(pedidoId);
        if (pedido == null)
            throw new Exception("Pedido não encontrado");

        // Gera código de rastreamento único
        var codigoBase = pedidoId.Length >= 8 ? pedidoId.Substring(0, 8) : pedidoId.PadRight(8, '0');
        pedido.CodigoRastreamento = $"TRK{codigoBase.ToUpper()}{DateTime.Now:yyyyMMdd}";
        pedido.Status = StatusEntrega.Preparando;

        _repositorio.Atualizar(pedido);

        // Delega notificação para serviço especializado
        _notificacaoService.NotificarCriacaoRastreamento(pedido);

        return pedido.CodigoRastreamento;
    }

    // Responsabilidade única: Atualizar status de entrega
    public void AtualizarStatus(string codigoRastreamento, StatusEntrega novoStatus)
    {
        var pedido = _repositorio.BuscarPorCodigoRastreamento(codigoRastreamento);
        if (pedido == null)
            throw new Exception("Pedido não encontrado");

        var statusAnterior = pedido.Status;
        pedido.Status = novoStatus;

        if (novoStatus == StatusEntrega.Entregue)
            pedido.DataEntrega = DateTime.Now;

        _repositorio.Atualizar(pedido);

        // Delega notificação para serviço especializado
        _notificacaoService.NotificarAtualizacaoStatus(pedido, statusAnterior, novoStatus);
    }

    // Responsabilidade única: Consultar rastreamento
    public Pedido ConsultarRastreamento(string codigoRastreamento)
    {
        return _repositorio.BuscarPorCodigoRastreamento(codigoRastreamento);
    }
}