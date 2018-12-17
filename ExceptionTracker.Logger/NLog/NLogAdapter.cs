using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using ExceptionTracker.Logger.Models;
using ExceptionTracker.Logger.NLog;
using MongoDB.Bson;
using NLog;
using NLog.Targets;
using NLog.Common;

namespace ExceptionTracker.Logger
{
    class NLogAdapter
    {

    }

    [Target("Mongo")]
    public class MongoTarget : Target
    {

        /// <summary>
        /// 终端地址
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MongoTarget()
        {
            
        }

        public MongoTarget(IEnumerable<LayoutField> fields)
        {

        }


        protected override void Write(LogEventInfo logEvent)
        {

        }


        protected override void Write(IList<AsyncLogEventInfo> logEvents)
        {

        }
    }
}
