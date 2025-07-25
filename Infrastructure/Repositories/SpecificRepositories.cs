using Microsoft.EntityFrameworkCore;
using AgizDisSagligiApp.Core.Entities;
using AgizDisSagligiApp.Core.Enums;
using AgizDisSagligiApp.Core.Interfaces;
using AgizDisSagligiApp.Infrastructure.Data;

namespace AgizDisSagligiApp.Infrastructure.Repositories
{
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        public ActivityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Activity>> GetUserActivitiesAsync(string userId)
        {
            return await _dbSet
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CompletedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetUserActivitiesByDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(a => a.UserId == userId && a.CompletedAt >= startDate && a.CompletedAt <= endDate)
                .OrderByDescending(a => a.CompletedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetUserActivitiesByTypeAsync(string userId, ActivityType type)
        {
            return await _dbSet
                .Where(a => a.UserId == userId && a.Type == type)
                .OrderByDescending(a => a.CompletedAt)
                .ToListAsync();
        }

        public async Task<Activity?> GetLatestActivityAsync(string userId, ActivityType type)
        {
            return await _dbSet
                .Where(a => a.UserId == userId && a.Type == type)
                .OrderByDescending(a => a.CompletedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetUserActivityCountAsync(string userId, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);
            
            return await _dbSet
                .CountAsync(a => a.UserId == userId && a.CompletedAt >= startOfDay && a.CompletedAt < endOfDay);
        }

        public async Task<int> GetUserActivityStreakAsync(string userId, ActivityType type)
        {
            var activities = await _dbSet
                .Where(a => a.UserId == userId && a.Type == type)
                .OrderByDescending(a => a.CompletedAt.Date)
                .Select(a => a.CompletedAt.Date)
                .Distinct()
                .ToListAsync();

            if (!activities.Any())
                return 0;

            var streak = 0;
            var currentDate = DateTime.UtcNow.Date;
            
            foreach (var activityDate in activities)
            {
                if (activityDate == currentDate || activityDate == currentDate.AddDays(-streak))
                {
                    streak++;
                    currentDate = activityDate.AddDays(-1);
                }
                else
                {
                    break;
                }
            }

            return streak;
        }

        public async Task<Dictionary<DateTime, int>> GetUserActivityStatisticsAsync(string userId, DateTime startDate, DateTime endDate)
        {
            var activities = await _dbSet
                .Where(a => a.UserId == userId && a.CompletedAt >= startDate && a.CompletedAt <= endDate)
                .GroupBy(a => a.CompletedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            return activities.ToDictionary(a => a.Date, a => a.Count);
        }
    }

    public class GoalRepository : Repository<Goal>, IGoalRepository
    {
        public GoalRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Goal>> GetUserGoalsAsync(string userId)
        {
            return await _dbSet
                .Where(g => g.UserId == userId)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Goal>> GetActiveGoalsAsync(string userId)
        {
            return await _dbSet
                .Where(g => g.UserId == userId && g.Status == GoalStatus.Active)
                .OrderBy(g => g.EndDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Goal>> GetCompletedGoalsAsync(string userId)
        {
            return await _dbSet
                .Where(g => g.UserId == userId && g.Status == GoalStatus.Completed)
                .OrderByDescending(g => g.CompletedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Goal>> GetOverdueGoalsAsync(string userId)
        {
            var currentDate = DateTime.UtcNow;
            return await _dbSet
                .Where(g => g.UserId == userId && g.Status == GoalStatus.Active && g.EndDate < currentDate)
                .OrderBy(g => g.EndDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Goal>> GetGoalsByTypeAsync(string userId, ActivityType type)
        {
            return await _dbSet
                .Where(g => g.UserId == userId && g.ActivityType == type)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetCompletedGoalsCountAsync(string userId)
        {
            return await _dbSet
                .CountAsync(g => g.UserId == userId && g.Status == GoalStatus.Completed);
        }

        public async Task<decimal> GetGoalCompletionRateAsync(string userId)
        {
            var totalGoals = await _dbSet.CountAsync(g => g.UserId == userId);
            if (totalGoals == 0) return 0;

            var completedGoals = await GetCompletedGoalsCountAsync(userId);
            return (decimal)completedGoals / totalGoals * 100;
        }
    }

    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId)
        {
            return await _dbSet
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(string userId)
        {
            var currentDate = DateTime.UtcNow;
            return await _dbSet
                .Where(a => a.UserId == userId && a.ScheduledDate > currentDate && a.Status == AppointmentStatus.Scheduled)
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(a => a.UserId == userId && a.ScheduledDate >= startDate && a.ScheduledDate <= endDate)
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync();
        }

        public async Task<Appointment?> GetNextAppointmentAsync(string userId)
        {
            var currentDate = DateTime.UtcNow;
            return await _dbSet
                .Where(a => a.UserId == userId && a.ScheduledDate > currentDate && a.Status == AppointmentStatus.Scheduled)
                .OrderBy(a => a.ScheduledDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Appointment>> GetTodayAppointmentsAsync(string userId)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);
            
            return await _dbSet
                .Where(a => a.UserId == userId && a.ScheduledDate >= today && a.ScheduledDate < tomorrow)
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(string userId, AppointmentStatus status)
        {
            return await _dbSet
                .Where(a => a.UserId == userId && a.Status == status)
                .OrderByDescending(a => a.ScheduledDate)
                .ToListAsync();
        }
    }
}