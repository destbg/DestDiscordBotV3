namespace DestDiscordBotV3.Data
{
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IRepository{T}" />
    /// </summary>
    /// <typeparam name="T">The model</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Adds an item <see cref="T"/> to the table
        /// </summary>
        Task Create(T item);

        /// <summary>
        /// Deletes an item <see cref="T"/> from the table by the specified conditions
        /// </summary>
        Task Delete(Expression<Func<T, bool>> conditions);

        /// <summary>
        /// Delete an item <see cref="T"/> from the table by id <see cref="TId"/>
        /// </summary>
        /// <typeparam name="TId">The type of id</typeparam>
        Task Delete<TId>(TId id);

        /// <summary>
        /// Deletes many items <see cref="T"/> from the table by the specified conditions
        /// </summary>
        Task DeleteMany(Expression<Func<T, bool>> conditions);

        /// <summary>
        /// Gets all of the items <see cref="T"/> in the table before they processed
        /// </summary>
        IMongoCollection<T> GetAll();

        /// <summary>
        /// Get all items <see cref="T"/> from the table by the specified conditions
        /// </summary>
        Task<List<T>> GetAllByCondition(Expression<Func<T, bool>> conditions);

        /// <summary>
        /// Get all of the items <see cref="T"/> in the table
        /// </summary>
        Task<List<T>> GetAllToList();

        /// <summary>
        /// Get an item from <see cref="T"/> the table by the specified conditions
        /// </summary>
        Task<T> GetByCondition(Expression<Func<T, bool>> conditions);

        /// <summary>
        /// Get a single item <see cref="T"/> from the table by it's identifier
        /// </summary>
        /// <typeparam name="TId">The type of id</typeparam>
        Task<T> GetById<TId>(TId id);

        /// <summary>
        /// Updates an item <see cref="T"/> from the table by the specified id <see cref="TId"/>
        /// </summary>
        /// <typeparam name="TId">The type of id</typeparam>
        Task Update<TId>(T item, TId id);
    }
}