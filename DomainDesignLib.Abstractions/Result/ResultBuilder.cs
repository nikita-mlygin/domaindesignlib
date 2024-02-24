namespace DomainDesignLib.Abstractions.Result;

using DomainDesignLib.Abstractions;
using DomainDesignLib.Abstractions.Result;
using DomainDesignLib.Abstractions.Result.Builder;

public class SuccessResultBuilder<T>(
    Func<T> onSuccess,
    Func<Error, T> onFailure,
    CheckResultBuilder checkBuilder
)
{
    public Func<T> OnSuccess { get; } = onSuccess;
    public Func<Error, T> OnFailure { get; } = onFailure;
    public CheckResultBuilder CheckBuilder { get; } = checkBuilder;

    public T Build()
    {
        return CheckBuilder.Test().Match(err => OnFailure(err), () => OnSuccess());
    }

    public static implicit operator T(SuccessResultBuilder<T> successResultBuilder)
    {
        return successResultBuilder.Build();
    }
}

public static class SuccessResultBuilderExtensions
{
    public static SuccessResultBuilder<Task<T>> After<T>(
        this SuccessResultBuilder<Task<T>> builder,
        Func<Task> afterSuccess
    )
    {
        return new SuccessResultBuilder<Task<T>>(
            () =>
            {
                async Task<T> res()
                {
                    var res = await builder.OnSuccess();
                    await afterSuccess();

                    return res;
                }

                return res();
            },
            builder.OnFailure,
            builder.CheckBuilder
        );
    }

    public static SuccessResultBuilder<Task<T>> After<T>(
        this SuccessResultBuilder<T> builder,
        Func<Task> afterSuccess
    )
    {
        return new SuccessResultBuilder<Task<T>>(
            () =>
            {
                async Task<T> res()
                {
                    var res = builder.OnSuccess();
                    await afterSuccess();
                    return res;
                }

                return res();
            },
            (err) => Task.FromResult(builder.OnFailure(err)),
            builder.CheckBuilder
        );
    }

    public static SuccessResultBuilder<Task<T>> After<T>(
        this SuccessResultBuilder<Task<T>> builder,
        Action afterSuccess
    )
    {
        return new SuccessResultBuilder<Task<T>>(
            () =>
            {
                async Task<T> res()
                {
                    var res = await builder.OnSuccess();
                    afterSuccess();
                    return res;
                }

                return res();
            },
            builder.OnFailure,
            builder.CheckBuilder
        );
    }

    public static SuccessResultBuilder<T> After<T>(
        this SuccessResultBuilder<T> builder,
        Action afterSuccess
    )
    {
        return new SuccessResultBuilder<T>(
            () =>
            {
                var res = builder.OnSuccess();
                afterSuccess();
                return res;
            },
            builder.OnFailure,
            builder.CheckBuilder
        );
    }
}

public class CheckResultBuilder(CheckResultBuilder? prev)
{
    protected CheckResultBuilder? previous = prev;

    protected internal virtual Error? Test()
    {
        var error = previous?.Test();

        if (error is not null)
        {
            return error;
        }

        return null;
    }

    public static implicit operator Result(CheckResultBuilder builder)
    {
        return builder.Test().Match(Result.Failure, Result.Success);
    }
}

public static class CheckResultBuilderExtensions
{
    public static ResultCheckBuilder<TInput> Check<TResultBuilder, TInput>(
        this TResultBuilder resultBuilder,
        Func<TInput, Error?> func,
        TInput input
    )
        where TResultBuilder : CheckResultBuilder
    {
        return new ResultCheckBuilder<TInput>(resultBuilder, input, func);
    }

    public static CheckResultBuilder Check(
        this CheckResultBuilder resultBuilder,
        Func<Error?> check
    )
    {
        return new ResultCheckBuilderFunc(resultBuilder, check);
    }

    public static CheckResultBuilder Check(this CheckResultBuilder resultBuilder, Func<Result> res)
    {
        return new ResultCheckBuilderRes(resultBuilder, res);
    }

    public static ResultCheckBuilder<TInput> Check<TInput>(
        this CheckResultBuilder resultBuilder,
        TInput input,
        Func<TInput, Error?> func
    )
    {
        return new ResultCheckBuilder<TInput>(resultBuilder, input, func);
    }

    public static ResultCheckBuilder<TInput> Check<TInput>(
        this CheckResultBuilder resultBuilder,
        TInput input,
        Func<TInput, bool> func,
        Error error
    )
    {
        return new ResultCheckBuilder<TInput>(resultBuilder, input, func, error);
    }

    public static ResultCheckBuilder Check(
        this CheckResultBuilder resultBuilder,
        bool condition,
        Error error
    )
    {
        return new ResultCheckBuilder(resultBuilder, condition, error);
    }

    public static SuccessResultBuilder<Result> Success(this CheckResultBuilder checkResultBuilder)
    {
        return new SuccessResultBuilder<Result>(Result.Success, (err) => err, checkResultBuilder);
    }

    public static SuccessResultBuilder<Result> Success(
        this CheckResultBuilder checkResultBuilder,
        Action action
    )
    {
        return new SuccessResultBuilder<Result>(
            () =>
            {
                action();
                return Result.Success();
            },
            (err) => err,
            checkResultBuilder
        );
    }

    public static SuccessResultBuilder<Result<TRes>> Success<TRes>(
        this CheckResultBuilder checkResultBuilder,
        Func<TRes> getRes
    )
    {
        return new SuccessResultBuilder<Result<TRes>>(
            () => Result.Success(getRes()),
            Result.Failure<TRes>,
            checkResultBuilder
        );
    }

    public static SuccessResultBuilder<Task<Result<TRes>>> Success<TRes>(
        this CheckResultBuilder checkResultBuilder,
        Func<Task<TRes>> getRes
    )
    {
        return new SuccessResultBuilder<Task<Result<TRes>>>(
            async () => Result.Success(await getRes()),
            (err) => Task.FromResult(Result.Failure<TRes>(err)),
            checkResultBuilder
        );
    }

    public static SuccessResultBuilder<Task<Result>> Success(
        this CheckResultBuilder checkResultBuilder,
        Func<Task> getRes
    )
    {
        return new SuccessResultBuilder<Task<Result>>(
            async () =>
            {
                await getRes();
                return Result.Success();
            },
            (err) => Task.FromResult(Result.Failure(err)),
            checkResultBuilder
        );
    }
}
