using Application.Common.models;
using Application.QueryModels.Orders;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Application.Infrastructure.Persistence;

public class ReadDbContext
{
    private readonly IMongoDbSettings _settings;
    private readonly IMongoDatabase _database;

    public ReadDbContext(IMongoDbSettings settings, IMongoClient mongoClient)
    {
        _settings = settings;
        _database = mongoClient.GetDatabase(settings.DatabaseName);
        Map();
    }

    internal IMongoCollection<OrderQueryModel> OrderMaterializedView => _database.GetCollection<OrderQueryModel>(_settings.OrderMaterializedViewCollection);

    private static void Map()
    {
        BsonClassMap.RegisterClassMap<OrderQueryModel>(cm =>
        {
            cm.AutoMap();
        });
    }
}
