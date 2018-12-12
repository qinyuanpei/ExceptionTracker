using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace ExceptionTracker.Apis.Repository
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entities"></param>
        void Insert(params TEntity[] entities);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task InsertAsync(params TEntity[] entities);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// 返回全部数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// 返回满足指定条件的数据
        /// </summary>
        /// <param name="exps"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetByQuery(Expression<Func<TEntity,bool>> exps);

        /// <summary>
        /// 通过主键查找数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(string id);
    }
}
