using MicrosservicoLogistica.Domain;

namespace MicrosservicoLogistica.Strategies;

// ============================================
// OCP: Estratégia de frete econômico
// Nova estratégia adicionada sem modificar código existente
// ============================================
public class FreteEconomico : IEstrategiaFrete
{
    public string TipoFrete => "Econômico";

    public decimal Calcular(Pedido pedido)
    {
        // Cálculo econômico: R$ 5,00 + R$ 1,00 por item (mais barato, mais lento)
        decimal valorBase = 5.00m;
        decimal valorPorItem = 1.00m;
        decimal totalItens = pedido.Itens.Sum(item => item.Quantidade);

        return valorBase + (valorPorItem * totalItens);
    }
}