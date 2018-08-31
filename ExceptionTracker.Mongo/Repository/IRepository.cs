using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExceptionTracker.Mongo.Repository
{
    public interface IRepository<T>
    {

        T Add(T enity);
        bool Update(Expression<Func<T, bool>> exp, T entity);
        bool Delete(Expression<Func<T, bool>> exp);
        T Find(Expression<Predicate<T>> exp);
        IQueryable<T> FindAll(Expression<Predicate<T>> exp);
    }
}
