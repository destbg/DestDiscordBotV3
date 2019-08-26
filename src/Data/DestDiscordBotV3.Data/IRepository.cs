using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Data
{
    public interface IRepository<T>
    {
        Task Create(T obj);

        Task Delete(Guid id);

        Task DeleteMany(Expression<Func<T, bool>> expression);

        IMongoCollection<T> GetAll();

        Task<List<T>> GetAllToList();

        Task<T> GetById(Guid id);

        Task<T> GetByExpression(Expression<Func<T, bool>> expression);

        Task<List<T>> GetAllByExpression(Expression<Func<T, bool>> expression);

        Task Update(T obj, Guid id);
    }
}