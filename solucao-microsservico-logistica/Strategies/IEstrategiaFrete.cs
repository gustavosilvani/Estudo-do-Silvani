using MicrosservicoLogistica.Domain;

namespace MicrosservicoLogistica.Strategies;

// ============================================
// OCP: Interface que permite adicionar novas estratégias sem modificar código existente
// ============================================
public interface IEstrategiaFrete
{
    decimal Calcular(Pedido pedido);
    string TipoFrete { get; }
}