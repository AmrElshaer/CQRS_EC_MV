namespace Application.Command.Common;

public class EntityId : ValueObject
{
    public Guid Value { get; private set; }

    protected EntityId(Guid value)
    {
        Value = value;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
