namespace Application.Command.Common.Exceptions;

public class ValidationException: Exception
{
    public ValidationException(string message) : base(message) { }

    public static ValidationException LessThanOrEqualZero(string arg)
    {
        return new ValidationException($"{arg}  must be greater than zero");
    }
}
