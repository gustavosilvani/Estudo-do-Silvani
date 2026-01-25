using MicrosservicoLogistica.Domain;
using MicrosservicoLogistica.Strategies;

namespace MicrosservicoLogistica.Services;

// ============================================
// SRP: CalculadoraFrete - Responsabilidade única: Cálculo de frete
// ============================================
public class CalculadoraFrete
{
    private readonly IEstrategiaFrete _estrategiaFrete;

    // DIP: Depende de abstração IEstrategiaFrete, não de implementação concreta
    // OCP: Permite adicionar novas estratégias sem modificar esta classe
    public CalculadoraFrete(IEstrategiaFrete estrategiaFrete)
    {
        _estrategiaFrete = estrategiaFrete;
    }

    // Responsabilidade única: Calcular valor do frete
    public decimal CalcularFrete(Pedido pedido)
    {
        if (pedido == null || pedido.Itens == null || pedido.Itens.Count == 0)
            throw new ArgumentException("Pedido inválido para cálculo de frete");

        // Delega cálculo para estratégia específica (OCP)
        return _estrategiaFrete.Calcular(pedido);
    }
}