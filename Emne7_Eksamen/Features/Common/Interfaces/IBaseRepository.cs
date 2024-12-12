using System.Linq.Expressions;

namespace Emne7_Eksamen.Features.Common.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> AddAsync(T entity);
    Task<T?> UpdateAsync(T entity);
    Task<T?> DeleteByIdAsync(int id);
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate );
}