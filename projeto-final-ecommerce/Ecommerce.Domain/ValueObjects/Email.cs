namespace Ecommerce.Domain.ValueObjects;

/// <summary>
/// Value Object: Email - Imutável, validado
/// </summary>
public class Email
{
    public string Valor { get; }

    public Email(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor) || !valor.Contains("@"))
        {
            throw new ArgumentException("Email inválido", nameof(valor));
        }
        Valor = valor.ToLowerInvariant();
    }

    public override bool Equals(object? obj)
    {
        return obj is Email outro && Valor == outro.Valor;
    }

    public override int GetHashCode() => Valor.GetHashCode();

    public override string ToString() => Valor;
}