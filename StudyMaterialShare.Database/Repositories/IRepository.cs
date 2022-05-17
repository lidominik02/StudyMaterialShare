using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.Database.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        T? Get(int id);
        T? Get(Expression<Func<T, bool>> predicate);
        T? Update(T entity);
        T? Delete(int id);
        T? Delete(T entity);
        T? Create(T entity);
    }
}
