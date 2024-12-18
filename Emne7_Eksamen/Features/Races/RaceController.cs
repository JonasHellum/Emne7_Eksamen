using Emne7_Eksamen.Features.Races.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Emne7_Eksamen.Features.Races;




[ApiController]
[Route("api/v1/races")]
public class RaceController : ControllerBase
{
    private readonly ILogger<RaceController> _logger;
    private readonly IRaceService _raceService;

    public RaceController(ILogger<RaceController> logger, 
        IRaceService raceService)
    {
        _logger = logger;
        _raceService = raceService;
    }
    
    [HttpPost("Register", Name = "RegisterRaceAsync")]
    public async Task<ActionResult<RaceDTO>> RegisterRaceAsync([FromBody] RaceRegistrationDTO registrationDTO)
    {
        _logger.LogInformation($"Doing a Post on race registration");
        var race = await _raceService.RegistrationAsync(registrationDTO);
        return race is null
            ? BadRequest("Failed to register new race")
            : Ok(race);
    }
    
    [HttpGet("GetRaces", Name = "GetRacesAsync")]
    public async Task<ActionResult<IEnumerable<RaceDTO>>> GetRacesAsync(
        [FromQuery] RaceSearchParams? searchParams,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (searchParams?.RaceId is null &&
            searchParams?.Date is null &&
            searchParams?.Year is null &&
            searchParams?.Month is null &&
            searchParams?.Day is null &&
            searchParams?.Distance is null)
        {
            _logger.LogInformation($"Doing a get on races with page number: {pageNumber} and page size: {pageSize}");
            var racesDtos = await _raceService.GetPagedAsync(pageNumber,pageSize);
            return Ok(racesDtos);
        }
        _logger.LogInformation($"Doing a filtered get on races based on criteria: {searchParams}");
        return Ok(await _raceService.FindAsync(searchParams));
    }
    
    [HttpPut("{raceId}", Name = "UpdateRaceAsync")]
    public async Task<ActionResult<RaceDTO>> UpdateRaceAsync(int raceId, [FromBody] RaceUpdateDTO raceDTO)
    {
        _logger.LogInformation($"Doing a put on race with id: {raceId}");
        var updatedRace = await _raceService.UpdateAsync(raceId, raceDTO);
        
        return updatedRace is null
            ? BadRequest("Failed to update race")
            : Ok(updatedRace);
    }
    
    [HttpDelete("{raceId}", Name = "DeleteRaceAsync")]
    public async Task<ActionResult<bool>> DeleteRaceAsync(int raceId)
    {
        _logger.LogInformation($"Doing a Delete on race with id: {raceId}");
        var deletedRace = await _raceService.DeleteByIdAsync(raceId);
        
        return deletedRace
            ? Ok(true)
            : BadRequest("Failed to delete race");
    }
}