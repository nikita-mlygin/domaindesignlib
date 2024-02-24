using DomainDesignLib.Abstractions.Result.Builder;

namespace DomainDesignLib.Abstractions.Result;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<T> Success<T>(T value) => new(true, Error.None, value);

    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Failure<T>(Error error) => new(false, error, default!);

    public static ResultCheckBuilder<TInput> Check<TInput>(TInput input, Func<TInput, Error?> func)
    {
        return new ResultCheckBuilder<TInput>(null, input, func);
    }

    public static ResultCheckBuilder<TInput> Check<TInput>(
        TInput input,
        Func<TInput, bool> func,
        Error error
    )
    {
        return new ResultCheckBuilder<TInput>(null, input, func, error);
    }

    public static ResultCheckBuilder Check(bool condition, Error error)
    {
        return new ResultCheckBuilder(null, condition, error);
    }

    public static CheckResultBuilder Check(Func<Error?> check)
    {
        return new ResultCheckBuilderFunc(null, check);
    }

    public static CheckResultBuilder Check(Func<Result> res)
    {
        return new ResultCheckBuilderRes(null, res);
    }
}
