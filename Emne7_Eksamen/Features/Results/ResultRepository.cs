using System.Linq.Expressions;
using Emne7_Eksamen.Data;
using Emne7_Eksamen.Features.Results.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emne7_Eksamen.Features.Results;


public class ResultRepository : IResultRepository
{
    private readonly ILogger<ResultRepository> _logger;
    private readonly GokstadAthleticsDbContext _dbContext;
    
    public ResultRepository(ILogger<ResultRepository> logger, 
        GokstadAthleticsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<Result?> AddAsync(Result entity)
    {
        _logger.LogDebug($"Adding new result with id: {entity.RaceId}, " +
                         $"MemberId: {entity.MemberId} and Time: {entity.Time}");
        await _dbContext.Result.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"Added new result with id: {entity.RaceId}");
        return entity;
    }

    public async Task<Result?> UpdateAsync(Result entity)
    {
        _logger.LogDebug($"Finding result based on id: {entity.RaceId}");
        var result = await _dbContext.Result.FindAsync(entity.RaceId, entity.MemberId);
        if (result == null)
        {
            _logger.LogWarning($"Result with RaceId: {entity.RaceId} and MemberId: {entity.MemberId} not found.");
            return null;
        }

        _logger.LogDebug($"Updating: {result.Time} to {entity.Time}");

        _dbContext.Result.Update(result);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Updated result based on id: {entity.RaceId}");
        return result;
    }
    

    public async Task<Result?> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException("Use DeleteByIdAsync with two parameters (RaceId, MemberId)");
    }

    public async Task<Result?> DeleteByIdAsync(params object[] keyValues)
    {
        if (keyValues.Length != 2)
        {
            throw new ArgumentException("Composite key requires two values: RaceId and MemberId.");
        }
        
        int raceId = (int) keyValues[0];
        int memberId = (int) keyValues[1];
        
        _logger.LogDebug($"Finding result based on RaceId: {raceId} and MemberId: {memberId}");
        var result = await _dbContext.Result.FindAsync(raceId, memberId);
        if (result == null)
        {
            _logger.LogWarning($"Result with RaceId: {raceId} and MemberId: {memberId} not found.");
            return null;
        }
        
        _logger.LogInformation($"Deleting result based on RaceId: {raceId} and MemberId: {memberId}");
        _dbContext.Result.Remove(result);
        await _dbContext.SaveChangesAsync();

        return result;
    }

    public async Task<Result?> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Getting result from id: {id}");
        return await _dbContext.Result.FirstOrDefaultAsync(c => c.RaceId == id);
    }

    public async Task<IEnumerable<Result>> FindAsync(Expression<Func<Result, bool>> predicate)
    {
        _logger.LogInformation($"Finding: {predicate} in Result.");
        return await _dbContext.Result
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Result>> GetPagedAsync(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;

        _logger.LogInformation($"Getting paged results from page: {pageNumber} with size: {pageSize}");
        var results = await _dbContext.Result
            .OrderBy(r => r.RaceId)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return results;
    }

    public async Task<Result?> GetByRaceAndMemberIdAsync(int raceId, int memberId)
    {
        _logger.LogInformation($"Getting result from raceId: {raceId} and memberId: {memberId}");
        return await _dbContext.Result.FirstOrDefaultAsync(r => r.RaceId == raceId && r.MemberId == memberId);
    }
}