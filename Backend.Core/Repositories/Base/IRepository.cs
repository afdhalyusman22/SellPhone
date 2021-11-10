using Backend.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories.Base
{
    public interface IRepository
    {
        Task<(bool Commited, string Message)> CommitSync();
        Task<(bool Added, string Message)> AddAsync<T>(T entity) where T : BaseEntity;
        Task<(bool Deleted, string Message)> DeleteAsync<T>(long id) where T : BaseEntity;
        Task<List<T>> ListAsync<T>(int takeMaxRows = 0) where T : BaseEntity;
        Task<List<T>> ListAsync<T>() where T : BaseEntity;
        Task<List<T>> ListAsync<T>(int takeMaxRows = 0, params Expression<Func<T, object>>[] includes) where T : BaseEntity;
        Task<List<T>> ListAsyncWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity;
        Task<T> GetByIdAsync<T>(long id) where T : BaseEntity;
        IQueryable<T> GetQueryable<T>() where T : BaseEntity;
        Task<T> GetByIdAsync<T>(long id, params Expression<Func<T, object>>[] includes) where T : BaseEntity;
        Task<(bool Updated, string Message)> UpdateAsync<T>(T entity) where T : BaseEntity;
        List<T> ListWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity;
 
         Task<(bool Updated, string Message)> UpdateManyAsync<T>(T entity) where T : BaseEntity;
    }
}
