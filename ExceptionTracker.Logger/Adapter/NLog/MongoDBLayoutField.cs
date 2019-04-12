using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using NLog.Config;
using NLog.Layouts;

namespace ExceptionTracker.Logger.Adapter.NLog
{
    [NLogConfigurationItem]
    [ThreadAgnostic]
    public sealed class MongoDBLayoutField
    {
        public MongoDBLayoutField(string name, Layout layout, string bsonType)
        {
            Name = name;
            Layout = layout;
            BsonType = bsonType;
        }

        public MongoDBLayoutField(string name, Layout layout) : this(name, layout, "String")
        {

        }


        [RequiredParameter]
        public string Name { get; set; }

        [RequiredParameter]
        public Layout Layout { get; set; }

        [DefaultValue("String")]
        public string BsonType { get; set; }

    }
}
