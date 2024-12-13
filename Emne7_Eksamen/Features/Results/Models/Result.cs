using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emne7_Eksamen.Features.Results;

public class Result
{
    [ForeignKey("RaceId")]
    public int RaceId { get; set; }
    
    [ForeignKey("MemberId")]
    public int MemberId { get; set; }
    
    public float? Time { get; set; }
}