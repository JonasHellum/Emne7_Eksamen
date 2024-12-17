using System.Linq.Expressions;
using Emne7_Eksamen.Data;
using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Races.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emne7_Eksamen.Features.Races;

public class RaceRepository : IRaceRepository
{
    private readonly ILogger<RaceRepository> _logger;
    private readonly GokstadAthleticsDbContext _dbContext;
    
    public RaceRepository(ILogger<RaceRepository> logger, GokstadAthleticsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<Race?> AddAsync(Race entity)
    {
        _logger.LogInformation($"Adding new race with Id: {entity.RaceId}, " +
                               $"FirstName: {entity.Date}, LastName: {entity.Distance}");
        await _dbContext.Race.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Race?> UpdateAsync(Race entity)
    {
        _logger.LogInformation($"Trying to find race based on id: {entity.RaceId}");
        var race = await _dbContext.Race.FirstOrDefaultAsync(r => r.RaceId == entity.RaceId);
        if (race == null) return null;

        _logger.LogInformation($"Updating member based on id: {entity.RaceId}");
        _dbContext.Race.Update(race);
        await _dbContext.SaveChangesAsync();

        return race;
    }

    public async Task<Race?> DeleteByIdAsync(int id)
    {
        _logger.LogInformation($"Trying to find race based on id: {id}");
        var race = await _dbContext.Race.FindAsync(id);
        if (race == null) return null;

        _logger.LogInformation($"Deleting race based on id: {id}");
        _dbContext.Race.Remove(race);
        await _dbContext.SaveChangesAsync();

        return race;
    }

    public async Task<Race?> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Getting race from id: {id}");
        return await _dbContext.Race.FirstOrDefaultAsync(r => r.RaceId == id);
    }

    public async Task<IEnumerable<Race>> FindAsync(Expression<Func<Race, bool>> predicate)
    {
        _logger.LogInformation($"Trying to find: {predicate} in race.");
        return await _dbContext.Race
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Race>> GetPagedAsync(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;

        var races = await _dbContext.Race
            .OrderBy(r => r.RaceId)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return races;
    }
}