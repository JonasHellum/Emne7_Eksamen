using Emne7_Eksamen.Features.Common.Interfaces;

namespace Emne7_Eksamen.Features.Results.Interfaces;

public interface IResultService : IBaseService<ResultDTO>
{
    Task<IEnumerable<ResultDTO?>> GetPagedAsync(int pageNumber, int pageSize);
    Task<ResultDTO?> RegistrationAsync(int raceId, ResultRegistrationDTO registrationDTO);
    Task<ResultDTO?> UpdateAsync(int raceId, ResultUpdateDTO updateDto);
}