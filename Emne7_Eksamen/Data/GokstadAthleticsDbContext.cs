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
    
    public DbSet<Member> Member { get; set; }
    public DbSet<Result> Result { get; set; }
    public DbSet<Race> Race { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(r => new { r.RaceId, r.MemberId }); // Defines a composite key
        });
    }
    
}