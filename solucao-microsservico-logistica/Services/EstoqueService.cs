using MicrosservicoLogistica.Domain;
using MicrosservicoLogistica.Repositories;

namespace MicrosservicoLogistica.Services;

// ============================================
// SRP: EstoqueService - Responsabilidade única: Gestão de estoque
// ============================================
public class EstoqueService
{
    private readonly ILeitorEstoque _leitorEstoque;
    private readonly IEscritorEstoque _escritorEstoque;

    // DIP + ISP: Depende de interfaces segregadas
    // ISP: Não força implementar métodos de leitura se só precisa escrever
    public EstoqueService(
        ILeitorEstoque leitorEstoque,
        IEscritorEstoque escritorEstoque)
    {
        _leitorEstoque = leitorEstoque;
        _escritorEstoque = escritorEstoque;
    }

    // Responsabilidade única: Verificar disponibilidade
    public bool VerificarDisponibilidade(string produtoId, int quantidade)
    {
        var estoqueAtual = _leitorEstoque.ObterQuantidade(produtoId);
        return estoqueAtual >= quantidade;
    }

    // Responsabilidade única: Reservar produtos
    public void ReservarProdutos(List<ItemPedido> itens)
    {
        foreach (var item in itens)
        {
            if (!VerificarDisponibilidade(item.ProdutoId, item.Quantidade))
                throw new Exception($"Produto {item.ProdutoId} sem estoque suficiente");

            _escritorEstoque.Reservar(item.ProdutoId, item.Quantidade);
        }
    }

    // Responsabilidade única: Liberar reserva
    public void LiberarReserva(List<ItemPedido> itens)
    {
        foreach (var item in itens)
        {
            _escritorEstoque.LiberarReserva(item.ProdutoId, item.Quantidade);
        }
    }
}