using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace ExceptionTracker.Logger.Adapter.NLog
{
    internal static class NLogExtend
    {
        public static BsonDocument GetPropertiesDocument(this LogEventInfo logEvent)
        {
            if (logEvent.Properties == null) return null;

            var bsonDocument = new BsonDocument();
            //foreach (PropertyEntry propertyEntry in logEvent.Properties)
            //{
            //    bsonDocument.Add(propertyEntry.Key.ToString(), propertyEntry.Value.ToString());
            //}

            return bsonDocument;
        }

        public static BsonDocument GetExceptionDocument(this LogEventInfo logEvent)
        {
            if (logEvent.Exception == null) return null;

            return new BsonDocument
            {
                {"source", logEvent.Exception.Source},
                {"message", logEvent.Exception.Message},
                {"stackTrace", logEvent.Exception.StackTrace}
            };
        }
    }
}
