using System.Diagnostics.CodeAnalysis;

namespace JwtAuth.Utilities;

public class Result<T> : Result
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    [MemberNotNullWhen(true, nameof(Data))]
    public new bool IsSuccessful => base.IsSuccessful;
    public new string? ErrorMessage => base.ErrorMessage;
    public T? Data { get; }

    private Result(bool isSuccessful, T? value, string? errorMessage) : base(isSuccessful, errorMessage)
    {
        Data = value;
    }

    public static Result<T> Success(T value) =>
        new Result<T>(true, value, null);

    public new static Result<T> Failure(string errorMessage) =>
        new Result<T>(false, default, errorMessage);

    public Result<U> ToResult<U>()
    {
        if(!this.IsSuccessful)
            return Result<U>.Failure(this.ErrorMessage);

        throw new InvalidOperationException("ToResult<U> può essere usato solo su un risultato fallito.");
    }
}

public class Result
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public bool IsSuccessful { get; }
    public string? ErrorMessage { get; }

    public Result(bool isSuccessful, string? errorMessage)
    {
        IsSuccessful = isSuccessful;
        ErrorMessage = errorMessage;
    }

    public static Result Success() =>
        new Result(true, null);

    public static Result Failure(string errorMessage) =>
        new Result(false, errorMessage);
}
