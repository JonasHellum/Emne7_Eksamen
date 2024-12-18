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
        _logger.LogInformation($"Trying to delete race with id: {raceId}.");
        var loggedMember = await GetLoggedInMemberAsync();
        
        _logger.LogDebug($"Trying to find race to delete by id: {raceId}");
        var raceToDelete = await _raceRepository.GetByIdAsync(raceId);
        if (raceToDelete is null)
        {
            _logger.LogWarning($"Race with id: {raceId} not found.");
            throw new KeyNotFoundException($"Race with id: {raceId} not found.");
        }
        
        _logger.LogDebug($"Checking if member with id: {loggedMember.MemberId} is " +
                               $"authorized to delete race with id: {raceToDelete.RaceId} " +
                               $"based on {raceToDelete.MemberId}");
        if (loggedMember.MemberId != raceToDelete.MemberId)
        {
            _logger.LogWarning($"Member with id: {loggedMember.MemberId} is not authorized " +
                               $"to delete race with id: {raceToDelete.RaceId}");
            throw new UnauthorizedAccessException($"Member with id: {loggedMember.MemberId} is not authorized to delete " +
                                                  $"race with id: {raceToDelete.RaceId}");
        }
        
        var deletedRace = await _raceRepository.DeleteByIdAsync(raceId);
        
        if (deletedRace == null)
        {
            _logger.LogWarning($"Did not delete race with id: {raceId}.");
            throw new KeyNotFoundException($"Did not delete race with id: {raceId}.");
        }
        
        return true;
    }

    public async Task<RaceDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RaceDTO?>> GetPagedAsync(int pageNumber, int pageSize)
    {
        _logger.LogInformation($"Trying to get paged races with page number: {pageNumber} and page size: {pageSize}");
        var races = await _raceRepository.GetPagedAsync(pageNumber, pageSize);

        return races
            .Select(rac => _raceMapper.MapToDTO(rac))
            .ToList();
    }

    public async Task<RaceDTO?> RegistrationAsync(RaceRegistrationDTO registrationDTO)
    {
        var loggedMember = await GetLoggedInMemberAsync();
        
        var race = _registrationMapper.MapToModel(registrationDTO);
        
        _logger.LogInformation($"Trying to add a new race with id: {race.RaceId}");
        race.MemberId = loggedMember.MemberId;

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
        var loggedMember = await GetLoggedInMemberAsync();
        
        _logger.LogInformation($"Trying to update race by id: {raceId} by logged in member id: {loggedMember.MemberId}");
        
        _logger.LogDebug($"Trying to find race to delete by id: {raceId}");
        var raceToUpdate = await _raceRepository.GetByIdAsync(raceId);
        if (raceToUpdate is null)
        {
            _logger.LogWarning($"Race with id: {raceId} not found.");
            throw new KeyNotFoundException($"Race with id: {raceId} not found.");
        }
        
        _logger.LogDebug($"Checking if member with id: {loggedMember.MemberId} is " +
                               $"authorized to update race with id: {raceToUpdate.RaceId} " +
                               $"based on {raceToUpdate.MemberId}");
        if (loggedMember.MemberId != raceToUpdate.MemberId)
        {
            _logger.LogWarning($"Member with id: {loggedMember.MemberId} is not authorized " +
                               $"to update race with id: {raceToUpdate.RaceId}");
            throw new UnauthorizedAccessException($"Member with id: {loggedMember.MemberId} is not authorized to update " +
                                                  $"race with id: {raceToUpdate.RaceId}");
        }
        
        raceToUpdate.Date = updateDTO.Date;
        raceToUpdate.Distance = updateDTO.Distance;
        
        var updatedRace = await _raceRepository.UpdateAsync(raceToUpdate);
        
        return updatedRace is null
            ? null
            : _raceMapper.MapToDTO(updatedRace);
    }

    public async Task<IEnumerable<RaceDTO>> FindAsync(RaceSearchParams searchParams)
    {
        _logger.LogInformation($"Searching for races with: {searchParams}");
        Expression<Func<Race, bool>> predicate = r =>
            (!searchParams.RaceId.HasValue || r.RaceId == searchParams.RaceId) &&
            (!searchParams.Date.HasValue || r.Date == searchParams.Date) &&
            (!searchParams.Year.HasValue || r.Date.Year == searchParams.Year) &&
            (!searchParams.Month.HasValue || r.Date.Month == searchParams.Month) &&
            (!searchParams.Day.HasValue || r.Date.Day == searchParams.Day) &&
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
}