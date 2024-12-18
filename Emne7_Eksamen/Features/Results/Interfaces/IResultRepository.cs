using Emne7_Eksamen.Features.Common.Interfaces;

namespace Emne7_Eksamen.Features.Results.Interfaces;

public interface IResultRepository : IBaseRepository<Result>
{
    Task<Result?> GetByRaceAndMemberIdAsync(int raceId, int memberId);
}