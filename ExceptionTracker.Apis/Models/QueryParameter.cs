using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ExceptionTracker.Apis.Models
{
    public class QueryParameter<T>
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public List<string> OrderBy { get; set; }

        /// <summary>
        /// 分页参数
        /// </summary>
        public Pagnation<T> Pagnation { get; set; }

        /// <summary>
        /// 筛选条件
        /// </summary>
        public List<FilterRule> Filters { get; set; }

        /// <summary>
        /// 默认升序排列
        /// </summary>
        public SortDirection Sort { get; set; } = SortDirection.ASC;
    }

    public class Pagnation<T>
    {
        public Pagnation(List<T> items)
        {
            this.Items = items;
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        public List<T> Items { get; private set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }
    }

    [Flags]
    public enum SortDirection
    {
        ASC = 0,   //升序
        DESC = 1 //降序
    }

    public class FilterRule
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        public FilterOperator Operator { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string OnGroup { get; set; }
    }

    public enum FilterOperator
    {

    }
}
