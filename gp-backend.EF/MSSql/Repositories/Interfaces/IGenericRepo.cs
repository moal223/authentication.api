using gp_backend.Core.Models;

namespace gp_backend.EF.MSSql.Repositories.Interfaces
{
    public interface IGenericRepo<T>
    {
        Task<List<T>> GetAllAsync(string userId);
        Task<T> GetByIdAsync(int id);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task DeleteAsync(T entity);
        Task SaveAsync();
    }
}
