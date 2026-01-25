using MicrosservicoLogistica.Domain;

namespace MicrosservicoLogistica.Services;

// Interface para serviço de notificações
public interface INotificacaoService
{
    void NotificarCriacaoRastreamento(Pedido pedido);
    void NotificarAtualizacaoStatus(Pedido pedido, StatusEntrega statusAnterior, StatusEntrega novoStatus);
}