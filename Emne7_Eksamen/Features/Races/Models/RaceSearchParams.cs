using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Emne7_Eksamen.Features.Races;

public class RaceSearchParams
{
    public int? RaceId { get; set; }
    
    public DateTime? Date { get; set; }
    

    public short? Distance { get; set; }
}