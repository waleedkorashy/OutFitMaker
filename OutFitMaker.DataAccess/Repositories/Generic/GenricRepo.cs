using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using OutFitMaker.Services.IServices.Generic;
using OutFitMaker.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OutFitMaker.DataAccess.DbContext;

namespace OutFitMaker.Services.Services.Generic
{
    public class GenricRepo<T> : IGenricRepo<T> where T : class
    {
        public readonly OutFitMakerDbContext _context;
        private DbSet<T> table = null;
        public GenricRepo(OutFitMakerDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }
        public void Delete(T entity)
        {
            //T ID = table.Find(id);

            table.Attach(entity);
        }

        public void ForceDelete(T entity)
        {
            _context.Remove(entity);
        }

        public void ForceDeleteRange(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> query)
        {
            return await table.AsNoTracking().Where(query).ToListAsync();
        }

        //public IEnumerable<T> GetAllWithPagination(Expression<Func<T, bool>> query, int pageNumber, int pageSize)
        //{
        //    return table.Where(query).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
        //}

        //public async Task<IEnumerable<T>> GetAllWithPaginationAsync(Expression<Func<T, bool>> query, int pageNumber, int pageSize)
        //{
        //    return await table.Where(query).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();
        //}

        public IQueryable<T> Find(Expression<Func<T, bool>> query)
        {
            return table.Where(query);
        }

        public IQueryable<T> GetPaginated(IQueryable<T> data, int pageNumber, int pageSize)
        {
            return data.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
        public IQueryable<T> GetPaginatedMarket(IQueryable<T> data, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                // Return the entire dataset without pagination if page number or page size is invalid
                return data;
            }

            return data.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> query)
        {
            return await table.AsNoTracking().Where(query).ToListAsync();
        }
        /// <summary>
        /// Submit your includes or null
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            IQueryable<T> query = table;
            if (includes != null)

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            return query.SingleOrDefault(predicate);
        }
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            IQueryable<T> query = table;
            if (includes != null)

                foreach (var include in includes)
                {
                    query = query.Include(include).AsSplitQuery();
                }
            return query.AsNoTracking().Where(predicate).ToList();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            IQueryable<T> query = table;
            if (includes != null)

                foreach (var include in includes)
                {
                    query = query.Include(include).AsSplitQuery();
                }
            return await query.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await table.FindAsync(id);
        }
        //public async Task<T> GetByIdsAsync(List< Guid> ids)
        //{
        //    return await table.FindAsync(ids);
        //}
        public T GetById(Guid id)
        {
            return table.Find(id);
        }

        public Task Add(T entity)
        {
            table.AddAsync(entity);

            return Task.CompletedTask;
        }
        public T AddhasReturn(T entity)
        {
            return table.Add(entity).Entity;
        }
        public void AddRange(IEnumerable<T> entities)
        {
            table.AddRange(entities);

        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await table.AddRangeAsync(entities);
        }
        public void Update(T entity)
        {
            try
            {
                var type = typeof(T);
                PropertyInfo lastUpdatedDate = type.GetProperty("LastUpdatedDate");
                lastUpdatedDate.SetValue(entity, DateTimeOffset.Now);
            }
            catch (Exception e)
            {

            }
            finally
            {
                _context.Update(entity);
                _context.SaveChanges();
            }
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            try
            {
                var type = typeof(T);
                PropertyInfo lastUpdatedDate = type.GetProperty("LastUpdatedDate");
                foreach (var entity in entities)
                {
                    lastUpdatedDate.SetValue(entity, DateTimeOffset.Now);
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                table.UpdateRange(entities);
            }

        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }





        public T UpdateTwo(T entity)
        {
            table.Update(entity);
            return entity;
        }

        public async Task<IEnumerable<TType>> GetSpecificSelectAsync<TType>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TType>> select,
            string includeProperties = null!,
            int? skip = null,
            int? take = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!

            ) where TType : class
        {
            IQueryable<T> query = table.AsNoTracking();

            if (includeProperties != null)
            {
                query.AsSplitQuery();
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty).IgnoreQueryFilters();
            }

            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                query = orderBy(query);

            if (skip.HasValue)
                query = query.Skip(skip.Value);
            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.Select(select).ToListAsync();
        }

    }
}
