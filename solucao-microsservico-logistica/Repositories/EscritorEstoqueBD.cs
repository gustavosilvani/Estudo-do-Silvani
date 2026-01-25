namespace MicrosservicoLogistica.Repositories;

// ============================================
// Implementação concreta de escritor de estoque
// ISP: Implementa apenas IEscritorEstoque
// ============================================
public class EscritorEstoqueBD : IEscritorEstoque
{
    private readonly Dictionary<string, int> _estoque = new Dictionary<string, int>
    {
        { "PROD001", 100 },
        { "PROD002", 50 },
        { "PROD003", 200 }
    };

    private readonly Dictionary<string, int> _reservas = new Dictionary<string, int>();

    public void Reservar(string produtoId, int quantidade)
    {
        var estoqueAtual = _estoque.ContainsKey(produtoId) ? _estoque[produtoId] : 0;

        if (estoqueAtual < quantidade)
            throw new Exception($"Estoque insuficiente para produto {produtoId}");

        _estoque[produtoId] = estoqueAtual - quantidade;

        if (_reservas.ContainsKey(produtoId))
            _reservas[produtoId] += quantidade;
        else
            _reservas[produtoId] = quantidade;
    }

    public void LiberarReserva(string produtoId, int quantidade)
    {
        if (_reservas.ContainsKey(produtoId))
        {
            _reservas[produtoId] = Math.Max(0, _reservas[produtoId] - quantidade);
            _estoque[produtoId] = (_estoque.ContainsKey(produtoId) ? _estoque[produtoId] : 0) + quantidade;
        }
    }

    public void AtualizarQuantidade(string produtoId, int quantidade)
    {
        _estoque[produtoId] = quantidade;
    }
}