namespace MicrosservicoLogistica.Repositories;

// ============================================
// ISP: Interface segregada para LEITURA de estoque
// Clientes que só precisam ler não são forçados a implementar escrita
// ============================================
public interface ILeitorEstoque
{
    int ObterQuantidade(string produtoId);
    bool VerificarDisponibilidade(string produtoId, int quantidade);
    Dictionary<string, int> ObterEstoqueCompleto();
}