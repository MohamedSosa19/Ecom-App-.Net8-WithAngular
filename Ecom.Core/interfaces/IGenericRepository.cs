using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.interfaces
{
    public interface IGenericRepository<T> where T:class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);// using this to  get all navigational Property 
        Task<T> GetAsyncById(int id);
        Task<T> GetAsyncById(int id,params Expression<Func<T, object>>[] includes);// using this to  get all navigational Property 
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
