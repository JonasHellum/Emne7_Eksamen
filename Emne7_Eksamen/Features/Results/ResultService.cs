using System.Data;
using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Interfaces;
using Emne7_Eksamen.Features.Members.Models;
using Emne7_Eksamen.Features.Races.Interfaces;
using Emne7_Eksamen.Features.Results.Interfaces;

namespace Emne7_Eksamen.Features.Results;

public class ResultService : IResultService
{
    private readonly ILogger<ResultService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper<Result, ResultDTO> _resultMapper;
    private readonly IMapper<Result, ResultRegistrationDTO> _registrationMapper;
    private readonly IResultRepository _resultRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IRaceRepository _raceRepository;

    public ResultService(
        ILogger<ResultService> logger, 
        IHttpContextAccessor httpContextAccessor, 
        IMapper<Result, ResultDTO> resultMapper,
        IMapper<Result, ResultRegistrationDTO> registrationMapper,
        IResultRepository resultRepository, 
        IMemberRepository memberRepository,
        IRaceRepository raceRepository)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _resultMapper = resultMapper;
        _registrationMapper = registrationMapper;
        _resultRepository = resultRepository;
        _memberRepository = memberRepository;
        _raceRepository = raceRepository;
    }


    public async Task<bool> DeleteByIdAsync(int raceId)
    {
        _logger.LogInformation($"Trying to delete result with id: {raceId}.");
        var loggedMember = await GetLoggedInMemberAsync();
        
        _logger.LogDebug($"Trying to find result to delete by id: {raceId}");
        var resultToDelete = await _resultRepository.GetByRaceAndMemberIdAsync(raceId, loggedMember.MemberId);
        if (resultToDelete is null)
        {
            _logger.LogWarning($"Result with raceId: {raceId} and memberId: {loggedMember.MemberId} not found.");
            throw new KeyNotFoundException($"Result with raceId: {raceId} and memberId: {loggedMember.MemberId} not found.");
        }
        
        _logger.LogDebug($"Checking if member with id: {loggedMember.MemberId} is " +
                         $"authorized to delete result with raceId: {resultToDelete.RaceId} " +
                         $"based on {resultToDelete.MemberId}");
        if (loggedMember.MemberId != resultToDelete.MemberId)
        {
            _logger.LogWarning($"Member with id: {loggedMember.MemberId} is not authorized " +
                               $"to delete result with RaceId: {resultToDelete.RaceId}");
            throw new UnauthorizedAccessException($"Member with id: {loggedMember.MemberId} is not authorized to delete " +
                                                  $"result with RaceId: {resultToDelete.RaceId}");
        }
        
        var deletedResult = await _resultRepository.DeleteByIdAsync(raceId, resultToDelete.MemberId);
        
        if (deletedResult == null)
        {
            _logger.LogWarning($"Did not delete result with raceId: {raceId}.");
            throw new KeyNotFoundException($"Did not delete result with raceId: {raceId}.");
        }
        
        return true;
    }
    
    public async Task<ResultDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<IEnumerable<ResultDTO?>> GetPagedAsync(int pageNumber, int pageSize)
    {
        _logger.LogInformation($"Trying to get paged results with page number: {pageNumber} and page size: {pageSize}");
        var results = await _resultRepository.GetPagedAsync(pageNumber, pageSize);

        return results
            .Select(rac => _resultMapper.MapToDTO(rac))
            .ToList();
    }

    public async Task<ResultDTO?> RegistrationAsync(int raceId, ResultRegistrationDTO registrationDTO)
    {
        var loggedMember = await GetLoggedInMemberAsync();
        
        var race = await _raceRepository.GetByIdAsync(raceId);
        if (race is null)
        {
            _logger.LogWarning($"Race with id: {raceId} not found.");
            throw new KeyNotFoundException($"Race with id: {raceId} not found.");
        }
        
        var existingResult = await _resultRepository.FindAsync(
            r => r.RaceId == raceId && r.MemberId == loggedMember.MemberId);
        if (existingResult.Any())
        {
            _logger.LogWarning($"Member with id: {loggedMember.MemberId} already registered for race with id: {raceId}");
            throw new InvalidOperationException($"Member with id: {loggedMember.MemberId} already registered for race with id: {raceId}");
        }
        
        var result = _registrationMapper.MapToModel(registrationDTO);
        
        _logger.LogInformation($"Trying to add a new result with id: {raceId} " +
                               $"and member with id: {loggedMember.MemberId}");
        result.RaceId = raceId;
        result.MemberId = loggedMember.MemberId;
        
        var addedResult = await _resultRepository.AddAsync(result);
        if (addedResult is null)
        {
            _logger.LogError("Failed to add result");
            throw new DataException("Failed to add result");
        }
        
        return _resultMapper.MapToDTO(addedResult);
    }

    public async Task<ResultDTO?> UpdateAsync(int raceId, ResultUpdateDTO updateDto)
    {
        var loggedMember = await GetLoggedInMemberAsync();
        if (loggedMember is null)
        {
            throw new UnauthorizedAccessException("Member is not authorized");
        }
    
        var resultToUpdate = await _resultRepository.GetByIdAsync(raceId);
        if (resultToUpdate is null)
        {
            throw new KeyNotFoundException($"Result with id: {raceId} not found.");
        }
    
        if (resultToUpdate.MemberId == loggedMember.MemberId)
        {
            resultToUpdate.RaceId = updateDto.RaceId;
            
            var updatedResult = await _resultRepository.UpdateAsync(resultToUpdate);
            
            return updatedResult is null
                ? null
                : _resultMapper.MapToDTO(updatedResult);
        }
        
        throw new UnauthorizedAccessException($"User {loggedMember.MemberId} is not authorized to update this race with id {resultToUpdate.RaceId}");
    }

    public async Task<IEnumerable<ResultDTO>> FindAsync(ResultSearchParams searchParams)
    {
        throw new NotImplementedException();
    }


    private async Task<Member?> GetLoggedInMemberAsync()
    {
        var loggedInMemberId = _httpContextAccessor.HttpContext?.Items["UserId"] as string;
        _logger.LogInformation($"Logged in member ID: {loggedInMemberId}");
    
        if (string.IsNullOrEmpty(loggedInMemberId))
        {
            _logger.LogWarning("No logged in member.");
            throw new UnauthorizedAccessException("No logged in member.");
            
        }
        
        var loggedInMember = (await _memberRepository.FindAsync(m => m.MemberId.ToString() == loggedInMemberId)).FirstOrDefault();
        if (loggedInMember == null)
        {
            _logger.LogWarning($"Logged in member not found: {loggedInMemberId}");
            throw new UnauthorizedAccessException("Logged in member ID not found.");
            
        }
        
        return loggedInMember;
    }
}