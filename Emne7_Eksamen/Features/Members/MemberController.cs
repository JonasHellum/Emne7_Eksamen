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

    [HttpPost("register", Name = "RegisterMemberAsync")]
    public async Task<ActionResult<MemberDTO>> RegisterMemberAsync(MemberRegistrationDTO registrationDTO)
    {
        var member = await _memberService.RegistrationAsync(registrationDTO);
        return member is null
            ? BadRequest("Failed to register new user")
            : Ok(member);
    }
}