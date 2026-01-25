namespace MicrosservicoLogistica.Notifications;

// ============================================
// LSP: NotificadorSMS pode ser substituído por qualquer INotificador
// OCP: Nova implementação adicionada sem modificar código existente
// ============================================
public class NotificadorSMS : INotificador
{
    public string Canal => "SMS";

    public void Enviar(string destinatario, string mensagem)
    {
        // Simulação de envio de SMS
        Console.WriteLine($"[SMS] Para: {destinatario}");
        Console.WriteLine($"[SMS] Mensagem: {mensagem}");
        Console.WriteLine($"[SMS] Enviado com sucesso!");
    }
}