using Emne7_Eksamen.Features.Common.Interfaces;

namespace Emne7_Eksamen.Features.Races.Mappers;

public class RaceMapper : IMapper<Race, RaceDTO>
{
    public RaceDTO MapToDTO(Race model)
    {
        return new RaceDTO()
            {
                RaceId = model.RaceId,
                Date = model.Date,
                Distance = model.Distance
            };
    }

    public Race MapToModel(RaceDTO dto)
    {
        return new Race()
            {
                RaceId = dto.RaceId,
                Date = dto.Date,
                Distance = dto.Distance
            };
    }
}