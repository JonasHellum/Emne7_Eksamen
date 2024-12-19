using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Mappers;
using Emne7_Eksamen.Features.Members.Models;
using Xunit;

namespace Emne7_Eksamen.UnitTests.Features;

public class MemberMapperTests
{
    private readonly IMapper<Member, MemberDTO> _memberMapper = new MemberMapper();
    
    [Fact]
    public void MapToDTO_When_MemberModelIsValid_Should_Return_MemberDTO()
    {
        // arrange
        Member member = new Member()
        {
            MemberId = 1,
            FirstName = "Jonas",
            LastName = "Jonasen",
            Gender = 'M',
            BirthYear = 1990,
            Created = new DateOnly(2024, 12, 17),
            Updated = new DateOnly(2024, 12, 17),
            HashedPassword = "kyugirdstiku7drxtyhgyrflcikgh8uftcoik7ubvtnilrdf7gnvtk"
        };
        
        // act
        MemberDTO dto = _memberMapper.MapToDTO(member);

        
        // assert
        Assert.NotNull(dto);
        Assert.Equal(member.MemberId, dto.MemberId);
        Assert.Equal(member.FirstName, dto.FirstName);
        Assert.Equal(member.LastName, dto.LastName);
        Assert.Equal(member.Gender, dto.Gender);
        Assert.Equal(member.BirthYear, dto.BirthYear);
        Assert.Equal(member.Created, dto.Created);
        Assert.Equal(member.Updated, dto.Updated);
    }

    [Fact]
    public void MapToModel_When_MemberDTOIsValid_Should_Return_Member()
    {
        // arrange
        MemberDTO dto = new MemberDTO()
        {
            MemberId = 1,
            FirstName = "Jonas",
            LastName = "Jonasen",
            Gender = 'M',
            BirthYear = 1990,
            Created = new DateOnly(2024, 12, 17),
            Updated = new DateOnly(2024, 12, 17),
        };
        
        // act
        Member member = _memberMapper.MapToModel(dto);
        
        // assert
        Assert.NotNull(member);
        Assert.Equal(dto.MemberId, member.MemberId);
        Assert.Equal(dto.FirstName, member.FirstName);
        Assert.Equal(dto.LastName, member.LastName);
        Assert.Equal(dto.Gender, member.Gender);
        Assert.Equal(dto.BirthYear, member.BirthYear);
        Assert.Equal(dto.Created, member.Created);
        Assert.Equal(dto.Updated, member.Updated);
    }
}