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
using log4net.Util;

namespace ExceptionTracker.Logger.Adapter.Log4Net
{

    public class Log4NetMongoAdapter : AppenderSkeleton, IMongoDBAdapter<LoggingEvent>
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
        public bool IncloudEventProperties { get; set; }

        /// <summary>
        /// 可忽略的事件属性
        /// </summary>
        public string IgnoredEventProperties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// 自定义字段
        /// </summary>
        public IList<MongoDBAppenderField> CustomFields = new List<MongoDBAppenderField>();

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

        public IMongoDatabase GetDatabase()
        {
            var url = MongoUrl.Create(ConnectionString);
            var settings = MongoClientSettings.FromUrl(url);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(url.DatabaseName ?? "etlogs");
            return database;
        }

        public BsonDocument CreateBsonDocument(LoggingEvent loggingEvent)
        {
            var document = new BsonDocument();

            //基础信息
            if (IncloudBaseInfo && loggingEvent != null)
            {
                document.Add("TimeStamp", loggingEvent.TimeStamp);
                document.Add("Level", loggingEvent.Level.ToString());
                document.Add("Message", loggingEvent.RenderedMessage);
                document.Add("LoggerName", loggingEvent.LoggerName);
            };

            //MessageObject
            var messageObject = loggingEvent.MessageObject;
            if (messageObject != null && !messageObject.GetType().IsValueType && messageObject.GetType().Name!="String")
            {
                var properties = messageObject.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(messageObject);
                    if (propertyValue != null)
                        document.Add(property.Name, propertyValue.ToString());
                }
            }

            //事件属性
            if (IncloudEventProperties && loggingEvent.Properties != null)
            {
                foreach (var propertyEntry in loggingEvent.Properties)
                {
                    var propertyKey = (propertyEntry as PropertyEntry).ToString();
                    var propertyValue = (propertyEntry as PropertyEntry).Value.ToString();
                    if (IgnoredEventProperties.Contains(propertyKey) || document.Contains(propertyKey))
                        continue;

                    document.Add(propertyKey, propertyValue);
                }
            }

            //自定义字段
            if (IncloudCustomFields && CustomFields.Any())
            {
                foreach (var layoutField in CustomFields)
                {
                    BsonValue bsonValue;
                    object fieldValue = layoutField.Layout.Format(loggingEvent);
                    if (!BsonTypeMapper.TryMapToBsonValue(fieldValue, out bsonValue))
                        bsonValue = fieldValue.ToBsonDocument();
                    document.Add(layoutField.Name, bsonValue);
                }
            }

            //异常堆栈
            if (loggingEvent.ExceptionObject != null)
            {
                document.Add("Exception", new BsonDocument()
                {
                    {"Message",loggingEvent.ExceptionObject.Message },
                    {"Source",loggingEvent.ExceptionObject.Source },
                    {"StackTrace",loggingEvent.ExceptionObject.StackTrace.Trim()},
                });
            }

            return document;
        }

        //private BsonValue GetBsonValueWithLayout(LoggingEvent logEvent, string layoutText)
        //{
        //    var layout = NEW(layoutText);
        //    var value = RenderLogEvent(layout, logEvent);
        //    if (string.IsNullOrEmpty(value))
        //        return null;
        //    BsonValue bsonValue;
        //    if (!BsonTypeMapper.TryMapToBsonValue(value, out bsonValue))
        //        bsonValue = bsonValue.ToBsonDocument();
        //    return bsonValue;
        //}

        //private BsonValue GetBsonValueWithField(LoggingEvent logEvent, MongoDBAppenderField field)
        //{
        //    var type = field.BsonType;
        //    var layout = field.Layout;
        //    if (layout == null)
        //        return null;
        //    var value = layout.Format(logEvent).ToString();
        //    if (string.IsNullOrEmpty(value))
        //        return null;
        //    BsonValue bsonValue;
        //    if (!BsonTypeMapper.TryMapToBsonValue(value, out bsonValue))
        //        bsonValue = bsonValue.ToBsonDocument();
        //    return bsonValue;
        //}
    }
}
