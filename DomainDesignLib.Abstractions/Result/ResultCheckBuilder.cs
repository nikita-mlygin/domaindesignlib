using System.ComponentModel.Design;

namespace DomainDesignLib.Abstractions.Result.Builder;

public class ResultCheckBuilder : CheckResultBuilder
{
    private readonly bool condition;
    private readonly Error error;

    protected internal ResultCheckBuilder(CheckResultBuilder? prev, bool condition, Error error)
        : base(prev)
    {
        this.condition = condition;
        this.error = error;
    }

    protected internal override Error? Test()
    {
        var testError = previous?.Test();

        if (testError is not null)
            return testError;

        return condition ? this.error : null;
    }
}

public class ResultCheckBuilder<TInput> : CheckResultBuilder
{
    protected internal ResultCheckBuilder(
        CheckResultBuilder? prev,
        TInput input,
        Func<TInput, Error?> condition
    )
        : base(prev)
    {
        this.input = input;
        this.condition = condition;
    }

    protected internal ResultCheckBuilder(
        CheckResultBuilder? prev,
        TInput input,
        Func<TInput, bool> condition,
        Error error
    )
        : base(prev)
    {
        this.input = input;
        this.condition = (input) => condition(input) ? error : null;
    }

    protected internal ResultCheckBuilder(CheckResultBuilder? prev, bool condition, Error error)
        : base(prev)
    {
        input = default!;
        this.condition = (input) => condition ? error : null;
    }

    private readonly TInput input;
    private readonly Func<TInput, Error?> condition;

    protected internal override Error? Test()
    {
        var error = previous?.Test();

        if (error is not null)
            return error;

        return condition(input);
    }
}

public class ResultCheckBuilderFunc : CheckResultBuilder
{
    private readonly Func<Error?> check;

    public ResultCheckBuilderFunc(CheckResultBuilder? prev, Func<Error?> check)
        : base(prev)
    {
        this.check = check;
    }

    protected internal override Error? Test()
    {
        var error = previous?.Test();

        if (error is not null)
            return error;

        return check();
    }
}

public class ResultCheckBuilderRes : CheckResultBuilder
{
    private readonly Func<Result> res;

    public ResultCheckBuilderRes(CheckResultBuilder? prev, Func<Result> res)
        : base(prev)
    {
        this.res = res;
    }

    protected internal override Error? Test()
    {
        var error = previous?.Test();

        if (error is not null)
            return error;

        var resV = res();

        return resV.IsFailure ? resV.Error : null;
    }
}
