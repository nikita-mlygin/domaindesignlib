namespace DomainDesignLib.Abstractions;

public static class NullableExtensions
{
    public static TResult Match<TInput, TResult>(
        this TInput? input,
        Func<TInput, TResult> notNullFunc,
        Func<TResult> nullFunc
    )
    {
        return (input is not null) ? notNullFunc(input) : nullFunc();
    }

    public static async Task<TResult> Match<TInput, TResult>(
        this Task<TInput?> inputTask,
        Func<TInput, Task<TResult>> notNullFunc,
        Func<Task<TResult>> nullFunc
    )
        where TInput : notnull
    {
        var input = await inputTask;
        return input is not null ? await notNullFunc(input) : await nullFunc();
    }
}
