using Emne7_Eksamen.Features.Members;
using Emne7_Eksamen.Features.Members.Interfaces;
using Emne7_Eksamen.Features.Members.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using Xunit;

namespace Emne7_Eksamen.UnitTests.Features;

public class MemberControllerTests
{
    private readonly MemberController _memberController;
    private readonly Mock<ILogger<MemberController>> _loggerMock = new();
    private readonly Mock<IMemberService> _memberServiceMock = new();

    public MemberControllerTests()
    {
        _memberController = new MemberController(_loggerMock.Object, _memberServiceMock.Object);
    }

    [Fact]
    public async Task UpdateAsync_When_MemberIsValid_Should_Return_UpdatedMember()
    {
        // arrange
        MemberDTO member = new MemberDTO()
        {
            MemberId = 1,
            FirstName = "Jonas",
            LastName = "Jonasen",
            Gender = 'M',
            BirthYear = 1990,
            Created = new DateOnly(2024, 12, 17),
            Updated = new DateOnly(2024, 12, 17),
        };
        
        MemberUpdateDTO memberUpdateDto = new MemberUpdateDTO()
        {
            FirstName = "Testin",
            LastName = "test",
            Gender = 'M',
            BirthYear = 1990,
        };
        
        MemberDTO updatedMemberDto = new MemberDTO()
        {
            MemberId = member.MemberId,  
            FirstName = memberUpdateDto.FirstName,
            LastName = memberUpdateDto.LastName,
            Gender = memberUpdateDto.Gender,
            BirthYear = memberUpdateDto.BirthYear,
            Created = member.Created, 
            Updated = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        
        

        _memberServiceMock.Setup(x => x.UpdateAsync(1, memberUpdateDto))
            .ReturnsAsync(updatedMemberDto);
        
        // act
        var result = await _memberController.UpdateMemberAsync(1, memberUpdateDto);
        
        // assert
        var actionResult = Assert.IsType<ActionResult<MemberDTO>>(result);
        var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        var memberDto = Assert.IsType<MemberDTO>(returnValue.Value);

        var dto = memberDto;
        Assert.NotNull(dto);
        Assert.Equal(dto.MemberId, updatedMemberDto.MemberId);
        Assert.Equal(dto.FirstName, updatedMemberDto.FirstName);
        Assert.Equal(dto.LastName, updatedMemberDto.LastName);
        Assert.Equal(dto.Gender, updatedMemberDto.Gender);
        Assert.Equal(dto.BirthYear, updatedMemberDto.BirthYear);
        Assert.Equal(dto.Created, updatedMemberDto.Created);
        Assert.Equal(dto.Updated, updatedMemberDto.Updated);
    }
}