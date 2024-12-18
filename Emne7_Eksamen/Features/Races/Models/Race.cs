using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Emne7_Eksamen.Features.Members.Models;
using Emne7_Eksamen.Features.Results;

namespace Emne7_Eksamen.Features.Races;

public class Race
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int RaceId { get; set; }
    
    [ForeignKey("MemberId")]
    public int MemberId { get; set; }
    
    [Required]
    [Column(TypeName = "DATE")]
    public DateOnly Date { get; set; }
    
    [Required]
    public short Distance { get; set; }
    
    
    // Navigation properties
    public virtual ICollection<Result> Results { get; set; } = new HashSet<Result>();
    public virtual Member? Member { get; set; }
}