using System.Linq.Expressions;
using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Interfaces;
using Emne7_Eksamen.Features.Members.Models;

namespace Emne7_Eksamen.Features.Members;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<MemberService> _logger;
    private readonly IMapper<Member, MemberDTO> _memberMapper;
    private readonly IMapper<Member, MemberRegistrationDTO> _registrationMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public MemberService(IMemberRepository memberRepository,
        ILogger<MemberService> logger,
        IMapper<Member, MemberDTO> memberMapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _memberRepository = memberRepository;
        _logger = logger;
        _memberMapper = memberMapper;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<bool> DeleteByIdAsync(int id)
    {
        _logger.LogInformation($"Trying to find logged in member.");
        var loggedMember = await GetLoggedInMemberAsync();
        if (loggedMember is null)
        {
            _logger.LogWarning("Member is not authorized.");
            throw new UnauthorizedAccessException("Member is not authorized");
        }

        if (loggedMember.MemberId != id)
        {
            _logger.LogWarning($"Member with id: {loggedMember.MemberId} is not authorized to delete member with id: {id}");
            throw new UnauthorizedAccessException($"Member with id: {loggedMember.MemberId} is not authorized to delete " +
                                                  $"member with id: {id}");
        }
        
        _logger.LogInformation($"Deleting member with id: {id}");
        var deletedMember = await _memberRepository.DeleteByIdAsync(id);

        if (deletedMember == null)
        {
            _logger.LogWarning($"Member with id: {id} not found.");
            throw new KeyNotFoundException($"Member with id: {id} not found.");
        }
        
        return true;
    }

    public async Task<MemberDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<MemberDTO?>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var loggedMember = await GetLoggedInMemberAsync();
        if (loggedMember is null)
        {
            throw new UnauthorizedAccessException("Member is not authorized");
        }
        
        var members = await _memberRepository.GetPagedAsync(pageNumber, pageSize);

        return members
            .Select(mem => _memberMapper.MapToDTO(mem))
            .ToList();
    }

    public async Task<MemberDTO?> RegistrationAsync(MemberRegistrationDTO registrationDTO)
    {
        var member = _registrationMapper.MapToModel(registrationDTO);
        
        member.Created = DateTime.UtcNow;
        member.Updated = DateTime.UtcNow;
        
        member.HashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationDTO.Password);
        
        var addedMember = await _memberRepository.AddAsync(member);

        if (addedMember is null) 
            return null;
        
        return _memberMapper.MapToDTO(addedMember);
    }

    public async Task<MemberDTO?> UpdateAsync(int id, MemberUpdateDTO updateDTO)
    {
        _logger.LogInformation($"Trying to find logged in member.");
        var loggedMember = await GetLoggedInMemberAsync();
        if (loggedMember is null)
        {
            _logger.LogWarning("Member is not authorized.");
            throw new UnauthorizedAccessException("Member is not authorized");
        }
        
        _logger.LogInformation($"Trying to find member based on id: {id}");
        var memberToUpdate = await _memberRepository.GetByIdAsync(id);
        if (memberToUpdate is null)
        {
            _logger.LogWarning($"Member with id: {id} not found.");
            throw new KeyNotFoundException($"Member with id: {id} not found.");
        }

        _logger.LogInformation($"Checking if member with id: {memberToUpdate.MemberId} is " +
                               $"authorized to update member with id: {id}");
        if (memberToUpdate.MemberId != loggedMember.MemberId)
        {
            _logger.LogWarning($"Member with id: {loggedMember.MemberId} is not authorized to update " +
                               $"member with id: {memberToUpdate.MemberId}");
            throw new UnauthorizedAccessException($"Member with id: {loggedMember.MemberId} is not authorized to update " +
                                                  $"member with id: {memberToUpdate.MemberId}");
        }
        
        memberToUpdate.FirstName = updateDTO.FirstName;
        memberToUpdate.LastName = updateDTO.LastName;
        memberToUpdate.Gender = updateDTO.Gender;
        memberToUpdate.BirthYear = updateDTO.BirthYear;
        memberToUpdate.Updated = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(updateDTO.Password))
        {
            memberToUpdate.HashedPassword = BCrypt.Net.BCrypt.HashPassword(updateDTO.Password);
        }
        
        var updatedMember = await _memberRepository.UpdateAsync(memberToUpdate);
        return updatedMember is null
            ? null
            : _memberMapper.MapToDTO(updatedMember);
    }

    public async Task<int?> AuthenticateUserAsync(string firstName, string password)
    {
        Expression<Func<Member, bool>> expr = user => user.FirstName == firstName;
        var usr = (await _memberRepository.FindAsync(expr)).FirstOrDefault();
        if (usr is null) return null;

        // sjekker om passord stemmer !!
        if (BCrypt.Net.BCrypt.Verify(password, usr.HashedPassword))
            return usr.MemberId;
        
        return null;
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