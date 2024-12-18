using System.Text.Json.Serialization;

namespace Emne7_Eksamen.Features.Races;

public class RaceDTO
{
    public int RaceId { get; set; }
    
    public int MemberId { get; set; }
    
    public DateOnly Date { get; set; }
    
    public short Distance { get; set; }
}