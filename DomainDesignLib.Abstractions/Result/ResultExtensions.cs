namespace DomainDesignLib.Abstractions.Result;

public static class ResultExtensions
{
    public static T Match<T>(this Result result, Func<T> onSuccess, Func<Error, T> onFailure) =>
        result.IsSuccess ? onSuccess() : onFailure(result.Error);

    public static TResult Match<TResult, T>(
        this Result<T> result,
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onFailure
    ) => result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);

    public static async Task<T> Match<T>(
        this Task<Result<T>> resultTask,
        Func<T> onSuccess,
        Func<Error, T> onFailure
    )
    {
        var res = await resultTask;

        return res.IsSuccess ? onSuccess() : onFailure(res.Error);
    }

    public static async Task<TResult> Match<TResult, T>(
        this Task<Result<T>> resultTask,
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onFailure
    )
    {
        var res = await resultTask;

        return res.IsSuccess ? onSuccess(res.Value) : onFailure(res.Error);
    }

    public static async Task<TResult> Match<TResult, T>(
        this SuccessResultBuilder<Task<Result<T>>> resultTask,
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onFailure
    )
    {
        var res = await ((Task<Result<T>>)resultTask ?? throw new InvalidCastException());

        return res.IsSuccess ? onSuccess(res.Value) : onFailure(res.Error);
    }

    public static async Task<T> Match<T>(
        this SuccessResultBuilder<Task<Result>> resultTask,
        Func<T> onSuccess,
        Func<Error, T> onFailure
    )
    {
        Task<Result> resTask = resultTask;
        var res = await resTask;

        return res.IsSuccess ? onSuccess() : onFailure(res.Error);
    }
}
