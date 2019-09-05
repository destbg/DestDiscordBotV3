namespace DestDiscordBotV3.Data
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class Repository<T> : IRepository<T>
    {
        private readonly IMongoDatabase _database;
        private readonly string _name;

        public Repository(IMongoDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _name = typeof(T).Name;
        }

        public IMongoCollection<T> GetAll() =>
            _database.GetCollection<T>(_name);

        public Task<List<T>> GetAllToList() =>
            _database.GetCollection<T>(_name).Find(new BsonDocument()).ToListAsync();

        public Task<T> GetById<TId>(TId id)
        {
            var list = _database.GetCollection<T>(_name);
            var filter = Builders<T>.Filter.Eq("_id", id);
            return list.Find(filter).FirstAsync();
        }

        public Task<T> GetByCondition(Expression<Func<T, bool>> conditions)
        {
            var list = _database.GetCollection<T>(_name);
            return list.Find(conditions).FirstAsync();
        }

        public Task<List<T>> GetAllByCondition(Expression<Func<T, bool>> conditions)
        {
            var list = _database.GetCollection<T>(_name);
            return list.Find(conditions).ToListAsync();
        }

        public async Task Create(T item)
        {
            var list = _database.GetCollection<T>(_name);
            await list.InsertOneAsync(item);
        }

        public async Task Update<TId>(T item, TId id)
        {
            var list = _database.GetCollection<T>(_name);
            await list.ReplaceOneAsync(
                Builders<T>.Filter.Eq("_id", id),
                item,
                new UpdateOptions { IsUpsert = true });
        }

        public async Task Delete<TId>(TId id)
        {
            var list = _database.GetCollection<T>(_name);
            var filter = Builders<T>.Filter.Eq("_id", id);
            await list.DeleteOneAsync(filter);
        }

        public async Task Delete(Expression<Func<T, bool>> conditions)
        {
            var list = _database.GetCollection<T>(_name);
            await list.DeleteOneAsync(conditions);
        }

        public async Task DeleteMany(Expression<Func<T, bool>> conditions)
        {
            var list = _database.GetCollection<T>(_name);
            await list.DeleteManyAsync(conditions);
        }
    }
}