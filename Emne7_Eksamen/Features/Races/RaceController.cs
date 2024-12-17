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
            searchParams?.Distance is null)
        {
            var racesDtos = await _raceService.GetPagedAsync(pageNumber,pageSize);
            return Ok(racesDtos);
        }
        return Ok(await _raceService.FindAsync(searchParams));
    }
}