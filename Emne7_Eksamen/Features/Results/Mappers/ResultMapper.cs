using Emne7_Eksamen.Features.Common.Interfaces;

namespace Emne7_Eksamen.Features.Results;

public class ResultMapper : IMapper<Result, ResultDTO>
{
    public ResultDTO MapToDTO(Result model)
    {
        return new ResultDTO()
        {
            RaceId = model.RaceId,
            MemberId = model.MemberId,
            Time = model.Time
        };
    }

    public Result MapToModel(ResultDTO dto)
    {
        return new Result()
        {
            RaceId = dto.RaceId,
            MemberId = dto.MemberId,
            Time = dto.Time
        };
    }
}