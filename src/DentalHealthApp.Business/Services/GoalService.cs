using DentalHealthApp.Core.DTOs;
using DentalHealthApp.Core.Entities;
using DentalHealthApp.Core.Interfaces;
using DentalHealthApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthApp.Business.Services;

public class GoalService : IGoalService
{
    private readonly DentalHealthDbContext _context;

    public GoalService(DentalHealthDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GoalDto>> GetUserGoalsAsync(int userId)
    {
        var goals = await _context.Goals
            .Where(g => g.UserId == userId)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();

        return goals.Select(g => new GoalDto
        {
            Id = g.Id,
            Title = g.Title,
            Description = g.Description,
            Period = g.Period,
            ImportanceLevel = g.ImportanceLevel,
            CreatedAt = g.CreatedAt,
            HasActivities = _context.Activities.Any(a => a.GoalId == g.Id)
        });
    }

    public async Task<GoalDto?> GetGoalByIdAsync(int goalId, int userId)
    {
        var goal = await _context.Goals
            .Where(g => g.Id == goalId && g.UserId == userId)
            .FirstOrDefaultAsync();

        if (goal == null)
            return null;

        return new GoalDto
        {
            Id = goal.Id,
            Title = goal.Title,
            Description = goal.Description,
            Period = goal.Period,
            ImportanceLevel = goal.ImportanceLevel,
            CreatedAt = goal.CreatedAt,
            HasActivities = await _context.Activities.AnyAsync(a => a.GoalId == goal.Id)
        };
    }

    public async Task<GoalDto?> CreateGoalAsync(int userId, GoalCreateDto goalDto)
    {
        var goal = new Goal
        {
            UserId = userId,
            Title = goalDto.Title,
            Description = goalDto.Description,
            Period = goalDto.Period,
            ImportanceLevel = goalDto.ImportanceLevel,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Goals.Add(goal);
        await _context.SaveChangesAsync();

        return new GoalDto
        {
            Id = goal.Id,
            Title = goal.Title,
            Description = goal.Description,
            Period = goal.Period,
            ImportanceLevel = goal.ImportanceLevel,
            CreatedAt = goal.CreatedAt,
            HasActivities = false
        };
    }

    public async Task<bool> DeleteGoalAsync(int goalId, int userId)
    {
        var goal = await _context.Goals
            .Where(g => g.Id == goalId && g.UserId == userId)
            .FirstOrDefaultAsync();

        if (goal == null)
            return false;

        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasActivitiesAsync(int goalId)
    {
        return await _context.Activities.AnyAsync(a => a.GoalId == goalId);
    }
}