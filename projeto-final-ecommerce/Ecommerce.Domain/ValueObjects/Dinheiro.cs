namespace Ecommerce.Domain.ValueObjects;

/// <summary>
/// Value Object: Dinheiro - Imutável, com validação
/// </summary>
public class Dinheiro
{
    public decimal Valor { get; }
    public string Moeda { get; }

    public Dinheiro(decimal valor, string moeda = "BRL")
    {
        if (valor < 0)
        {
            throw new ArgumentException("Valor não pode ser negativo", nameof(valor));
        }
        Valor = valor;
        Moeda = moeda;
    }

    public Dinheiro Somar(Dinheiro outro)
    {
        if (Moeda != outro.Moeda)
        {
            throw new InvalidOperationException("Não é possível somar valores de moedas diferentes");
        }
        return new Dinheiro(Valor + outro.Valor, Moeda);
    }

    public override bool Equals(object? obj)
    {
        return obj is Dinheiro outro && Valor == outro.Valor && Moeda == outro.Moeda;
    }

    public override int GetHashCode() => HashCode.Combine(Valor, Moeda);
}