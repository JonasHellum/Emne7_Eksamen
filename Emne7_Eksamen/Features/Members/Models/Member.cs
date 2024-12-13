using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emne7_Eksamen.Features.Members.Models;

public class Member
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int MemberId { get; set; }
    
    [Required]
    [MinLength(2), MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MinLength(2), MaxLength(50)]
    public string LastName { get; set; }
    
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
    public string HashedPassword { get; set; }
}