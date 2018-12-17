using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ExceptionTracker.Logger.Models
{
    [Serializable]
    public class BaseLog
    {
        public string Id { get; set; }
        public DateTime? Date { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
    }
}
