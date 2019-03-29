using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace ExceptionTracker.Logger.Adapter
{
    public interface IMongoDBAdapter
    {
        IMongoDatabase GetDatabase();
    }
}
