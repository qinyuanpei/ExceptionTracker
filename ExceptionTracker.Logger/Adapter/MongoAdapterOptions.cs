using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionTracker.Logger.Adapter
{
    public class MongoAdapterOptions
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库集合名称
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// 是否包含基本信息
        /// </summary>
        public bool IncloudBaseInfo { get; set; } = true;

        /// <summary>
        /// 是否包含自定义字段
        /// </summary>
        public bool IncloudCustomFields { get; set; } = true;

        /// <summary>
        /// 是否包含事件属性
        /// </summary>
        public bool IncloudEventProperties { get; set; } = true;

        /// <summary>
        /// 可忽略的事件属性
        /// </summary>
        public string IgnoredEventProperties { get; set; }

        /// <summary>
        /// 是否为封闭集合
        /// </summary>
        public bool IsCappedCollection { get; set; } = true;

        /// <summary>
        /// 集合大小
        /// </summary>
        public long? CappedCollectionSize { get; set; } = 1024;

        /// <summary>
        /// 文档数目
        /// </summary>
        public long? CappedCollectionMaxItems { get; set; } = 100;
    }
}
