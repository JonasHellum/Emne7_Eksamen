using System.Data;
using System.Linq.Expressions;
using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Interfaces;
using Emne7_Eksamen.Features.Members.Models;
using Emne7_Eksamen.Features.Races.Interfaces;

namespace Emne7_Eksamen.Features.Races;

public class RaceService : IRaceService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IRaceRepository _raceRepository;
    private readonly ILogger<RaceService> _logger;
    private readonly IMapper<Race, RaceDTO> _raceMapper;
    private readonly IMapper<Race, RaceRegistrationDTO> _registrationMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RaceService(
        IMemberRepository memberRepository, 
        IRaceRepository raceRepository,
        ILogger<RaceService> logger, 
        IMapper<Race, RaceDTO> raceMapper, 
        IMapper<Race, RaceRegistrationDTO> registrationMapper, 
        IHttpContextAccessor httpContextAccessor)
    {
        _memberRepository = memberRepository;
        _raceRepository = raceRepository;
        _logger = logger;
        _raceMapper = raceMapper;
        _registrationMapper = registrationMapper;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<bool> DeleteByIdAsync(int raceId)
    {
        // _logger.LogInformation($"Trying to find logged in member.");
        // var loggedMember = await GetLoggedInMemberAsync();
        // if (loggedMember is null)
        // {
        //     _logger.LogWarning("Member is not authorized.");
        //     throw new UnauthorizedAccessException("Member is not authorized");
        // }
        //
        // _logger.LogInformation($"Trying to find race to delete by id: {raceId}");
        // var raceToDelete = await _raceRepository.GetByIdAsync(raceId);
        // if (raceToDelete is null)
        // {
        //     _logger.LogWarning($"Race with id: {raceId} not found.");
        //     throw new KeyNotFoundException($"Race with id: {raceId} not found.");
        // }
        //
        // _logger.LogInformation($"Checking if member with id: {loggedMember.MemberId} is " +
        //                        $"authorized to delete race with id: {raceToDelete.RaceId} " +
        //                        $"based on {raceToDelete.MemberId}");
        // if (loggedMember.MemberId != raceToDelete.MemberId)
        // {
        //     _logger.LogWarning($"Member with id: {loggedMember.MemberId} is not authorized " +
        //                        $"to delete race with id: {raceToDelete.RaceId}");
        //     throw new UnauthorizedAccessException($"Member with id: {loggedMember.MemberId} is not authorized to delete " +
        //                                           $"race with id: {raceToDelete.RaceId}");
        // }
        //
        // _logger.LogInformation($"Deleting race with id: {raceId}");
        // var deletedRace = await _raceRepository.DeleteByIdAsync(raceId);
        //
        // if (deletedRace == null)
        // {
        //     _logger.LogWarning($"Did not delete race with id: {raceId}.");
        //     throw new KeyNotFoundException($"Did not delete race with id: {raceId}.");
        // }
        //
        // return true;
        
        throw new NotImplementedException();
    }

    public async Task<RaceDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RaceDTO?>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var races = await _raceRepository.GetPagedAsync(pageNumber, pageSize);

        return races
            .Select(rac => _raceMapper.MapToDTO(rac))
            .ToList();
    }

    public async Task<RaceDTO?> RegistrationAsync(RaceRegistrationDTO registrationDTO)
    {
        _logger.LogInformation($"Trying to find logged in member.");
        var loggedMember = await GetLoggedInMemberAsync();
        if (loggedMember is null)
        {
            _logger.LogWarning("Member is not authorized.");
            throw new UnauthorizedAccessException("Member is not authorized");
        }
        
        var race = _registrationMapper.MapToModel(registrationDTO);

        var addedRace = await _raceRepository.AddAsync(race);
        if (addedRace is null)
        {
            _logger.LogError("Failed to add race.");
            throw new DataException("Failed to add race.");
        }
        
        return _raceMapper.MapToDTO(addedRace);
    }

    public async Task<RaceDTO?> UpdateAsync(int raceId, RaceUpdateDTO updateDTO)
    {
        // _logger.LogInformation($"Trying to find logged in member.");
        // var loggedMember = await GetLoggedInMemberAsync();
        // if (loggedMember is null)
        // {
        //     _logger.LogWarning("Member is not authorized.");
        //     throw new UnauthorizedAccessException("Member is not authorized");
        // }
        //
        // _logger.LogInformation($"Trying to find race to delete by id: {raceId}");
        // var raceToUpdate = await _raceRepository.GetByIdAsync(raceId);
        // if (raceToUpdate is null)
        // {
        //     _logger.LogWarning($"Race with id: {raceId} not found.");
        //     throw new KeyNotFoundException($"Race with id: {raceId} not found.");
        // }
        //
        // _logger.LogInformation($"Checking if member with id: {loggedMember.MemberId} is " +
        //                        $"authorized to update race with id: {raceToUpdate.RaceId} " +
        //                        $"based on {raceToUpdate.MemberId}");
        // if (loggedMember.MemberId != raceToUpdate.MemberId)
        // {
        //     _logger.LogWarning($"Member with id: {loggedMember.MemberId} is not authorized " +
        //                        $"to update race with id: {raceToUpdate.RaceId}");
        //     throw new UnauthorizedAccessException($"Member with id: {loggedMember.MemberId} is not authorized to update " +
        //                                           $"race with id: {raceToUpdate.RaceId}");
        // }
        //
        // _logger.LogInformation($"Updating race with id: {raceId} with current calues: " +
        //                        $"from: {raceToUpdate.Date} to: {updateDTO.Date} " +
        //                        $"from: {raceToUpdate.Distance} to: {updateDTO.Distance}");
        // raceToUpdate.Date = updateDTO.Date.ToDateTime(TimeOnly.MinValue);
        // raceToUpdate.Distance = updateDTO.Distance;
        //
        // var updatedRace = await _raceRepository.UpdateAsync(raceToUpdate);
        //
        // return updatedRace is null
        //     ? null
        //     : _raceMapper.MapToDTO(updatedRace);
        
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RaceDTO>> FindAsync(RaceSearchParams searchParams)
    {
        Expression<Func<Race, bool>> predicate = r =>
            (!searchParams.RaceId.HasValue || r.RaceId == searchParams.RaceId) &&
            (!searchParams.Date.HasValue || r.Date == searchParams.Date) &&
            (!searchParams.Distance.HasValue || r.Distance.ToString().Contains(searchParams.Distance.Value.ToString()));

        var races = await _raceRepository.FindAsync(predicate);
        
        return races.Select(u => _raceMapper.MapToDTO(u));
    }
    
    
    
    private async Task<Member?> GetLoggedInMemberAsync()
    {
        var loggedInMemberId = _httpContextAccessor.HttpContext?.Items["UserId"] as string;
        _logger.LogInformation("Logged in member ID: {LoggedInMemberId}", loggedInMemberId);
    
        if (string.IsNullOrEmpty(loggedInMemberId))
        {
            _logger.LogWarning("No logged in member.");
            throw new UnauthorizedAccessException("No logged in member.");
        }
        
        var loggedInMember = (await _memberRepository.FindAsync(m => m.MemberId.ToString() == loggedInMemberId)).FirstOrDefault();
        if (loggedInMember == null)
        {
            _logger.LogWarning("Logged in member not found: {LoggedInMemberId}", loggedInMemberId);
            throw new UnauthorizedAccessException("Logged in member ID not found.");
        }
        
        return loggedInMember;
    }
    
    // private async Task<IEnumerable<Race>> SearchRacesAsync(RaceSearchParams searchParams)
    // {
    //     DateTime? startDate = null;
    //     DateTime? endDate = null;
    //
    //     // Handle logic for interpreting DateString
    //     if (!string.IsNullOrEmpty(searchParams.DateString))
    //     {
    //         if (searchParams.DateString.Length == 4 && int.TryParse(searchParams.DateString, out var year))
    //         {
    //             // If DateString is a year, set start and end of the year
    //             startDate = new DateTime(year, 1, 1);
    //             endDate = new DateTime(year, 12, 31);
    //         }
    //         else if (DateTime.TryParseExact(searchParams.DateString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var exactDate))
    //         {
    //             // If DateString is a full date, filter by that exact date
    //             startDate = exactDate;
    //             endDate = exactDate;
    //         }
    //     }
    //
    //     // Fetch filtered races (based on other parameters, and optionally the date range)
    //     var races = await _raceRepository.GetAllAsync(r => 
    //         (!searchParams.RaceId.HasValue || r.RaceId == searchParams.RaceId) &&
    //         (!startDate.HasValue || (r.Date >= startDate && r.Date <= endDate)) &&
    //         (!searchParams.Distance.HasValue || r.Distance == searchParams.Distance));
    //
    //     return races;
    // }
}