using MicrosservicoLogistica.Domain;

namespace MicrosservicoLogistica.Strategies;

// ============================================
// OCP: Estratégia de frete expresso
// Nova estratégia adicionada sem modificar código existente
// ============================================
public class FreteExpresso : IEstrategiaFrete
{
    public string TipoFrete => "Expresso";

    public decimal Calcular(Pedido pedido)
    {
        // Cálculo expresso: R$ 25,00 + R$ 5,00 por item (mais caro, mais rápido)
        decimal valorBase = 25.00m;
        decimal valorPorItem = 5.00m;
        decimal totalItens = pedido.Itens.Sum(item => item.Quantidade);

        return valorBase + (valorPorItem * totalItens);
    }
}