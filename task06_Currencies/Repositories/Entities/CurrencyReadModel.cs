using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace task06_Currencies.Repositories.Entities
{
    public class CurrencyReadModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public string? Id { get; set; }

        public required string Symbol { get; init; }

        public string Name { get; init; } = string.Empty;

        public Dictionary<string, decimal> Rates { get; init; } = [];
    }
}
