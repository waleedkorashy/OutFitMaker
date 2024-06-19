using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Services.IServices.Generic
{
    public interface IGenricRepo<T> where T : class
    {
        IEnumerable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> query);
        IQueryable<T> GetPaginated(IQueryable<T> data, int pageNumber, int pageSize);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> query);
        T Find(Expression<Func<T, bool>> predicate, string[] includes = null);
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, string[] includes = null);
        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);
        Task Add(T item);
        T AddhasReturn(T item);
        void AddRange(IEnumerable<T> entities);
        void Update(T item);
        void Delete(T entity);
        void SaveChanges();
        Task SaveChangesAsync();
        Task<IEnumerable<TType>> GetSpecificSelectAsync<TType>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TType>> select,
            string includeProperties = null!,
            int? skip = null,
            int? take = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!
            ) where TType : class;

        Task<IEnumerable<T>> GetAllAsync();
    }
}

