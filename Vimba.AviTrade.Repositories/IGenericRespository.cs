using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Vimba.AviTrade.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        void Modify(T entity);

        IEnumerable<T> GetAll(int pageIndex = 0, int pageCount = 10, Func<T, object> orderBy = null, bool descending = true);
        IEnumerable<T> Search(Expression<Func<T, bool>> criteria, int pageIndex = 0, int pageCount = 10, Func<T, object> orderBy = null, bool descending = true);

        int CountAll();
        int Count(Expression<Func<T, bool>> criteria);

        T FindById(int id);
        IEnumerable<T> FindById(params int[] ids);

        T FindById(string id);
        IEnumerable<T> FindById(params string[] ids);
    }
}
