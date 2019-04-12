using System;
using System.Collections.Generic;
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
    public class MongoDBTarget : Target, IMongoDBAdapter
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
        /// 是否使用自定义字段
        /// </summary>
        public bool UseCustomFields { get; set; } = true;

        /// <summary>
        /// 是否使用异常信息
        /// </summary>
        public bool UseExceptionObject { get; set; } = true;

        /// <summary>
        /// 自定义字段
        /// </summary>
        [ArrayParameter(typeof(MongoDBLayoutField), "field")]
        public IList<MongoDBLayoutField> Fields { get; private set; }

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

            //基础日志信息
            if (UseBaseInfo && logEvent != null)
            {
                retDocument.Add("timestamp", logEvent.TimeStamp);
                retDocument.Add("level", logEvent.Level.Name);
                retDocument.Add("thread", GetBsonValueWithLayout<Int32>(logEvent, "${threadid}"));
                retDocument.Add("userName", GetBsonValueWithLayout<string>(logEvent, "${windows-identity}"));
                retDocument.Add("message", logEvent.FormattedMessage);
                retDocument.Add("loggerName", logEvent.LoggerName);
                retDocument.Add("domain", "NOT AVAILABLE");
                retDocument.Add("machineName", Environment.MachineName);
            };

            //自定义字段信息
            if (UseCustomFields && Fields.Any())
            {
                foreach (var field in Fields)
                {
                    var bsonValue = GetBsonValueWithField(logEvent, field);
                    if (bsonValue != null)
                        retDocument.Add(field.Name, bsonValue);
                }
            }

            //异常堆栈信息
            if (UseExceptionObject && logEvent.Exception != null)
            {
                retDocument.Add("exception", logEvent.GetExceptionDocument());
            }

            return retDocument;
        }

        private BsonValue GetBsonValueWithLayout<T>(LogEventInfo logEvent, string layoutText)
        {
            var layout = Layout.FromString(layoutText);
            var value = RenderLogEvent(layout, logEvent);
            if (string.IsNullOrEmpty(value))
                return null;
            return MongoValueConverter.Convert<T>(value);
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
            return MongoValueConverter.Convert(value, type);
        }
    }
}
