using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Emne7_Eksamen.Features.Members.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Emne7_Eksamen.IntegrationTests.Features;

public class MemberUpdateTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public MemberUpdateTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task UpdateAsync_When_MemberIsValid_Should_Return_UpdatedMember()
    {
        // Arrange
        Member member = new Member()
        {
            // BasicBase64: 1:test = "MTp0ZXN0"
            MemberId = 1,
            FirstName = "Jonas",
            LastName = "Jonasen",
            Gender = 'M',
            BirthYear = 1990,
            Created = new DateOnly(2024, 12, 17),
            Updated = new DateOnly(2024, 12, 17),
            HashedPassword = "$2a$11$M.TzvrMtYnLBqgqBm7gzhuTVEdZTNWPa4RI4BlEhm55vkJL/1mgHi"
        };
        
        MemberUpdateDTO memberUpdateDto = new MemberUpdateDTO()
        {
            FirstName = "Testin",
            LastName = "test",
            Gender = 'M',
            BirthYear = 1997,
            Password = "test"
        };

        Member updatedMember = new Member()
        {
            MemberId = member.MemberId,
            FirstName = memberUpdateDto.FirstName,
            LastName = memberUpdateDto.LastName,
            Gender = memberUpdateDto.Gender,
            BirthYear = memberUpdateDto.BirthYear,
            Created = member.Created,
            Updated = DateOnly.FromDateTime(DateTime.UtcNow),
        };
        
        // Add Authentication Header
        _client.DefaultRequestHeaders.Add("Authorization", "Basic MTp0ZXN0");
        
        // var memb = (await _memberRepository.FindAsync(expr)).FirstOrDefault(); (MemberService:AuthenticateMemberAsync)
        _factory.MemberRepositoryMock
            .Setup(x => x.FindAsync(It.Is<Expression<Func<Member, bool>>>
                (expr => expr.Compile().Invoke(member))))
            .ReturnsAsync(new List<Member> { member }); 
        
        
        // var memberToUpdate = await _memberRepository.GetByIdAsync(id); (MemberService:UpdateAsync)
        _factory.MemberRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(member);
        
        // var updatedMember = await _memberRepository.UpdateAsync(memberToUpdate); (MemberService:UpdateAsync)
        _factory.MemberRepositoryMock
            .Setup(x => x.UpdateAsync(member))
            .ReturnsAsync(updatedMember);
        

        // Act
        var result = await _client.PutAsJsonAsync("/api/v1/members/1", memberUpdateDto);

        // Deserialize
        var memberDto = JsonConvert
                    .DeserializeObject<MemberDTO>(await result.Content.ReadAsStringAsync());
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        
        var dto = memberDto;
        
        Assert.NotNull(dto);
        Assert.Equal(dto.MemberId, updatedMember.MemberId);
        Assert.Equal(dto.FirstName, updatedMember.FirstName);
        Assert.Equal(dto.LastName, updatedMember.LastName);
        Assert.Equal(dto.Gender, updatedMember.Gender);
        Assert.Equal(dto.BirthYear, updatedMember.BirthYear);
        Assert.Equal(dto.Created, updatedMember.Created);
        Assert.Equal(dto.Updated, updatedMember.Updated);
    }
}