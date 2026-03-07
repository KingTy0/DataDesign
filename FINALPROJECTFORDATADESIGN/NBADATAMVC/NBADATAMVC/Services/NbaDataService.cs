using MongoDB.Bson;
using MongoDB.Driver;

public class NbaDataService
{
    private readonly IMongoDatabase _database;

    public NbaDataService(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("MongoDb"));
        _database = client.GetDatabase("NBAData");
    }

    public IMongoCollection<BsonDocument> Players => _database.GetCollection<BsonDocument>("players");
    public IMongoCollection<BsonDocument> Teams => _database.GetCollection<BsonDocument>("teams");
    public IMongoCollection<BsonDocument> PlayerStats => _database.GetCollection<BsonDocument>("player_stats");
    public IMongoCollection<BsonDocument> Games => _database.GetCollection<BsonDocument>("games");
}
