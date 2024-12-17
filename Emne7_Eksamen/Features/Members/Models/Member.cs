using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Emne7_Eksamen.Features.Results;

namespace Emne7_Eksamen.Features.Members.Models;

public class Member
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int MemberId { get; set; }

    [Required]
    [MinLength(2), MaxLength(30)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MinLength(2), MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    public char Gender { get; set; }
    
    [Required]
    public short BirthYear { get; set; }
    
    [Required]
    [Column(TypeName = "DATE")]
    public DateTime Created { get; set; }
    
    [Required]
    [Column(TypeName = "DATE")]
    public DateTime Updated { get; set; }
    
    [Required]
    [Column(TypeName = "LONGTEXT")]
    public string HashedPassword { get; set; } = string.Empty;
    
    
    // Navigation properties
    public virtual ICollection<Result> Results { get; set; } = new HashSet<Result>();
}