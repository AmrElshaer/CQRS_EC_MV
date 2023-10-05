namespace Application.Command.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public static NotFoundException ForCustomer(Guid customerId)
    {
        return new NotFoundException($"Customer with id {customerId} not found");
    }
}
