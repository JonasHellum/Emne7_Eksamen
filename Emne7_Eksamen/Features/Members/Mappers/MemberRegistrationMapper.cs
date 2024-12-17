using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Models;

namespace Emne7_Eksamen.Features.Members.Mappers;

public class MemberRegistrationMapper : IMapper<Member, MemberRegistrationDTO>
{
    public MemberRegistrationDTO MapToDTO(Member model)
    {
        return new MemberRegistrationDTO()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Gender = model.Gender,
            BirthYear = model.BirthYear
        };
    }

    public Member MapToModel(MemberRegistrationDTO dto)
    {
        return new Member()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            BirthYear = dto.BirthYear,
        };
    }
}