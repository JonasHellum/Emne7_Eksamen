using Emne7_Eksamen.Features.Members.Models;
using Emne7_Eksamen.Features.Races;
using Emne7_Eksamen.Features.Results;
using Microsoft.EntityFrameworkCore;

namespace Emne7_Eksamen.Data;

public class GokstadAthleticsDbContext : DbContext
{
    public GokstadAthleticsDbContext(DbContextOptions<GokstadAthleticsDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Member> Members { get; set; }
    public DbSet<Result> Results { get; set; }
    public DbSet<Race> Races { get; set; }
    
}