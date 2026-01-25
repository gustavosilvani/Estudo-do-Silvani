namespace MicrosservicoLogistica.Notifications;

// ============================================
// LSP: NotificadorEmail pode ser substituído por qualquer INotificador
// OCP: Nova implementação adicionada sem modificar código existente
// ============================================
public class NotificadorEmail : INotificador
{
    public string Canal => "Email";

    public void Enviar(string destinatario, string mensagem)
    {
        // Simulação de envio de email
        Console.WriteLine($"[EMAIL] Para: {destinatario}");
        Console.WriteLine($"[EMAIL] Mensagem: {mensagem}");
        Console.WriteLine($"[EMAIL] Enviado com sucesso!");
    }
}