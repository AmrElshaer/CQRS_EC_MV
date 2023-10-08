using Application.Command.Common;

namespace Application.Command.Features.Orders.ValueObjects;

public class Location : ValueObject
{
    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    private Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public static Result<Location> Create(double latitude, double longitude)
    {
        if (latitude <= 0)
        {
            return ValidationException.LessThanOrEqualZero(nameof(latitude));
        }

        if (longitude <= 0)
        {
            return ValidationException.LessThanOrEqualZero(nameof(longitude));
        }

        return new Location(latitude, longitude);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
