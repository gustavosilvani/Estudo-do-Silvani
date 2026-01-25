using MicrosservicoLogistica.Domain;
using MicrosservicoLogistica.Notifications;

namespace MicrosservicoLogistica.Services;

// ============================================
// SRP: NotificacaoService - Responsabilidade única: Orquestração de notificações
// ============================================
public class NotificacaoService : INotificacaoService
{
    private readonly List<INotificador> _notificadores;

    // DIP: Depende de lista de abstrações INotificador
    // OCP: Pode adicionar novos notificadores sem modificar esta classe
    public NotificacaoService(List<INotificador> notificadores)
    {
        _notificadores = notificadores ?? new List<INotificador>();
    }

    // Responsabilidade única: Notificar criação de rastreamento
    public void NotificarCriacaoRastreamento(Pedido pedido)
    {
        var mensagem = $"Seu pedido {pedido.Id} foi criado. Código de rastreamento: {pedido.CodigoRastreamento}";

        // LSP: Qualquer implementação de INotificador pode ser usada
        foreach (var notificador in _notificadores)
        {
            notificador.Enviar(pedido.ClienteId, mensagem);
        }
    }

    // Responsabilidade única: Notificar atualização de status
    public void NotificarAtualizacaoStatus(
        Pedido pedido,
        StatusEntrega statusAnterior,
        StatusEntrega novoStatus)
    {
        var mensagem = $"Status do pedido {pedido.Id} alterado de {statusAnterior} para {novoStatus}";

        // LSP: Qualquer implementação de INotificador pode ser usada
        foreach (var notificador in _notificadores)
        {
            notificador.Enviar(pedido.ClienteId, mensagem);
        }
    }
}