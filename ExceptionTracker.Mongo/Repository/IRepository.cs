using System;
using System.Linq;
using System.Linq.Expressions;

namespace ExceptionTracker.Mongo.Repository
{
    public interface IRepository<T> 
    {
        
        T Add(T enity);
        bool Update(T entity);
        bool Delete(T entity);
        T Find(Expression<Predicate<T>> exp);
        IQueryable<T> FindAll(Expression<Predicate<T>> exp);
    }
}
