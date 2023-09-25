#nullable disable

namespace Application.Shared.Models;

public class RabbitMqSettings
{
    public string Host { get; init; }

    public string UserName { get; init; }

    public string Password { get; init; }
}
