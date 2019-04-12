using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using log4net.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;


namespace ExceptionTracker.Logger.Adapter.Log4Net
{

    public class MongoDBAppender : AppenderSkeleton, IMongoDBAdapter
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
        /// 是否使用基础信息
        /// </summary>
        public bool UseBaseInfo { get; set; } = true;

        /// <summary>
        /// 是否使用位置信息
        /// </summary>
        public bool UseLocationInfo { get; set; } = true;

        /// <summary>
        /// 是否使用消息对象
        /// </summary>
        public bool UseMessageObject { get; set; } = true;

        /// <summary>
        /// 是否使用异常信息
        /// </summary>
        public bool UseExceptionObject { get; set; } = true;


        /// <summary>
        /// 是否使用自定义属性
        /// </summary>
        public bool UseProperties { get; set; }

        /// <summary>
        /// 是否使用自定义字段
        /// </summary>
        public bool UseCustomFields { get; set; }

        /// <summary>
        /// Appender字段集合
        /// </summary>
        private readonly List<MongoDBAppenderField> _layoutFields = new List<MongoDBAppenderField>();

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="fields"></param>
        public void AddFields(params MongoDBAppenderField[] fields)
        {
            _layoutFields.AddRange(fields);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var document = CreateBsonDocument(loggingEvent);
            var collection = GetDatabase().GetCollection<BsonDocument>(CollectionName);
            collection.InsertOne(document);
        }

        protected override void Append(LoggingEvent[] loggingEvents)
        {
            var documents = loggingEvents.Select(l => CreateBsonDocument(l));
            var collection = GetDatabase().GetCollection<BsonDocument>(CollectionName);
            collection.InsertMany(documents);
        }

        private BsonDocument CreateBsonDocument(LoggingEvent loggingEvent)
        {
            var retDocument = new BsonDocument();

            if (UseBaseInfo && loggingEvent != null)
            {
                retDocument.Add("timestamp", loggingEvent.TimeStamp);
                retDocument.Add("level", loggingEvent.Level.ToString());
                retDocument.Add("thread", loggingEvent.ThreadName);
                if (!string.IsNullOrEmpty(loggingEvent.UserName) && loggingEvent.UserName != "NOT AVAILABLE")
                    retDocument.Add("userName", loggingEvent.UserName);
                retDocument.Add("message", loggingEvent.RenderedMessage);
                retDocument.Add("loggerName", loggingEvent.LoggerName);
                if (!string.IsNullOrEmpty(loggingEvent.Domain) && loggingEvent.Domain != "NOT AVAILABLE")
                    retDocument.Add("domain", loggingEvent.Domain);
                retDocument.Add("machineName", Environment.MachineName);
            };

            if (UseLocationInfo && loggingEvent.LocationInformation != null)
            {
                retDocument.Add("fileName", loggingEvent.LocationInformation.FileName);
                retDocument.Add("className", loggingEvent.LocationInformation.ClassName);
                retDocument.Add("lineNumber", loggingEvent.LocationInformation.LineNumber);
                retDocument.Add("methodName", loggingEvent.LocationInformation.MethodName);
            }

            if (UseMessageObject && loggingEvent.MessageObject != null)
            {
                var properties = loggingEvent.MessageObject.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(loggingEvent.MessageObject);
                    if (propertyValue != null)
                        retDocument.Add(property.Name, propertyValue.ToString());
                }
            }

            if (UseExceptionObject && loggingEvent.ExceptionObject != null)
            {
                retDocument.Add("exception", loggingEvent.GetExceptionDocument());
            }

            if (UseProperties && loggingEvent.Properties != null)
            {
                retDocument.Add("properties", loggingEvent.GetPropertiesDocument());
            }

            if (UseCustomFields && _layoutFields.Any())
            {
                foreach (var layoutField in _layoutFields)
                {
                    BsonValue bsonValue;
                    object fieldValue = layoutField.Layout.Format(loggingEvent);
                    if (!BsonTypeMapper.TryMapToBsonValue(fieldValue, out bsonValue))
                        bsonValue = fieldValue.ToBsonDocument();
                    retDocument.Add(layoutField.Name, bsonValue);
                }
            }

            return retDocument;
        }

        public IMongoDatabase GetDatabase()
        {
            var url = MongoUrl.Create(ConnectionString);
            var settings = MongoClientSettings.FromUrl(url);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(url.DatabaseName ?? "etlogs");
            return database;
        }
    }
}
