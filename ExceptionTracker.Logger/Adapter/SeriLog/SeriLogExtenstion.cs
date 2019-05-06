using Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionTracker.Logger.Adapter.SeriLog
{
    public static class SeriLogExtenstion
    {
        public static LoggerConfiguration MongoAdapter(
            this LoggerSinkConfiguration loggerConfiguration,MongoAdapterOptions options,
            IFormatProvider formatProvider = null)
        {
            var sink = new SeriLogMongoAdapter(formatProvider);
            sink.ConnectionString = options.ConnectionString;
            sink.CollectionName = options.CollectionName;
            sink.IsCappedCollection = options.IsCappedCollection;
            sink.CappedCollectionSize = options.CappedCollectionSize;
            sink.CappedCollectionMaxItems = options.CappedCollectionSize;
            sink.IncloudBaseInfo = options.IncloudBaseInfo;
            sink.IncloudCustomFields = options.IncloudCustomFields;
            sink.IncloudEventProperties = options.IncloudEventProperties;
            sink.IgnoredEventProperties = options.IgnoredEventProperties;
            return loggerConfiguration.Sink(sink);
        }
    }
}
