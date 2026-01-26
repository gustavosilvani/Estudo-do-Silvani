namespace Ecommerce.Domain.Common;

/// <summary>
/// Result Pattern - Evita exceções para fluxo de negócio
/// </summary>
public class Result
{
    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public string? ErrorMessage { get; private set; }
    public List<string> Errors { get; private set; } = new();

    protected Result(bool isSuccess, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        if (!string.IsNullOrEmpty(errorMessage))
        {
            Errors.Add(errorMessage);
        }
    }

    public static Result Ok() => new Result(true);
    public static Result Fail(string errorMessage) => new Result(false, errorMessage);
    public static Result Fail(List<string> errors) => new Result(false) { Errors = errors };

    public static Result<T> Ok<T>(T value) => new Result<T>(value, true);
    public static Result<T> Fail<T>(string errorMessage) => new Result<T>(default!, false, errorMessage);
}

public class Result<T> : Result
{
    public T Value { get; private set; }

    protected internal Result(T value, bool isSuccess, string? errorMessage = null)
        : base(isSuccess, errorMessage)
    {
        Value = value;
    }
}