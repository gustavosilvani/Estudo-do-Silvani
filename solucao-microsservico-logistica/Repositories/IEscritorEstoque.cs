namespace MicrosservicoLogistica.Repositories;

// ============================================
// ISP: Interface segregada para ESCRITA de estoque
// Clientes que só precisam escrever não são forçados a implementar leitura
// ============================================
public interface IEscritorEstoque
{
    void Reservar(string produtoId, int quantidade);
    void LiberarReserva(string produtoId, int quantidade);
    void AtualizarQuantidade(string produtoId, int quantidade);
}