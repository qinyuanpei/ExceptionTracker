using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using NLog.Config;

namespace ExceptionTracker.Logger.NLog
{
    [NLogConfigurationItem]
    public class LayoutField
    {
        public LayoutField(string name, string layout, string bsonType = null)
        {
            Name = name;
            Layout = layout;
            bsonType = bsonType ?? "String";
        }

        [RequiredParameter]
        public string Name { get; set; }

        [RequiredParameter]
        public string Layout { get; set; }

        [RequiredParameter]
        public string BsonType { get; set; }
    }
}
