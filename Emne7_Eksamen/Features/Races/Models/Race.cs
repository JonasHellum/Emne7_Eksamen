using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emne7_Eksamen.Features.Races;

public class Race
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int RaceId { get; set; }
    
    [Required]
    [Column(TypeName = "DATE")]
    public DateTime Date { get; set; }
    
    [Required]
    public short Distance { get; set; }
}