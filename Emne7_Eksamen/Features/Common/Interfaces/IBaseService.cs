namespace Emne7_Eksamen.Features.Common.Interfaces;

public interface IBaseService<T> where T : class
{
    Task<T> AddAsync<TAdd>(TAdd addDto) where TAdd : class;
    Task<T> UpdateAsync<TUpdate>(int id, TUpdate updateDto) where TUpdate : class;
    Task<bool> DeleteByIdAsync(int id);
    Task<T?> GetByIdAsync(int id);
}