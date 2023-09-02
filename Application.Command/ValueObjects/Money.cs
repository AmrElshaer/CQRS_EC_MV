using Application.Command.Common;

namespace Application.Command.ValueObjects;

public class Money : ValueObject
{
    public Money(decimal amount, string currencyCode)
    {
        this.Amount = Argument.IsGreaterThan(amount, 0);
        this.CurrencyCode = Argument.IsNotEmpty(currencyCode);
    }

    public decimal Amount { get; private set; }

    public string CurrencyCode { get; private set; }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Amount;
        yield return CurrencyCode;
    }
}
