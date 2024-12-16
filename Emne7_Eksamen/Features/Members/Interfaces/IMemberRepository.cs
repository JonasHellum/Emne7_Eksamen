using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Models;

namespace Emne7_Eksamen.Features.Members.Interfaces;

public interface IMemberRepository : IBaseRepository<Member>
{
    Task<IEnumerable<Member>> GetPagedAsync(int pageNumber, int pageSize);
}