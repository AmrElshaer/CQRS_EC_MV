using Application.Command.Common;

namespace Application.Command.ValueObjects;

public class Address : ValueObject
{
    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    private Address(double latitude, double longitude)
    {
        this.Latitude = Argument.IsGreaterThan(latitude, 0);
        this.Longitude = Argument.IsGreaterThan(longitude, 0);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }

    public static Address Create(double latitude, double longitude)
    {
        return new Address(latitude, longitude);
    }
}
