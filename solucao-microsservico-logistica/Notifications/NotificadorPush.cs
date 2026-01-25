namespace MicrosservicoLogistica.Notifications;

// ============================================
// LSP: NotificadorPush pode ser substituído por qualquer INotificador
// OCP: Nova implementação adicionada sem modificar código existente
// ============================================
public class NotificadorPush : INotificador
{
    public string Canal => "Push";

    public void Enviar(string destinatario, string mensagem)
    {
        // Simulação de notificação push
        Console.WriteLine($"[PUSH] Para: {destinatario}");
        Console.WriteLine($"[PUSH] Mensagem: {mensagem}");
        Console.WriteLine($"[PUSH] Enviado com sucesso!");
    }
}