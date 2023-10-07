#nullable disable
using System.Diagnostics.CodeAnalysis;

namespace Application.Command.Common;

public class Result
{
    public bool Success { get; private set; }

    public Exception Error { get; private set; }

    public bool Failure => !Success;

    protected Result(bool success, Exception error)
    {
        Success = success;
        Error = error;
    }

    public static Result Fail(Exception exception)
    {
        return new Result(false, exception);
    }

    public static Result<T> Fail<T>(Exception ex)
    {
        return new Result<T>(default(T), false, ex);
    }

    public static Result Ok()
    {
        return new Result(true, null);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true, null);
    }
}

public class Result<T> : Result
{
    public T Value { get; private set; }

    internal protected Result([AllowNull] T value, bool success, Exception error)
        : base(success, error)
    {
        Value = value;
    }

    public static implicit operator Result<T>(T value) => Ok(value);
    public static implicit operator Result<T>(Exception error) => Fail<T>(error);

    public R Match<R>(Func<T, R> succ, Func<Exception, R> fail) =>
        Failure
            ? fail(Error)
            : succ(Value);

    public async Task<R> MatchAsync<R>(Func<T, Task<R>> succ, Func<Exception, R> fail) =>
        Failure
            ? fail(Error)
            : await succ(Value);
}
