using MongoDB.Driver;
using System.Text.Json.Serialization;

namespace Stushbr.Shared.DataAccess.MongoDb
{
    public class MongoDBConfig
    {
        public string? Endpoint { get; set; }

        [JsonIgnore]
        public string? CollectionName { get; set; }

        [JsonPropertyName("DatabaseName")]
        private string? _databaseName;

        [JsonIgnore]
        public string DatabaseName
        {
            get { return _databaseName ?? new MongoUrl(Endpoint).DatabaseName; }

            set { _databaseName = value; }
        }
    }
}
