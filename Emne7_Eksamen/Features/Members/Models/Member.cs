using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Emne7_Eksamen.Features.Races;
using Emne7_Eksamen.Features.Results;

namespace Emne7_Eksamen.Features.Members.Models;

public class Member
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int MemberId { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public char Gender { get; set; }
    
    public short BirthYear { get; set; }
    
    [Column(TypeName = "DATE")]
    public DateOnly Created { get; set; }
    
    [Column(TypeName = "DATE")]
    public DateOnly Updated { get; set; }
    
    [Column(TypeName = "LONGTEXT")]
    public string HashedPassword { get; set; } = string.Empty;
    
    
    public virtual ICollection<Result> Results { get; set; } = new HashSet<Result>();
    public virtual ICollection<Race> Races { get; set; } = new HashSet<Race>();
}