using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExceptionTracker.Logger.Adapter
{
    public interface IMongoDBAdapter<TLogEvent>
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 数据库集合名称
        /// </summary>
        string CollectionName { get; set; }

        /// <summary>
        /// 是否包含基本信息
        /// </summary>
        bool IncloudBaseInfo { get; set; }

        /// <summary>
        /// 是否包含自定义字段
        /// </summary>
        bool IncloudCustomFields { get; set; }

        /// <summary>
        /// 是否包含事件属性
        /// </summary>
        bool IncloudEventProperties { get; set; }

        /// <summary>
        /// 可忽略的事件属性
        /// </summary>
        string IgnoredEventProperties { get; set; }

        /// <summary>
        /// 返回当前数据库
        /// </summary>
        /// <returns></returns>
        IMongoDatabase GetDatabase();

        /// <summary>
        /// 创建Bson文档
        /// </summary>
        /// <typeparam name="TLogEvent">typeof(TLogEvent)</typeparam>
        /// <param name="logEvent">日志事件</param>
        /// <returns></returns>
        BsonDocument CreateBsonDocument(TLogEvent logEvent);
    }
}
