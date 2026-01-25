using MicrosservicoLogistica.Domain;

namespace MicrosservicoLogistica.Strategies;

// ============================================
// OCP: Exemplo de como adicionar nova estratégia sem modificar código existente
// ============================================
public class FreteGratis : IEstrategiaFrete
{
    public string TipoFrete => "Grátis";

    public decimal Calcular(Pedido pedido)
    {
        // Frete grátis para pedidos acima de R$ 200,00
        decimal valorTotal = pedido.Itens.Sum(item => item.PrecoUnitario * item.Quantidade);

        if (valorTotal >= 200.00m)
            return 0.00m;

        // Se não atingir o valor, usa cálculo padrão
        return new FretePadrao().Calcular(pedido);
    }
}