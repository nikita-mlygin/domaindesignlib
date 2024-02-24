using DomainDesignLib.Abstractions.Result.Builder;

namespace DomainDesignLib.Abstractions.Result;

public class Result<T> : Result
{
    protected internal Result(bool isFailure, Error error, T value)
        : base(isFailure, error) => this.value = value;

    private readonly T value;

    public T Value =>
        IsSuccess
            ? value
            : throw new InvalidOperationException("Cannot assign to value of failure result.");

    public static implicit operator Result<T>(T value) => Success(value);
}
