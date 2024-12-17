using System.Text.Json.Serialization;

namespace Emne7_Eksamen.Features.Races;

public class RaceUpdateDTO
{
    [JsonIgnore]
    public DateTime? Date { get; set; }
    
    [JsonPropertyName("Date")]
    public string? DateString => Date?.ToString("yyyy-MM-dd");
    
    public short? Distance { get; set; }
}