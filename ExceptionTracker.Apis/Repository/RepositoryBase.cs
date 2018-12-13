using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExceptionTracker.Apis.Models;
using ExceptionTracker.Apis.Utils;

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

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="schemaName">集合名称</param>
        /// <param name="documents">一个或多个文档</param>
        /// <returns></returns>
        public IEnumerable<BsonDocument> Insert(string schemaName, params BsonDocument[] documents)
        {
            var collection = database.GetCollection<BsonDocument>(schemaName);
            collection.InsertMany(documents);
            return documents;
        }

        /// <summary>
        /// 通过Id查找数据
        /// </summary>
        /// <param name="schemaName">集合名称</param>
        /// <param name="ids">一个或多个Id</param>
        /// <returns></returns>
        public IEnumerable<BsonDocument> GetById(string schemaName, params string[] ids)
        {
            var keys = ids.Select(id => new ObjectId(id)).ToList();
            var collection = database.GetCollection<BsonDocument>(schemaName);
            var findFiter = Builders<BsonDocument>.Filter.In("_id", keys);
            return collection.Find(findFiter).ToList();
        }

        /// <summary>
        /// 返回全部数据
        /// </summary>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        public IEnumerable<BsonDocument> GetAll(string schemaName)
        {
            var collection = database.GetCollection<BsonDocument>(schemaName);
            return collection.Find(new BsonDocument()).ToList();
        }

        /// <summary>
        /// 通过Id删除数据
        /// </summary>
        /// <param name="schemaName">集合名称</param>
        /// <param name="ids">一个或多个id</param>
        public int Delete(string schemaName, params string[] ids)
        {
            var keys = ids.Select(id => new ObjectId(id)).ToList();
            var collection = database.GetCollection<BsonDocument>(schemaName);
            var deleteFilter = Builders<BsonDocument>.Filter.In("_id", keys);
            var result = collection.DeleteOne(deleteFilter);
            return (int)result.DeletedCount;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="schemaName">集合名称</param>
        /// <param name="documents">一个或多个文档</param>
        /// <returns></returns>
        public IEnumerable<BsonDocument> Update(string schemaName, params BsonDocument[] documents)
        {
            var ids = documents.Select(e => e.GetValue("_id").ToString()).ToArray();
            var collection = database.GetCollection<BsonDocument>(schemaName);
            var updateOptions = new UpdateOptions() { IsUpsert = true };
            foreach (var document in documents)
            {
                var findFilter = Builders<BsonDocument>.Filter.Eq("_id", document.GetValue("_id"));
                var updateDefines = Builders<BsonDocument>.Update.Combine(document.BuildUpdateDefine(null));
                collection.UpdateOne(findFilter, updateDefines, updateOptions);
            }

            return GetById(schemaName, ids);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="schemaName">集合名称</param>
        /// <param name="id">Id</param>
        /// <param name="document">更新的文档</param>
        /// <returns></returns>
        public IEnumerable<BsonDocument> Update(string schemaName, string id, BsonDocument document)
        {
            var collection = database.GetCollection<BsonDocument>(schemaName);
            var updateOptions = new UpdateOptions() { IsUpsert = true };
            var findFilter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            var updateDefines = Builders<BsonDocument>.Update.Combine(document.BuildUpdateDefine(null));
            collection.UpdateOne(findFilter, updateDefines, updateOptions);
            return GetById(schemaName, id);
        }

        /// <summary>
        /// 复杂查询
        /// </summary>
        /// <param name="schemaName">集合名称</param>
        /// <param name="parameters">查询参数</param>
        /// <returns></returns>
        public IEnumerable<BsonDocument> GetByQuery(string schemaName, QueryParameter<BsonDocument> parameters)
        {
            var collection = database.GetCollection<BsonDocument>(schemaName);
            var sort
        }
    }
}
