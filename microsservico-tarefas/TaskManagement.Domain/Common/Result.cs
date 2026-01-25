namespace TaskManagement.Domain.Common;

/// <summary>
/// Result Pattern - Retorna sucesso ou erro sem exceções
/// Clean Code: Nomes descritivos, responsabilidade única
/// </summary>
public class Result {
    public bool Success { get; }
    public string Error { get; }
    public bool IsFailure => !Success;
    
    protected Result(bool success, string error) {
        if (success && error != string.Empty) {
            throw new InvalidOperationException("Resultado de sucesso não pode ter erro");
        }
        
        if (!success && error == string.Empty) {
            throw new InvalidOperationException("Resultado de falha deve ter erro");
        }
        
        Success = success;
        Error = error;
    }
    
    public static Result Ok() => new Result(true, string.Empty);
    public static Result Fail(string error) => new Result(false, error);
    
    public static Result<T> Ok<T>(T value) => new Result<T>(value, true, string.Empty);
    public static Result<T> Fail<T>(string error) => new Result<T>(default!, false, error);
}

public class Result<T> : Result {
    public T Value { get; }
    
    protected internal Result(T value, bool success, string error)
        : base(success, error) {
        Value = value;
    }
}
