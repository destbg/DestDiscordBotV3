namespace DestDiscordBotV3.Data.Extension
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Linq like expressions />
    /// </summary>
    public static class MongoExtension
    {
        /// <summary>
        /// Filters a sequence of values based on a predicate
        /// </summary>
        public static IFindFluent<T, T> Where<T>(this IMongoCollection<T> collection, Expression<Func<T, bool>> expression) =>
            collection.Find(expression);

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key
        /// </summary>
        public static IOrderedFindFluent<T, T> OrderByDescending<T>(this IFindFluent<T, T> collection, Expression<Func<T, object>> expression) =>
            collection.SortByDescending(expression);

        /// <summary>
        /// Gets the position of an element from a sequence based on a key
        /// </summary>
        public static async Task<int> IndexOf<T>(this IOrderedFindFluent<T, T> collection, Func<T, bool> func)
        {
            var list = await collection.ToListAsync();
            for (var i = 0; i < list.Count; i++)
                if (func.Invoke(list[i]))
                    return i;
            return -1;
        }

        /// <summary>
        /// Converts a sequence into a list
        /// </summary>
        public static Task<List<T>> ToList<T>(this IFindFluent<T, T> collection) =>
            collection.ToListAsync();

        /// <summary>
        /// Goes thru each element in a sequence and applies processor
        /// </summary>
        public static Task ForEach<T>(this IAsyncCursorSource<T> source, Func<T, Task> processor) =>
            source.ForEachAsync(processor);

        /// <summary>
        /// Goes thru each element in a sequence and applies processor
        /// </summary>
        public static Task ForEach<T>(this IMongoCollection<T> source, Action<T> processor) =>
            source.Find(new BsonDocument()).ForEachAsync(processor);
    }
}