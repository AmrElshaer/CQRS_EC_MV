using Application.Query.Common.Models;
using Application.Shared.QueryModels.Orders;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Application.Query.Infrastructure.Persistance;

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
