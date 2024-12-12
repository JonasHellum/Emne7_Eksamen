using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Models;

namespace Emne7_Eksamen.Features.Members.Interfaces;

public interface IMemberService : IBaseService<MemberDTO>
{
    Task<int?> AuthenticateUserAsync(string firstName, string password);
}