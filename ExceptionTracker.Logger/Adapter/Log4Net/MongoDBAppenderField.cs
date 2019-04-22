using System;
using System.Collections.Generic;
using System.Text;
using log4net.Layout;

namespace ExceptionTracker.Logger.Adapter.Log4Net
{
    public class MongoDBAppenderField
    {
        public MongoDBAppenderField() 
            : this(null, null, "String")
        {

        }

        private MongoDBAppenderField(string name, IRawLayout layout, string bsonType)
        {
            Name = name;
            Layout = layout;
            BsonType = bsonType;
        }

        private MongoDBAppenderField(string name, IRawLayout layout) : this(name, layout, "String")
        {

        }

        public string Name { get; set; }

        public IRawLayout Layout { get; set; }

        public string BsonType { get; set; } = "String";
    }
}
