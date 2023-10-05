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

   
    public static Result Combine(params Result[] results)
    {
        foreach (var result in results)
        {
            if (result.Failure)
                return result;
        }

        return Ok();
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
}
