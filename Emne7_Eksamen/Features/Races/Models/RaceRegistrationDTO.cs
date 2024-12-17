using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;

namespace Emne7_Eksamen.Features.Races;

public class RaceRegistrationDTO
{
    
    public DateOnly Date { get; set; }
    
    public short Distance { get; set; }
}