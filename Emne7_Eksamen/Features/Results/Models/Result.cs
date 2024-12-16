using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Emne7_Eksamen.Features.Members.Models;
using Emne7_Eksamen.Features.Races;

namespace Emne7_Eksamen.Features.Results;

public class Result
{
    [ForeignKey("RaceId")]
    public int RaceId { get; set; }
    
    [ForeignKey("MemberId")]
    public int MemberId { get; set; }
    
    public float? Time { get; set; }
    
    
    // Navigation properties
    public virtual Race? Race { get; set; }
    public virtual Member? Member { get; set; }
}