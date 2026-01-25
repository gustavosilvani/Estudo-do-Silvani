namespace MicrosservicoLogistica.Notifications;

// ============================================
// ISP: Interface segregada e simples
// Apenas o método necessário, sem configurações complexas
// ============================================
public interface INotificador
{
    void Enviar(string destinatario, string mensagem);
    string Canal { get; }
}