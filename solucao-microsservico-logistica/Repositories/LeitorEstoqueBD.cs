namespace MicrosservicoLogistica.Repositories;

// ============================================
// Implementação concreta de leitor de estoque
// ISP: Implementa apenas ILeitorEstoque
// ============================================
public class LeitorEstoqueBD : ILeitorEstoque
{
    // Simulação de banco de dados em memória
    private readonly Dictionary<string, int> _estoque = new Dictionary<string, int>
    {
        { "PROD001", 100 },
        { "PROD002", 50 },
        { "PROD003", 200 }
    };

    public int ObterQuantidade(string produtoId)
    {
        _estoque.TryGetValue(produtoId, out var quantidade);
        return quantidade;
    }

    public bool VerificarDisponibilidade(string produtoId, int quantidade)
    {
        return ObterQuantidade(produtoId) >= quantidade;
    }

    public Dictionary<string, int> ObterEstoqueCompleto()
    {
        return new Dictionary<string, int>(_estoque);
    }
}