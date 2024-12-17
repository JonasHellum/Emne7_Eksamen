using Emne7_Eksamen.Features.Members.Interfaces;
using Emne7_Eksamen.Features.Members.Models;
using Microsoft.AspNetCore.Mvc;

namespace Emne7_Eksamen.Features.Members;



[ApiController]
[Route("api/v1/members")]
public class MemberController : ControllerBase
{
    private readonly ILogger<MemberController> _logger;
    private readonly IMemberService _memberService;

    public MemberController(ILogger<MemberController> logger,
        IMemberService memberService)
    {
        _logger = logger;
        _memberService = memberService;
    }

    [HttpPost("Register", Name = "RegisterMemberAsync")]
    public async Task<ActionResult<MemberDTO>> RegisterMemberAsync([FromBody] MemberRegistrationDTO registrationDTO)
    {
        var member = await _memberService.RegistrationAsync(registrationDTO);
        return member is null
            ? BadRequest("Failed to register new user")
            : Ok(member);
    }

    [HttpGet("GetMembers", Name = "GetMembersAsync")]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembersAsync(
        [FromQuery] MemberSearchParams? searchParams,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (searchParams?.MemberId is null &&
            searchParams?.FirstName is null &&
            searchParams?.LastName is null)
        {
            var memberDtos = await _memberService.GetPagedAsync(pageNumber,pageSize);
            return Ok(memberDtos);
        }
        return Ok(await _memberService.FindAsync(searchParams));
    }

    [HttpPut("{memberId}", Name = "UpdateMemberAsync")]
    public async Task<ActionResult<MemberDTO>> UpdateMemberAsync(int memberId, [FromBody] MemberUpdateDTO memberDTO)
    {
        _logger.LogInformation($"Updating member: {memberId}");
        var updatedMember = await _memberService.UpdateAsync(memberId, memberDTO);
        
        return updatedMember is null
            ? BadRequest("Failed to update member")
            : Ok(updatedMember);
    }
    
    [HttpDelete("{memberId}", Name = "DeleteMemberAsync")]
    public async Task<ActionResult<bool>> DeleteMemberAsync(int memberId)
    {
        _logger.LogInformation($"Deleting member with id: {memberId}");
        var result = await _memberService.DeleteByIdAsync(memberId);
        
        return result
            ? Ok(result)
            : BadRequest($"Failed to delete user with id: {memberId}");
    }
}