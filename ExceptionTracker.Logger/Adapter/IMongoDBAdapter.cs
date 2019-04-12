using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace ExceptionTracker.Logger.Adapter
{
    public interface IMongoDBAdapter
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 数据库集合名称
        /// </summary>
        string CollectionName { get; set; }

        IMongoDatabase GetDatabase();
    }
}
