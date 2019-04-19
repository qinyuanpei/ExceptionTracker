using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using NLog;
using NLog.Targets;
using NLog.Common;
using MongoDB.Driver;
using NLog.Config;
using NLog.Layouts;

namespace ExceptionTracker.Logger.Adapter.NLog
{
    [Target("MongoAdapter")]
    public class NLogMongoAdapter : Target, IMongoDBAdapter<LogEventInfo>
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库集合名称
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// 是否包含基础信息
        /// </summary>
        public bool IncloudBaseInfo { get; set; } = true;

        /// <summary>
        /// 是否包含自定义字段
        /// </summary>
        public bool IncloudCustomFields { get; set; } = true;

        /// <summary>
        /// 是否包含事件属性
        /// </summary>
        public bool IncloudEventProperties { get; set; } = true;

        /// <summary>
        /// 可忽略的事件属性
        /// </summary>
        public string IgnoredEventProperties { get; set; }

        /// <summary>
        /// 自定义字段
        /// </summary>
        [ArrayParameter(typeof(MongoDBLayoutField), "field")]
        public IList<MongoDBLayoutField> CustomFields { get; private set; } = new List<MongoDBLayoutField>();

        /// <summary>
        /// 返回当前数据库
        /// </summary>
        /// <returns></returns>
        public IMongoDatabase GetDatabase()
        {
            var url = MongoUrl.Create(ConnectionString);
            var settings = MongoClientSettings.FromUrl(url);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(url.DatabaseName ?? "etlogs");
            return database;
        }

        protected override void Write(IList<AsyncLogEventInfo> logEvents)
        {
            var documents = logEvents.Select(logEvent => CreateBsonDocument(logEvent.LogEvent));
            var collection = GetDatabase().GetCollection<BsonDocument>(CollectionName);
            collection.InsertMany(documents);
        }

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            var document = CreateBsonDocument(logEvent.LogEvent);
            var collection = GetDatabase().GetCollection<BsonDocument>(CollectionName);
            collection.InsertOne(document);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var document = CreateBsonDocument(logEvent);
            var collection = GetDatabase().GetCollection<BsonDocument>(CollectionName);
            collection.InsertOne(document);
        }

        public BsonDocument CreateBsonDocument(LogEventInfo logEvent)
        {
            var document = new BsonDocument();

            if (IncloudBaseInfo && logEvent != null)
            {
                document.Add(nameof(logEvent.TimeStamp), logEvent.TimeStamp);
                document.Add(nameof(logEvent.Level), logEvent.Level.Name);
                document.Add("Message", logEvent.FormattedMessage);
                document.Add(nameof(logEvent.LoggerName), logEvent.LoggerName);
            };

            if (IncloudCustomFields && CustomFields.Any())
            {
                foreach (var field in CustomFields)
                {
                    var bsonValue = GetBsonValueWithField(logEvent, field);
                    if (bsonValue == null)
                        continue;

                    document.Add(field.Name, bsonValue);
                }
            }

            if (logEvent.Exception != null)
            {
                document.Add("Exception", new BsonDocument()
                {
                    {"Message",logEvent.Exception.Message },
                    {"Source",logEvent.Exception.Source },
                    {"StackTrace",logEvent.Exception.StackTrace.Trim()},

                });
            }

            if (IncloudEventProperties && logEvent.HasProperties)
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

            return document;
        }

        private BsonValue GetBsonValueWithLayout<T>(LogEventInfo logEvent, string layoutText)
        {
            var layout = Layout.FromString(layoutText);
            var value = RenderLogEvent(layout, logEvent);
            if (string.IsNullOrEmpty(value))
                return null;
            return BsonValueConverter.Convert<T>(value);
        }

        private BsonValue GetBsonValueWithField(LogEventInfo logEvent, MongoDBLayoutField field)
        {
            var type = field.BsonType;
            var layout = field.Layout;
            if (layout == null)
                return null;
            var value = RenderLogEvent(layout, logEvent);
            if (string.IsNullOrEmpty(value))
                return null;
            return BsonValueConverter.Convert(value, type);
        }
    }
}
