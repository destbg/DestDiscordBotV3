using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Data.Extension
{
    public static class MongoExtension
    {
        public static IFindFluent<T, T> Where<T>(this IMongoCollection<T> collection, Expression<Func<T, bool>> expression) =>
            collection.Find(expression);

        public static IOrderedFindFluent<T, T> OrderByDescending<T>(this IFindFluent<T, T> collection, Expression<Func<T, object>> expression) =>
            collection.SortByDescending(expression);

        public static async Task<int> IndexOf<T>(this IOrderedFindFluent<T, T> collection, Func<T, bool> func)
        {
            var list = await collection.ToListAsync();
            for (var i = 0; i < list.Count; i++)
                if (func.Invoke(list[i]))
                    return i;
            return -1;
        }

        public static Task<List<T>> ToList<T>(this IFindFluent<T, T> collection) =>
            collection.ToListAsync();

        public static Task ForEach<T>(this IAsyncCursorSource<T> source, Func<T, Task> processor) =>
            source.ForEachAsync(processor);

        public static Task ForEach<T>(this IMongoCollection<T> source, Action<T> processor) =>
            source.Find(new BsonDocument()).ForEachAsync(processor);
    }
}