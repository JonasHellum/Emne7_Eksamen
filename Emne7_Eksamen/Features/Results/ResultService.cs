using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Interfaces;
using Emne7_Eksamen.Features.Members.Models;
using Emne7_Eksamen.Features.Results.Interfaces;

namespace Emne7_Eksamen.Features.Results;

public class ResultService : IResultService
{
    private readonly ILogger<ResultService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper<Result, ResultDTO> _resultMapper;
    private readonly IResultRepository _resultRepository;
    private readonly IMemberRepository _memberRepository;
    
    public async Task<ResultDTO?> AddAsync<TAdd>(TAdd addDto) where TAdd : class
    {
        throw new NotImplementedException();
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

    public async Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
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