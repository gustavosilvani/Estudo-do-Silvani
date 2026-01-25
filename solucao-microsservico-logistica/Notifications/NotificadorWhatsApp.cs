namespace MicrosservicoLogistica.Notifications;

// ============================================
// OCP: Exemplo de como adicionar novo canal sem modificar código existente
// LSP: NotificadorWhatsApp pode substituir qualquer INotificador
// ============================================
public class NotificadorWhatsApp : INotificador
{
    public string Canal => "WhatsApp";

    public void Enviar(string destinatario, string mensagem)
    {
        // Simulação de envio via WhatsApp
        Console.WriteLine($"[WHATSAPP] Para: {destinatario}");
        Console.WriteLine($"[WHATSAPP] Mensagem: {mensagem}");
        Console.WriteLine($"[WHATSAPP] Enviado com sucesso!");
    }
}