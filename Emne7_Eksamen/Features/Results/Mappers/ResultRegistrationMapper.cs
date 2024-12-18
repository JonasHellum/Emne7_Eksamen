using Emne7_Eksamen.Features.Common.Interfaces;

namespace Emne7_Eksamen.Features.Results;

public class ResultRegistrationMapper : IMapper<Result, ResultRegistrationDTO>
{
    public ResultRegistrationDTO MapToDTO(Result model)
    {
        return new ResultRegistrationDTO()
        {
            
        };
    }

    public Result MapToModel(ResultRegistrationDTO dto)
    {
        return new Result()
        {
            
        };
    }
}