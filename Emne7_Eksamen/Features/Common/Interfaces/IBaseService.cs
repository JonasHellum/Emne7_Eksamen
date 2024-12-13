namespace Emne7_Eksamen.Features.Common.Interfaces;

public interface IBaseService<T> where T : class
{
    Task<bool> DeleteByIdAsync(int id);
    Task<T?> GetByIdAsync(int id);
}