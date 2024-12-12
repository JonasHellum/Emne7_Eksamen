using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Models;

namespace Emne7_Eksamen.Features.Members.Mappers;

public class MemberMapper : IMapper<Member, MemberDTO>
{
    public MemberDTO MapToDTO(Member model)
    {
        return new MemberDTO()
        {
            MemberId = model.MemberId,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Gender = model.Gender,
            BirthYear = model.BirthYear,
            Created = model.Created,
            Updated = model.Updated,
            HashedPassword = model.HashedPassword
        };
    }

    public Member MapToModel(MemberDTO dto)
    {
        return new Member()
        {
            MemberId = dto.MemberId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            BirthYear = dto.BirthYear,
            Created = dto.Created,
            Updated = dto.Updated,
            HashedPassword = dto.HashedPassword
        };
    }
}