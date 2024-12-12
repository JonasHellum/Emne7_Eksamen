using System.Text.Json.Serialization;

namespace Emne7_Eksamen.Features.Members.Models;

public class MemberDTO
{
    public int MemberId { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public char Gender { get; set; }
    
    public short BirthYear { get; set; }
    
    [JsonIgnore]
    public DateTime Created { get; set; }
    
    [JsonIgnore]
    public DateTime Updated { get; set; }
    
    [JsonPropertyName("Created")]
    public string CreatedDate => Created.ToString("yyyy-MM-dd");
    [JsonPropertyName("Updated")]
    public string UpdatedDate => Updated.ToString("yyyy-MM-dd");
    
    public string HashedPassword { get; set; }
    
}