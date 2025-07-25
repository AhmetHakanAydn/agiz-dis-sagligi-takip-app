using DentalHealthApp.Core.DTOs;
using DentalHealthApp.Core.Entities;
using DentalHealthApp.Core.Interfaces;
using DentalHealthApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthApp.Business.Services;

public class ActivityService : IActivityService
{
    private readonly DentalHealthDbContext _context;

    public ActivityService(DentalHealthDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ActivityDto>> GetUserActivitiesAsync(int userId)
    {
        var activities = await _context.Activities
            .Include(a => a.Goal)
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.ActivityDate)
            .ThenByDescending(a => a.CreatedAt)
            .ToListAsync();

        return activities.Select(a => new ActivityDto
        {
            Id = a.Id,
            GoalId = a.GoalId,
            GoalTitle = a.Goal.Title,
            ActivityDate = a.ActivityDate,
            IsCompleted = a.IsCompleted,
            Notes = a.Notes,
            ImagePath = a.ImagePath,
            CreatedAt = a.CreatedAt
        });
    }

    public async Task<IEnumerable<ActivityDto>> GetUserActivitiesForPeriodAsync(int userId, DateTime startDate, DateTime endDate)
    {
        var activities = await _context.Activities
            .Include(a => a.Goal)
            .Where(a => a.UserId == userId && a.ActivityDate >= startDate && a.ActivityDate <= endDate)
            .OrderByDescending(a => a.ActivityDate)
            .ThenByDescending(a => a.CreatedAt)
            .ToListAsync();

        return activities.Select(a => new ActivityDto
        {
            Id = a.Id,
            GoalId = a.GoalId,
            GoalTitle = a.Goal.Title,
            ActivityDate = a.ActivityDate,
            IsCompleted = a.IsCompleted,
            Notes = a.Notes,
            ImagePath = a.ImagePath,
            CreatedAt = a.CreatedAt
        });
    }

    public async Task<ActivityDto?> CreateActivityAsync(int userId, ActivityCreateDto activityDto)
    {
        // Verify that the goal belongs to the user
        var goal = await _context.Goals
            .FirstOrDefaultAsync(g => g.Id == activityDto.GoalId && g.UserId == userId);

        if (goal == null)
            return null;

        var activity = new Activity
        {
            UserId = userId,
            GoalId = activityDto.GoalId,
            ActivityDate = activityDto.ActivityDate,
            IsCompleted = activityDto.IsCompleted,
            Notes = activityDto.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Handle image upload if provided
        if (activityDto.Image != null && activityDto.Image.Length > 0)
        {
            var uploadsPath = Path.Combine("wwwroot", "uploads", "activities");
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(activityDto.Image.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await activityDto.Image.CopyToAsync(stream);
            }

            activity.ImagePath = $"/uploads/activities/{fileName}";
        }

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        return new ActivityDto
        {
            Id = activity.Id,
            GoalId = activity.GoalId,
            GoalTitle = goal.Title,
            ActivityDate = activity.ActivityDate,
            IsCompleted = activity.IsCompleted,
            Notes = activity.Notes,
            ImagePath = activity.ImagePath,
            CreatedAt = activity.CreatedAt
        };
    }

    public async Task<bool> DeleteActivityAsync(int activityId, int userId)
    {
        var activity = await _context.Activities
            .FirstOrDefaultAsync(a => a.Id == activityId && a.UserId == userId);

        if (activity == null)
            return false;

        // Delete associated image file if exists
        if (!string.IsNullOrEmpty(activity.ImagePath))
        {
            var filePath = Path.Combine("wwwroot", activity.ImagePath.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IDictionary<string, int>> GetLast7DaysStatsAsync(int userId)
    {
        var endDate = DateTime.Today;
        var startDate = endDate.AddDays(-6);

        var activities = await _context.Activities
            .Where(a => a.UserId == userId && a.ActivityDate >= startDate && a.ActivityDate <= endDate)
            .GroupBy(a => a.ActivityDate)
            .Select(g => new
            {
                Date = g.Key,
                CompletedCount = g.Count(a => a.IsCompleted),
                TotalCount = g.Count()
            })
            .ToListAsync();

        var stats = new Dictionary<string, int>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var dayStats = activities.FirstOrDefault(a => a.Date == date);
            var dateKey = date.ToString("dd/MM");
            stats[dateKey] = dayStats?.CompletedCount ?? 0;
        }

        return stats;
    }
}