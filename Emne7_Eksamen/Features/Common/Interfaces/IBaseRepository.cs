using System.Linq.Expressions;

namespace Emne7_Eksamen.Features.Common.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> AddAsync(T entity);
    Task<T?> UpdateAsync(T entity); 
    Task<T?> DeleteByIdAsync(int id); // for single key
    Task<T?> DeleteByIdAsync(params object[] keyValues); // for composite key
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate );
    Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
}