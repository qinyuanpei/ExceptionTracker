using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Linq.Expressions;


namespace ExceptionTracker.Mongo.Repository
{
    public class MongoRepository<T> : IRepository<T> where T : class, new()
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = _database.GetCollection<T>（typeof(T).FullName);
        }

        public T Add(T entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }


        public bool Update(T entity)
        {
            var result = _collection.UpdateOne(entity);
            return entity;
        }

        public bool Delete(T entity)
        {
            
        }

        public T Find(Expression<Predicate<T>> exp)
        {

        }

        public IQueryable<T> FindAll(Expression<Predicate<T>> exp)
        {

        }
    }
}
