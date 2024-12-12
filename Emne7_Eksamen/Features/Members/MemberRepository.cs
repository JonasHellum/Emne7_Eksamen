using System.Linq.Expressions;
using Emne7_Eksamen.Features.Results;
using Emne7_Eksamen.Features.Results.Interfaces;

namespace Emne7_Eksamen.Features.Members;

public class MemberRepository : IResultRepository
{
    public async Task<Result?> AddAsync(Result entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Result?> UpdateAsync(Result entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Result?> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Result>> FindAsync(Expression<Func<Result, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}