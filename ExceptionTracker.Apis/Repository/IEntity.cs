using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExceptionTracker.Apis.Repository
{
    public interface IEntity
    {
        ObjectId Id { get; set; }
    }
}
