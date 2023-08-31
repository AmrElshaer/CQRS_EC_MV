namespace Application.Common.models;

public class MongoDbSettings : IMongoDbSettings
{
    public string DatabaseName { get; init; } = default!;

    public string ConnectionString { get; init; } = default!;

    public string OrderMaterializedViewCollection { get; init; } = default!;

    public string OrderMaterializedView { get; init; } = default!;
}

public interface IMongoDbSettings
{
    public string DatabaseName { get; init; }

    public string ConnectionString { get; init; }

    public string OrderMaterializedViewCollection { get; init; }
}
