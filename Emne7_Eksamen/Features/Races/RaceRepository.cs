using System.Linq.Expressions;
using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Races.Interfaces;

namespace Emne7_Eksamen.Features.Races;

public class RaceRepository : IRaceRepository
{
    public async Task<Race?> AddAsync(Race entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Race?> UpdateAsync(Race entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Race?> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Race?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Race>> FindAsync(Expression<Func<Race, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}