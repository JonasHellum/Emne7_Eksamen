using System.ComponentModel.DataAnnotations;

namespace Emne7_Eksamen.Features.Members.Models;

public class MemberRegistrationDTO
{
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public char Gender { get; set; }
    
    public short BirthYear { get; set; }
    
    public string Password { get; set; } = string.Empty;
}