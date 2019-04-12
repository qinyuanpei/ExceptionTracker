using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using NLog;
using NLog.Targets;
using NLog.Common;
using MongoDB.Driver;

namespace ExceptionTracker.Logger.Adapter.NLog
{
    public class MongoDBTarget : Target,IMongoDBAdapter
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库集合名称
        /// </summary>
        public string CollectionName { get; set; }

        public bool UseBaseInfo { get; set; }

        public IMongoDatabase GetDatabase()
        {
            var url = MongoUrl.Create(ConnectionString);
            var settings = MongoClientSettings.FromUrl(url);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(url.DatabaseName ?? "etlog");
            return database;
        }

        protected override void Write(IList<AsyncLogEventInfo> logEvents)
        {

        }

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            
        }

        protected override void Write(LogEventInfo logEvent)
        {
            
        }

        private BsonDocument CreateBsonDocument(LogEventInfo logEvent)
        {
            var retDocument = new BsonDocument();

            if (UseBaseInfo && logEvent != null)
            {
                retDocument.Add("timestamp", logEvent.TimeStamp);
                retDocument.Add("level", logEvent.Level.Name);
                //retDocument.Add("thread", logEvent.StackTrace.GetFrame().);
                //retDocument.Add("userName", loggingEvent.UserName);
                //retDocument.Add("message", loggingEvent.RenderedMessage);
                //retDocument.Add("loggerName", loggingEvent.LoggerName);
                //retDocument.Add("domain", loggingEvent.Domain);
                //retDocument.Add("machineName", Environment.MachineName);
            };

            //if (UseLocationInfo && loggingEvent.LocationInformation != null)
            //{
            //    retDocument.Add("fileName", loggingEvent.LocationInformation.FileName);
            //    retDocument.Add("className", loggingEvent.LocationInformation.ClassName);
            //    retDocument.Add("lineNumber", loggingEvent.LocationInformation.LineNumber);
            //    retDocument.Add("methodName", loggingEvent.LocationInformation.MethodName);
            //}

            //if (UseMessageObject && loggingEvent.MessageObject != null)
            //{
            //    var properties = loggingEvent.MessageObject.GetType().GetProperties();
            //    foreach (var property in properties)
            //    {
            //        var propertyValue = property.GetValue(loggingEvent.MessageObject);
            //        if (propertyValue != null)
            //            retDocument.Add(property.Name, propertyValue.ToString());
            //    }
            //}

            //if (UseExceptionObject && loggingEvent.ExceptionObject != null)
            //{
            //    retDocument.Add("innerException", loggingEvent.GetExceptionDocument());
            //}

            //if (UseProperties && loggingEvent.Properties != null)
            //{
            //    retDocument.Add("properties", loggingEvent.GetPropertiesDocument());
            //}

            //if (_layoutFields.Any())
            //{
            //    foreach (var layoutField in _layoutFields)
            //    {
            //        BsonValue bsonValue;
            //        object fieldValue = layoutField.Layout.Format(loggingEvent);
            //        if (!BsonTypeMapper.TryMapToBsonValue(fieldValue, out bsonValue))
            //            bsonValue = fieldValue.ToBsonDocument();
            //        retDocument.Add(layoutField.Name, bsonValue);
            //    }
            //}

            return retDocument;
        }
    }
}
