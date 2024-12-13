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
        _logger.LogInformation($"Adding: {entity.RaceId}, with MemberId: {entity.MemberId}, and Time: {entity.Time}");
        await _dbContext.Results.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Result?> UpdateAsync(Result entity)
    {
        _logger.LogInformation($"Updating: {entity.RaceId}");
        var result = await _dbContext.Results.FirstOrDefaultAsync(p => p.RaceId == entity.RaceId);
        if (result == null) return null;

        _logger.LogInformation($"Updating: {result.MemberId} to {entity.MemberId}");
        result.MemberId = entity.MemberId;
        _logger.LogInformation($"Updating: {result.Time} to {entity.Time}");
        result.Time = entity.Time;

        _dbContext.Results.Update(result);
        await _dbContext.SaveChangesAsync();

        return result;
    }

    public async Task<Result?> DeleteByIdAsync(int id)
    {
        _logger.LogInformation($"Deleting ResultId: {id}");
        var result = await _dbContext.Results.FindAsync(id);
        if (result == null) return null;

        _dbContext.Results.Remove(result);
        await _dbContext.SaveChangesAsync();

        return result;
    }

    public async Task<Result?> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Executing a GET on: {id}");
        return await _dbContext.Results.FirstOrDefaultAsync(c => c.RaceId == id);
    }

    public async Task<IEnumerable<Result>> FindAsync(Expression<Func<Result, bool>> predicate)
    {
        _logger.LogInformation($"Trying to find: {predicate} in Results.");
        return await _dbContext.Results
            .Where(predicate)
            .ToListAsync();
    }
}