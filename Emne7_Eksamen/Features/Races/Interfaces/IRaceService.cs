using Emne7_Eksamen.Features.Common.Interfaces;

namespace Emne7_Eksamen.Features.Races.Interfaces;

public interface IRaceService : IBaseService<RaceDTO>
{
    Task<IEnumerable<RaceDTO?>> GetPagedAsync(int pageNumber, int pageSize);
    Task<RaceDTO?> RegistrationAsync(RaceRegistrationDTO registrationDTO);
    Task<RaceDTO?> UpdateAsync(int id, RaceUpdateDTO updateDTO);
    Task<IEnumerable<RaceDTO>> FindAsync(RaceSearchParams searchParams);
}