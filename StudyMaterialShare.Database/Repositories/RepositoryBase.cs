using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.Database.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly StudyMaterialShareDbContext _context;

        public RepositoryBase(StudyMaterialShareDbContext context)
        {
            _context = context;
        }

        public abstract T? Create(T entity);

        public abstract T? Delete(int id);

        public abstract T? Delete(T entity);

        public abstract IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

        public abstract T? Get(int id);

        public abstract T? Get(Expression<Func<T, bool>> predicate);

        public abstract T? Update(T entity);
    }
}
