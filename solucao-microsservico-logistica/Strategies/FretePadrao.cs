using MicrosservicoLogistica.Domain;

namespace MicrosservicoLogistica.Strategies;

// ============================================
// OCP: Estratégia de frete padrão
// Pode adicionar novas estratégias sem modificar CalculadoraFrete
// ============================================
public class FretePadrao : IEstrategiaFrete
{
    public string TipoFrete => "Padrão";

    public decimal Calcular(Pedido pedido)
    {
        // Cálculo base: R$ 10,00 + R$ 2,00 por item
        decimal valorBase = 10.00m;
        decimal valorPorItem = 2.00m;
        decimal totalItens = pedido.Itens.Sum(item => item.Quantidade);

        return valorBase + (valorPorItem * totalItens);
    }
}