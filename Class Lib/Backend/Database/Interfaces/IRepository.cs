using System.Linq.Expressions;

namespace Class_Lib.Backend.Database.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetByCriteriaAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(uint id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        QueryBuilderService<T> Query();
    }
}
