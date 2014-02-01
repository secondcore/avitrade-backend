using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //properties
        protected virtual IQueryable<T> DefaultSet
        {
            get { return _Context.Set<T>(); }
        }

        protected virtual Func<T, object> DefaultOrderBy
        {
            get { return x => x.GetHashCode(); }
        }

        protected AviTradeContext _Context;

        //constructor
        public GenericRepository(AviTradeContext context)
        {
            _Context = context;
        }

        //methods
        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _Context.Set<T>().Add(entity);
            _Context.SaveChanges();
        }

        public virtual void Remove(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _Context.Set<T>().Remove(entity);
            _Context.SaveChanges();
        }

        public virtual void Modify(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _Context.Set<T>().Attach(entity);
            _Context.Entry(entity).State = System.Data.EntityState.Modified;
            _Context.SaveChanges();
        }

        public virtual IEnumerable<T> GetAll(int pageIndex = 0, int pageCount = 10, Func<T, object> orderBy = null, bool descending = true)
        {
            var order = orderBy ?? DefaultOrderBy;
            if (pageIndex > 0)
            {
                int toSkip = (pageIndex - 1) * pageCount;
                if (descending)
                    return DefaultSet.OrderByDescending(order).Skip(toSkip).Take(pageCount);
                else
                    return DefaultSet.OrderBy(order).Skip(toSkip).Take(pageCount);
            }
            else
            {
                if (descending)
                    return DefaultSet.OrderByDescending(order);
                else
                    return DefaultSet.OrderBy(order);
            }
        }

        public virtual IEnumerable<T> Search(Expression<Func<T, bool>> criteria, int pageIndex = 1, int pageCount = 10, Func<T, object> orderBy = null, bool descending = true)
        {
            if (criteria == null)
                throw new ArgumentNullException("criteria");

            var order = orderBy ?? DefaultOrderBy;

            if (pageIndex > 0)
            {
                int toSkip = (pageIndex - 1) * pageCount;
                if (descending)
                    return DefaultSet.Where(criteria).OrderByDescending(order).Skip(toSkip).Take(pageCount);
                else
                    return DefaultSet.Where(criteria).OrderBy(order).Skip(toSkip).Take(pageCount);
            }
            else
            {
                if (descending)
                    return DefaultSet.Where(criteria).OrderByDescending(order);
                else
                    return DefaultSet.Where(criteria).OrderBy(order);
            }
        }

        public virtual T FindById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> FindById(params int[] ids)
        {
            throw new NotImplementedException();
        }

        public virtual T FindById(string id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> FindById(params string[] ids)
        {
            throw new NotImplementedException();
        }

        public virtual int CountAll()
        {
            return DefaultSet.Count();
        }

        public virtual int Count(Expression<Func<T, bool>> criteria)
        {
            return DefaultSet.Where(criteria).Count();
        }
    }
}