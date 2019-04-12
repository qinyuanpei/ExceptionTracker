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
            if (loggingEvent.ExceptionObject == null)
                return null;

            var document = new BsonDocument();
            document.Add("message", new BsonString(loggingEvent.ExceptionObject.Message));
            document.Add("baseMessage", new BsonString(loggingEvent.ExceptionObject.GetBaseException().Message));
            document.Add("type", new BsonString(loggingEvent.ExceptionObject.GetType().ToString()));
            document.Add("hResult", new BsonInt32(loggingEvent.ExceptionObject.HResult));
            document.Add("source", new BsonString(loggingEvent.ExceptionObject.Source));
            document.Add("stackTrace", new BsonString(loggingEvent.ExceptionObject.StackTrace));
            var methodBase = loggingEvent.ExceptionObject.TargetSite;
            if (methodBase != null)
            {
                document.Add("methodName", new BsonString(methodBase.Name));
                var assembly = methodBase.Module.Assembly.GetName();
                document.Add("moduleName", new BsonString(assembly.Name));
                document.Add("moduleVersion", new BsonString(assembly.Version.ToString()));
            }

            return document;
        }
    }
}
