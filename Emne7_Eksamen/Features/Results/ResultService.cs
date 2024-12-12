using Emne7_Eksamen.Features.Results.Interfaces;

namespace Emne7_Eksamen.Features.Results;

public class ResultService : IResultService
{
    public async Task<ResultDTO> AddAsync<TAdd>(TAdd addDto) where TAdd : class
    {
        throw new NotImplementedException();
    }

    public async Task<ResultDTO> UpdateAsync<TUpdate>(int id, TUpdate updateDto) where TUpdate : class
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}