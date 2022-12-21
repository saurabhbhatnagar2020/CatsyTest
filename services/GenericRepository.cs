using CatsyTest.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatsyTest.services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected CatsyTestContext context;
        internal DbSet<T> dbSet;
        protected readonly ILogger _logger;

        public GenericRepository(CatsyTestContext context, ILogger logger)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
            this._logger = logger;

        }

        public virtual Task<IEnumerable<T>> All()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

    }
}
