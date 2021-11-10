using Backend.Core.Entities.Base;
using Backend.Core.Repositories.Base;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories.Base
{
    public class Repository : IRepository
    {
        protected readonly DBContext _context;
        public Repository(DBContext context)
        {
            _context = context;
        }

        public async Task<(bool Commited, string Message)> CommitSync()
        {
            try
            {
                var r = await _context.SaveChangesAsync();
                String msgSuccess = null;

                msgSuccess = "Commited";
                return (true, msgSuccess);

            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<(bool Added, string Message)> AddAsync<T>(T entity) where T : BaseEntity
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return (true, "Success Save Data");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<(bool Deleted, string Message)> DeleteAsync<T>(long id) where T : BaseEntity
        {
            try
            {
                var item = await _context.Set<T>().SingleOrDefaultAsync(e => e.Id == id);

                _context.Remove<T>(item);

                await _context.SaveChangesAsync();
                return (true, "Success Deleted Data");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public async Task<List<T>> ListAsync<T>(int takeMaxRows = 0) where T : BaseEntity
        {
            var query = _context.Set<T>() as IQueryable<T>;
            if (takeMaxRows > 0)
            {
                query = query.Take(takeMaxRows).OrderBy(o => o.Id);
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> ListAsync<T>() where T : BaseEntity
        {
            var query = _context.Set<T>() as IQueryable<T>;
            return await query.ToListAsync();
        }

        public async Task<List<T>> ListAsync<T>(int takeMaxRows = 0, params Expression<Func<T, object>>[] includes) where T : BaseEntity
        {
            var query = _context.Set<T>() as IQueryable<T>;
            query = query.EagerLoadInclude(includes);
            if (takeMaxRows > 0)
            {
                query = query.Take(takeMaxRows).OrderBy(o => o.Id);
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> ListAsyncWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            var query = _context.Set<T>() as IQueryable<T>;
            query = query.Where(whereString);

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(long id) where T : BaseEntity
        {
            var query = _context.Set<T>() as IQueryable<T>;
            query = query.Where(i => i.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        public IQueryable<T> GetQueryable<T>() where T : BaseEntity
        {
            var rItem = _context.Set<T>().AsQueryable<T>();
            return rItem;
        }

        public async Task<T> GetByIdAsync<T>(long id, params Expression<Func<T, object>>[] includes) where T : BaseEntity
        {
            var query = _context.Set<T>() as IQueryable<T>;
            query = query.EagerLoadInclude(includes).Where(i => i.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<(bool Updated, string Message)> UpdateAsync<T>(T entity) where T : BaseEntity
        {
            try
            {
                var item = await _context.Set<T>().SingleOrDefaultAsync(e => e.Id == entity.Id);

                _context.Entry(item).State = EntityState.Modified;
                _context.Entry(item).CurrentValues.SetValues(entity);
                _context.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                _context.Entry(item).Property(x => x.CreatedAt).IsModified = false;
                await _context.SaveChangesAsync();
                return (true, "Success Updated Data");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public List<T> ListWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            var query = _context.Set<T>() as IQueryable<T>;
            query = query.Where(whereString);

            return query.ToList();
        }

        public async Task<(bool Updated, string Message)> UpdateManyAsync<T>(T entity) where T : BaseEntity
        {
            try
            {
                var item = await _context.Set<T>().SingleOrDefaultAsync(e => e.Id == entity.Id);

                _context.Entry(item).State = EntityState.Modified;
                _context.Entry(item).CurrentValues.SetValues(entity);
                _context.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                _context.Entry(item).Property(x => x.CreatedAt).IsModified = false;
                await _context.SaveChangesAsync();
                return (true, "Success Updated Data");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
    }
}
