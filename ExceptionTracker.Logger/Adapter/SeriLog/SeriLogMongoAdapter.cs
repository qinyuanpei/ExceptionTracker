using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Linq;

namespace ExceptionTracker.Logger.Adapter.SeriLog
{
    public class SeriLogMongoAdapter : ILogEventSink, IMongoDBAdapter<LogEvent>
    {
        public string ConnectionString { get; set; }
        public string CollectionName { get; set; }
        public bool IncloudBaseInfo { get; set; }
        public bool IncloudCustomFields { get; set; }
        public bool IncloudEventProperties { get; set; }
        public string IgnoredEventProperties { get; set; }
        public bool IsCappedCollection { get; set; }
        public long? CappedCollectionSize { get; set; }
        public long? CappedCollectionMaxItems { get; set; }

        private readonly IFormatProvider _formatProvider;
        public SeriLogMongoAdapter(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            var document = CreateBsonDocument(logEvent);
            var collection = GetDatabase().GetCollection<BsonDocument>(CollectionName);
            collection.InsertOne(document);
        }

        public IMongoDatabase GetDatabase()
        {
            var url = MongoUrl.Create(ConnectionString);
            var settings = MongoClientSettings.FromUrl(url);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(url.DatabaseName ?? "etlogs");
            return database;
        }

        public BsonDocument CreateBsonDocument(LogEvent logEvent)
        {
            var document = new BsonDocument();

            //基础信息
            if (IncloudBaseInfo && logEvent != null)
            {
                document.Add("TimeStamp", logEvent.Timestamp.DateTime);
                document.Add("Level", logEvent.Level);
                var message = logEvent.RenderMessage(_formatProvider);
                document.Add("Message", message);
                document.Add("LoggerName", string.Empty);
            };

            //事件属性
            if (IncloudEventProperties && logEvent.Properties.Count > 0)
            {
                foreach (var property in logEvent.Properties)
                {
                    var propertyKey = property.Key.ToString();
                    var propertyValue = property.Value.ToString();
                    if (IgnoredEventProperties.Contains(propertyKey) || document.Contains(propertyKey))
                        continue;

                    document.Add(propertyKey, propertyValue);
                }
            }

            //异常堆栈
            if (logEvent.Exception != null)
            {
                document.Add("Exception", new BsonDocument()
                {
                    {"Message",logEvent.Exception.Message },
                    {"Source",logEvent.Exception.Source },
                    {"StackTrace",logEvent.Exception.StackTrace.Trim()},
                });
            }

            return document;
        }

        public void EnsureCollectionExists(string collectionName)
        {
            var database = GetDatabase();
            if (IsCollectionExists(collectionName))
                return;

            var options = new CreateCollectionOptions
            {
                Capped = IsCappedCollection,
                MaxSize = CappedCollectionSize,
                MaxDocuments = CappedCollectionMaxItems
            };

            database.CreateCollection(collectionName, options);
        }

        public bool IsCollectionExists(string collectionName)
        {
            var database = GetDatabase();
            var listOptions = new ListCollectionsOptions
            {
                Filter = Builders<BsonDocument>.Filter.Eq("name", collectionName)
            };

            return database.ListCollections(listOptions).ToEnumerable().Any();
        }
    }
}
