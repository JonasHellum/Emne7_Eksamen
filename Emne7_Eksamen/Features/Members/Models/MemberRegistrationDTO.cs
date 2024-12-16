namespace Emne7_Eksamen.Features.Members.Models;

public class MemberRegistrationDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public char Gender { get; set; }
    public short BirthYear { get; set; }
    public string Password { get; set; }
}