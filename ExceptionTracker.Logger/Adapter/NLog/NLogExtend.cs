using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using System.Runtime.InteropServices;
using System.Reflection;

namespace ExceptionTracker.Logger.Adapter.NLog
{
    internal static class NLogExtend
    {
        public static BsonDocument GetExceptionDocument(this LogEventInfo logEvent)
        {
            if (logEvent.Exception == null)
                return null;

            var document = new BsonDocument();
            document.Add("message", new BsonString(logEvent.Exception.Message));
            document.Add("source", new BsonString(logEvent.Exception.Source));
            document.Add("stackTrace", new BsonString(logEvent.Exception.StackTrace));
            var methodBase = logEvent.Exception.TargetSite;
            if (methodBase != null)
            {
                document.Add("fileName", new BsonString(string.Empty));
                document.Add("lineNumber", -1);
                document.Add("className", "");
                document.Add("methodName", new BsonString(methodBase.Name));
                var assembly = methodBase.Module.Assembly.GetName();
                document.Add("moduleName", new BsonString(assembly.Name));
                document.Add("moduleVersion", new BsonString(assembly.Version.ToString()));
            }

            return document;
        }
    }
}
