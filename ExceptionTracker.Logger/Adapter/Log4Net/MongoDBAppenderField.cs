using System;
using System.Collections.Generic;
using System.Text;
using log4net.Layout;

namespace ExceptionTracker.Logger.Adapter.Log4Net
{
    public class MongoDBAppenderField
    {
        public string Name { get; set; }
        public IRawLayout Layout { get; set; }
    }
}
