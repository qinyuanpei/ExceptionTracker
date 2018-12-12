using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExceptionTracker.Apis.Repository
{
    /// <summary>
    /// Monogodb Repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class MongoRepository<TEntity> : RepositoryBase, IRepository<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// IMongoDatabase
        /// </summary>
        private readonly IMongoDatabase database;

        public MongoRepository(string mongoUrl, string dbName) : base(mongoUrl, dbName) { }

        public MongoRepository(IMongoClient client, string dbName) : base(client, dbName) { }

        public MongoRepository(IMongoDatabase database) : base(database) { }

        public void Delete(TEntity entity)
        {
            var collection = database.GetCollection<TEntity>(nameof(TEntity));
            collection.DeleteOne(e => e.Id == entity.Id);
        }

        public Task DeleteAsync(TEntity entity)
        {
            var collection = database.GetCollection<TEntity>(nameof(TEntity));
            return collection.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return null;
        }

        public TEntity GetById(string id)
        {
            var collection = database.GetCollection<TEntity>(nameof(TEntity));
            return collection.Find(e => e.Id == new ObjectId(id)).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetByQuery(Expression<Func<TEntity, bool>> filter)
        {
            var collection = database.GetCollection<TEntity>(nameof(TEntity));
            return collection.Find(filter).ToList();
        }

        public void Insert(params TEntity[] entities)
        {
            var collection = database.GetCollection<TEntity>(nameof(TEntity));
            collection.InsertMany(entities);
        }

        public Task InsertAsync(params TEntity[] entities)
        {
            var collection = database.GetCollection<TEntity>(nameof(TEntity));
            return collection.InsertManyAsync(entities);
        }

        public void Update(TEntity entity)
        {
            //collection.UpdateOne(e=>e.Id == entity.Id)
        }

        public Task UpdateAsync(TEntity entity)
        {
            //throw new NotImplementedException();
            return null;
        }


    }
}
