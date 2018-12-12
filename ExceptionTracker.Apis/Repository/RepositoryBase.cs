using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExceptionTracker.Apis.Repository
{
    public class RepositoryBase
    {
        /// <summary>
        /// IMongoDatabase
        /// </summary>
        private readonly IMongoDatabase database;

        public RepositoryBase(string mongoUrl, string dbName)
        {
            var client = new MongoClient(mongoUrl);
            this.database = client.GetDatabase(dbName);
        }

        public RepositoryBase(IMongoClient client, string dbName)
        {
            this.database = client.GetDatabase(dbName);
        }

        public RepositoryBase(IMongoDatabase database)
        {
            this.database = database;
        }

        public void Insert(string entityType, params BsonDocument[] documents)
        {
            var collection = database.GetCollection<BsonDocument>(entityType);
            collection.InsertMany(documents);
        }

        public Task InsertAsync(string entityType, params BsonDocument[] documents)
        {
            var collection = database.GetCollection<BsonDocument>(nameof(entityType));
            return collection.InsertManyAsync(documents);
        }

        /// <summary>
        /// 通过主键查找数据
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public BsonDocument GetById(string schema, string id)
        {
            var collection = database.GetCollection<BsonDocument>(schema);
            return collection.Find(new BsonDocument("_id",new ObjectId(id))).FirstOrDefault();;
        }
    }
}
