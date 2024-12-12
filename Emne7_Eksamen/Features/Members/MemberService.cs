using System.Linq.Expressions;
using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Interfaces;
using Emne7_Eksamen.Features.Members.Models;

namespace Emne7_Eksamen.Features.Members;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    public MemberService(
        IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }
    public async Task<MemberDTO> AddAsync<TAdd>(TAdd addDto) where TAdd : class
    {
        throw new NotImplementedException();
    }

    public async Task<MemberDTO> UpdateAsync<TUpdate>(int id, TUpdate updateDto) where TUpdate : class
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<MemberDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> AuthenticateUserAsync(string firstName, string password)
    {
        Expression<Func<Member, bool>> expr = user => user.FirstName == firstName;
        var usr = (await _memberRepository.FindAsync(expr)).FirstOrDefault();
        if (usr is null) return null;

        // sjekker om passord stemmer !!
        if (BCrypt.Net.BCrypt.Verify(password, usr.HashedPassword))
            return usr.MemberId;
        
        return null;
    }
}