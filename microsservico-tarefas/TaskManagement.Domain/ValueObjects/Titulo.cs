using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.ValueObjects;

/// <summary>
/// Value Object (DDD) - Imutável, comparável por valor
/// Clean Code: Encapsulamento, validação centralizada
/// </summary>
public class Titulo {
    public string Valor { get; }
    
    private Titulo(string valor) {
        Valor = valor;
    }
    
    public static Result<Titulo> Criar(string valor) {
        if (string.IsNullOrWhiteSpace(valor)) {
            return Result.Fail<Titulo>("Título não pode ser vazio");
        }
        
        if (valor.Length < 3) {
            return Result.Fail<Titulo>("Título deve ter no mínimo 3 caracteres");
        }
        
        if (valor.Length > 100) {
            return Result.Fail<Titulo>("Título deve ter no máximo 100 caracteres");
        }
        
        return Result.Ok(new Titulo(valor.Trim()));
    }
    
    public override bool Equals(object? obj) {
        if (obj is not Titulo other) return false;
        return Valor == other.Valor;
    }
    
    public override int GetHashCode() => Valor.GetHashCode();
    
    public override string ToString() => Valor;
    
    public static implicit operator string(Titulo titulo) => titulo.Valor;
}
