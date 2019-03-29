using System;
using System.Collections.Generic;
using System.Text;
using log4net.Core;
using MongoDB.Bson;
using System.Collections;
using log4net.Util;

namespace ExceptionTracker.Logger.Adapter.Log4Net
{
    internal static class Log4NetExtend
    {
        public static BsonDocument GetPropertiesDocument(this LoggingEvent loggingEvent)
        {
            if (loggingEvent.Properties == null) return null;

            var bsonDocument = new BsonDocument();
            foreach (PropertyEntry propertyEntry in loggingEvent.Properties)
            {
                bsonDocument.Add(propertyEntry.Key.ToString(), propertyEntry.Value.ToString());
            }

            return bsonDocument;
        }

        public static BsonDocument GetExceptionDocument(this LoggingEvent loggingEvent)
        {
            if (loggingEvent.ExceptionObject == null) return null;

            return new BsonDocument
            {
                {"soirce", loggingEvent.ExceptionObject.Source},
                {"message", loggingEvent.ExceptionObject.Message},
                {"stackTrace", loggingEvent.ExceptionObject.StackTrace}
            };
        }
    }
}
