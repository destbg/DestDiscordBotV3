using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DestDiscordBotV3.Data
{
    public interface IRepository<T>
    {
        Task Create(T obj);
        Task Delete(Expression<Func<T, bool>> expression);
        Task Delete<TId>(TId id);
        Task DeleteMany(Expression<Func<T, bool>> expression);
        IMongoCollection<T> GetAll();
        Task<List<T>> GetAllByExpression(Expression<Func<T, bool>> expression);
        Task<List<T>> GetAllToList();
        Task<T> GetByExpression(Expression<Func<T, bool>> expression);
        Task<T> GetById<TId>(TId id);
        Task Update<TId>(T obj, TId id);
    }
}