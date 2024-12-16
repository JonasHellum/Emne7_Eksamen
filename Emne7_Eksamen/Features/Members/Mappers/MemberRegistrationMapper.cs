using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Models;

namespace Emne7_Eksamen.Features.Members.Mappers;

public class MemberRegistrationMapper : IMapper<MemberDTO, MemberRegistrationDTO>
{
    public MemberRegistrationDTO MapToDTO(MemberDTO model)
    {
        return new MemberRegistrationDTO()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Gender = model.Gender,
            BirthYear = model.BirthYear
        };
    }

    public MemberDTO MapToModel(MemberRegistrationDTO dto)
    {
        return new MemberDTO()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            BirthYear = dto.BirthYear,
        };
    }
}