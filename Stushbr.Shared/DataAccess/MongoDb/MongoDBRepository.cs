using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Stushbr.Shared.DataAccess.MongoDb
{
    public abstract class MongoDbRepository<T> : IRepository<T> where T : class, IIdentifier
    {
        private const int MaxConcurrentOperations = 50;
        private readonly TimeSpan _semaphoreTimeout = TimeSpan.FromSeconds(35);
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(MaxConcurrentOperations, MaxConcurrentOperations);

        private IMongoDatabase? _database;
        private MongoClient _client;
        private readonly MongoDBConfig _config;
        private ILogger _logger;

        protected IMongoCollection<T> Collection;
        protected IMongoCollection<BsonDocument>? BsonCollection;

        protected abstract string CollectionName { get; }

        static MongoDbRepository()
        {
            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreExtraElementsConvention(true)
            };

            ConventionRegistry.Register("SAB_MongoConvensions", pack, _ => true);
        }

        public MongoDbRepository(
            MongoClient client,
            MongoDBConfig config,
            ILogger logger)
        {
            _logger = logger;
            _client = client;
            _config = config;

            Initialize();
        }

        public async Task CreateItemAsync(T item)
        {
            var locked = await Lock();

            try
            {
                await Collection.InsertOneAsync(item);
            }
            catch (MongoWriteException exception) when (exception.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new Exception($"{typeof(T).Name} with the same id already exists");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                Release(locked);
            }
        }

        public async Task CreateItemsAsync(IEnumerable<T> items)
        {
            List<T> itemsList = items.ToList();
            if (!itemsList.Any())
            {
                return;
            }

            var locked = await Lock();

            try
            {
                if (itemsList.Any())
                {
                    await Collection.InsertManyAsync(itemsList);
                }
            }
            catch (MongoBulkWriteException exception) when (exception.WriteErrors[0].Category == ServerErrorCategory.DuplicateKey)
            {
                throw new Exception($"{typeof(T).Name} with the same id already exists");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                Release(locked);
            }
        }

        public async Task DeleteItemAsync(string id)
        {
            var locked = await Lock();
            try
            {
                var res = await Collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));

                if (res.DeletedCount == 0)
                {
                    throw new Exception($"{typeof(T).Name} with id {id} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                Release(locked);
            }
        }

        public async Task DeleteItemsAsync(IEnumerable<string> ids)
        {
            var locked = await Lock();
            try
            {
                await Collection.DeleteManyAsync(Builders<T>.Filter.In("_id", ids));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                Release(locked);
            }
        }

        public async Task<IList<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            var locked = await Lock();

            try
            {
                var query = await Collection.FindAsync(predicate);
                var res = await query.ToListAsync();

                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                Release(locked);
            }
        }

        public async Task<IList<T>> GetAllItemsAsync()
        {
            var locked = await Lock();

            try
            {
                var query = await Collection.FindAsync(x => true);
                var res = await query.ToListAsync();

                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                Release(locked);
            }
        }

        public async Task<T> GetItemByIdAsync(string id)
        {
            var locked = await Lock();

            try
            {
                var query = await Collection.FindAsync(x => x.Id == id);
                var res = await query.FirstOrDefaultAsync();

                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                Release(locked);
            }
        }

        public async Task UpdateItemAsync(string id, T item)
        {
            var locked = await Lock();

            try
            {
                var res = await Collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), item);

                if (res.MatchedCount == 0)
                {
                    throw new Exception($"{typeof(T).Name} with id {id} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                Release(locked);
            }
        }

        protected virtual IEnumerable<CreateIndexModel<T>> GetCustomIndexes()
        {
            return new List<CreateIndexModel<T>>();
        }

        private void Initialize()
        {
            _database = _client.GetDatabase(_config.DatabaseName);
            Collection = _database.GetCollection<T>(CollectionName);
            BsonCollection = _database.GetCollection<BsonDocument>(CollectionName);

            var customIndexes = GetCustomIndexes();
            foreach (var index in customIndexes)
                Collection.Indexes.CreateOne(index);
        }

        private async Task<bool> Lock()
        {
            var locked = await _semaphore.WaitAsync(_semaphoreTimeout);
            if (!locked)
            {
                _logger.LogWarning($"[{GetType().Name}] Failed to get semaphore after timeout {_semaphoreTimeout}");
            }

            return locked;
        }

        private void Release(bool locked)
        {
            if (locked)
            {
                _semaphore.Release();
            }
        }
    }
}
