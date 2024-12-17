using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Emne7_Eksamen.Features.Races;

public class RaceSearchParams
{
    public int? RaceId { get; set; }
    
    public DateOnly? Date { get; set; }
    
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }

    public short? Distance { get; set; }
}