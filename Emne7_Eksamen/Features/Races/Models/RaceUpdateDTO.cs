using System.Text.Json.Serialization;

namespace Emne7_Eksamen.Features.Races;

public class RaceUpdateDTO
{
    public DateOnly Date { get; set; }
    
    public short Distance { get; set; }
}