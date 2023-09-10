using Application.Command.Common;

namespace Application.Command.ValueObjects;

public class Money : ValueObject
{
    private Money() { }

    public Money(decimal amount, string currencyCode)
    {
        this.Amount = Argument.IsGreaterThan(amount, 0);
        this.CurrencyCode = Argument.IsNotEmpty(currencyCode);
    }

    public decimal Amount { get; private set; } = default!;

    public string CurrencyCode { get; private set; } = default!;

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Amount;
        yield return CurrencyCode;
    }
}
