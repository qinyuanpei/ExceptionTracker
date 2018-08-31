using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Linq.Expressions;


namespace ExceptionTracker.Mongo.Repository
{
    public class MongoRepository<T> : IRepository<T> where T : class, new()
    {
        /// <summary>
        /// IMongoDatabase
        /// </summary>
        private readonly IMongoDatabase _database;

        /// <summary>
        /// IMonoCollection
        /// </summary>
        private readonly IMongoCollection<T> _collection;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="database">IMongoDatabase</param>
        public MongoRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = _database.GetCollection<T>(typeof(T).FullName);
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">T</param>
        /// <returns></returns>
        public T Add(T entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="exp">filter</param>
        /// <param name="entity">new value of T</param>
        /// <returns></returns>
        public bool Update(Expression<Func<T, bool>> exp, T entity)
        {
            var define = new ObjectUpdateDefinition<T>(entity);
            var result = _collection.UpdateOne(exp, define, null);
            return result.IsAcknowledged;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="exp">filter</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> exp)
        {
            var result = _collection.DeleteOne(exp);
            return result.IsAcknowledged;
        }

        /// <summary>
        /// Find
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public T Find(Expression<Predicate<T>> exp)
        {
            return default(T);
        }

        public IQueryable<T> FindAll(Expression<Predicate<T>> exp)
        {
            return null;
        }
    }
}
