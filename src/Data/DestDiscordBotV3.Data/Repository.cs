using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Data
{
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

        public Task<T> GetByExpression(Expression<Func<T, bool>> expression)
        {
            var list = _database.GetCollection<T>(_name);
            return list.Find(expression).FirstAsync();
        }

        public Task<List<T>> GetAllByExpression(Expression<Func<T, bool>> expression)
        {
            var list = _database.GetCollection<T>(_name);
            return list.Find(expression).ToListAsync();
        }

        public async Task Create(T obj)
        {
            var list = _database.GetCollection<T>(_name);
            await list.InsertOneAsync(obj);
        }

        public async Task Update<TId>(T obj, TId id)
        {
            var list = _database.GetCollection<T>(_name);
            await list.ReplaceOneAsync(
                Builders<T>.Filter.Eq("_id", id),
                obj,
                new UpdateOptions { IsUpsert = true });
        }

        public async Task Delete<TId>(TId id)
        {
            var list = _database.GetCollection<T>(_name);
            var filter = Builders<T>.Filter.Eq("_id", id);

            await list.DeleteOneAsync(filter);
        }

        public async Task Delete(Expression<Func<T, bool>> expression)
        {
            var list = _database.GetCollection<T>(_name);
            await list.DeleteOneAsync(expression);
        }

        public async Task DeleteMany(Expression<Func<T, bool>> expression)
        {
            var list = _database.GetCollection<T>(_name);
            await list.DeleteManyAsync(expression);
        }
    }
}