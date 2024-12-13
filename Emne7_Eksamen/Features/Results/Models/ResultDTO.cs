using Emne7_Eksamen.Features.Results.Interfaces;

namespace Emne7_Eksamen.Features.Results;

public class ResultDTO
{
    
    public int RaceId { get; set; }
    public int MemberId { get; set; }
    public float? Time { get; set; }
}

public class ResultUpdateDTO
{
    public int RaceId { get; set; }
}