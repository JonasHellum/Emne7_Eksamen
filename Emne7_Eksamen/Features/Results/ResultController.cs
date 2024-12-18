using Emne7_Eksamen.Features.Results.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Emne7_Eksamen.Features.Results;


[ApiController]
[Route("api/v1/results")]
public class ResultController : ControllerBase
{
    private readonly ILogger<ResultController> _logger;
    private readonly IResultService _resultService;

    public ResultController(ILogger<ResultController> logger, 
        IResultService resultService)
    {
        _logger = logger;
        _resultService = resultService;
    }

    [HttpPost("Register", Name = "RegisterResultAsync")]
    public async Task<ActionResult<ResultDTO>> RegisterResultAsync(int racetId, [FromBody] ResultRegistrationDTO registrationDTO)
    {
        _logger.LogInformation($"Doing a Post on result registration");
        var result = await _resultService.RegistrationAsync(racetId, registrationDTO);
        return result is null
            ? BadRequest("Failed to register new result")
            : Ok(result);
    }
    
    [HttpDelete("{resultId}", Name = "DeleteResultAsync")]
    public async Task<ActionResult<bool>> DeleteResultAsync(int resultId)
    {
        _logger.LogInformation($"Doing a Delete on result with id: {resultId}");
        var deletedResult = await _resultService.DeleteByIdAsync(resultId);
        
        return deletedResult
            ? Ok(true)
            : BadRequest("Failed to delete result");
    }
}