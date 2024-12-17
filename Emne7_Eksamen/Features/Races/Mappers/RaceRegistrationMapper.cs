using Emne7_Eksamen.Features.Common.Interfaces;

namespace Emne7_Eksamen.Features.Races.Mappers;

public class RaceRegistrationMapper : IMapper<Race, RaceRegistrationDTO>
{
    public RaceRegistrationDTO MapToDTO(Race model)
    {
        return new RaceRegistrationDTO()
            {
                Date = model.Date,
                Distance = model.Distance
            };
    }

    public Race MapToModel(RaceRegistrationDTO dto)
    {
        return new Race()
            {
                Date = dto.Date,
                Distance = dto.Distance
            };
    }
}