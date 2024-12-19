using System.Linq.Expressions;
using Emne7_Eksamen.Data;
using Emne7_Eksamen.Features.Common.Interfaces;
using Emne7_Eksamen.Features.Members.Interfaces;
using Emne7_Eksamen.Features.Members.Models;
using Emne7_Eksamen.Features.Results;
using Emne7_Eksamen.Features.Results.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emne7_Eksamen.Features.Members;

public class MemberRepository : IMemberRepository
{
    private readonly ILogger<MemberRepository> _logger;
    private readonly GokstadAthleticsDbContext _dbContext;

    public MemberRepository(ILogger<MemberRepository> logger,
        GokstadAthleticsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<Member?> AddAsync(Member entity)
    {
        _logger.LogDebug($"Adding new member with Id: {entity.MemberId}, " +
                         $"FirstName: {entity.FirstName}, LastName: {entity.LastName}," +
                         $"Gender: {entity.Gender}, BirthYear: {entity.BirthYear}," +
                         $"Created: {entity.Created}, Updated: {entity.Updated}");
        await _dbContext.Member.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation($"Added new member with Id: {entity.MemberId}");
        return entity;
    }

    public async Task<Member?> UpdateAsync(Member entity)
    {
        _logger.LogDebug($"Finding member based on id: {entity.MemberId}");
        var member = await _dbContext.Member.FirstOrDefaultAsync(m => m.MemberId == entity.MemberId);
        if (member == null) return null;

        _logger.LogDebug($"Updating member with id: {entity.MemberId} with current values: " +
                         $"from: {member.FirstName} to: {entity.FirstName} " +
                         $"from {member.LastName} to: {entity.LastName} " +
                         $"from: {member.Gender} to: {entity.Gender} " +
                         $"from: {member.BirthYear} to: {entity.BirthYear} " +
                         $"from: {member.Updated} to: {entity.Updated}");
        
        _dbContext.Member.Update(member);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation($"Updated member with id {member.MemberId}");
        return member;
    }

    public async Task<Member?> DeleteByIdAsync(int id)
    {
        _logger.LogDebug($"Finding member based on id: {id}");
        var member = await _dbContext.Member.FindAsync(id);
        if (member == null) return null;

        _logger.LogInformation($"Deleting member based on id: {id}");
        _dbContext.Member.Remove(member);
        await _dbContext.SaveChangesAsync();

        return member;
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Getting member from id: {id}");
        return await _dbContext.Member.FirstOrDefaultAsync(m => m.MemberId == id);
    }

    public async Task<IEnumerable<Member>> FindAsync(Expression<Func<Member, bool>> predicate)
    {
        _logger.LogInformation($"Finding: {predicate} in Member.");
        return await _dbContext.Member
            .Where(predicate)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Member>> GetPagedAsync(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;
        
        _logger.LogInformation($"Getting paged members from page: {pageNumber} with size: {pageSize}");
        var users = await _dbContext.Member
            .OrderBy(m => m.MemberId)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return users;
    }
}