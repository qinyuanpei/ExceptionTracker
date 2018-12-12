using ExceptionTracker.Apis.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ExceptionTracker.Apis.Models
{
    [Serializable]
    public class BaseLog : IEntity
    {
        public ObjectId Id { get; set; }

        public string Message { get; set; }

        public DateTime? CreatedTime { get; set; }

        public string Level { get; set; }

        public string UserId { get; set; }

        public string DeviceName { get; set; }

        public string DeviceID { get; set; }
    }
}
