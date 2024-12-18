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
        _logger.LogDebug($"Adding new race with Id: {entity.RaceId}, " +
                               $"MemberId: {entity.MemberId}, Date: {entity.Date}, " +
                               $"Distance: {entity.Distance}");
        await _dbContext.Race.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"Added new race with Id: {entity.RaceId}");
        return entity;
    }

    public async Task<Race?> UpdateAsync(Race entity)
    {
        _logger.LogDebug($"Finding race based on id: {entity.RaceId}");
        var race = await _dbContext.Race.FirstOrDefaultAsync(r => r.RaceId == entity.RaceId);
        if (race == null) return null;
        
        _logger.LogDebug($"Updating race with id: {entity.RaceId} with current values: " +
                         $"from: {race.Date} to: {entity.Date} " +
                         $"from: {race.Distance} to: {entity.Distance}");
        
        _dbContext.Race.Update(race);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Updated member based on id: {entity.RaceId}");
        return race;
    }

    public async Task<Race?> DeleteByIdAsync(int id)
    {
        _logger.LogDebug($"Finding race based on id: {id}");
        var race = await _dbContext.Race.FindAsync(id);
        if (race == null) return null;

        _logger.LogInformation($"Deleting race based on id: {id}");
        _dbContext.Race.Remove(race);
        await _dbContext.SaveChangesAsync();

        return race;
    }

    public async Task<Race?> DeleteByIdAsync(params object[] keyValues)
    {
        throw new NotImplementedException("Use DeleteByIdAsync with one paramter (RaceId)");
    }

    public async Task<Race?> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Getting race from id: {id}");
        return await _dbContext.Race.FirstOrDefaultAsync(r => r.RaceId == id);
    }

    public async Task<IEnumerable<Race>> FindAsync(Expression<Func<Race, bool>> predicate)
    {
        _logger.LogInformation($"Finding: {predicate} in race.");
        return await _dbContext.Race
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Race>> GetPagedAsync(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;

        _logger.LogInformation($"Getting paged races from page: {pageNumber} with size: {pageSize}");
        var races = await _dbContext.Race
            .OrderBy(r => r.RaceId)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return races;
    }
}