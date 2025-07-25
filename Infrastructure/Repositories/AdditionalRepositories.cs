using Microsoft.EntityFrameworkCore;
using AgizDisSagligiApp.Core.Entities;
using AgizDisSagligiApp.Core.Enums;
using AgizDisSagligiApp.Core.Interfaces;
using AgizDisSagligiApp.Infrastructure.Data;

namespace AgizDisSagligiApp.Infrastructure.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _dbSet
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId)
        {
            return await _dbSet
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetScheduledNotificationsAsync()
        {
            var currentTime = DateTime.UtcNow;
            return await _dbSet
                .Where(n => n.ScheduledFor.HasValue && n.ScheduledFor <= currentTime && !n.IsSent)
                .OrderBy(n => n.ScheduledFor)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _dbSet
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _dbSet.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
            }
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var unreadNotifications = await _dbSet
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(string userId, NotificationType type)
        {
            return await _dbSet
                .Where(n => n.UserId == userId && n.Type == type)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
    }

    public class UserSettingsRepository : Repository<UserSettings>, IUserSettingsRepository
    {
        public UserSettingsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<UserSettings?> GetUserSettingAsync(string userId, string key)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Key == key);
        }

        public async Task<Dictionary<string, string>> GetUserSettingsAsync(string userId)
        {
            var settings = await _dbSet
                .Where(s => s.UserId == userId)
                .ToListAsync();

            return settings.ToDictionary(s => s.Key, s => s.Value);
        }

        public async Task SetUserSettingAsync(string userId, string key, string value)
        {
            var existingSetting = await GetUserSettingAsync(userId, key);
            
            if (existingSetting != null)
            {
                existingSetting.Value = value;
                existingSetting.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var newSetting = new UserSettings
                {
                    UserId = userId,
                    Key = key,
                    Value = value,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _dbSet.AddAsync(newSetting);
            }
        }

        public async Task RemoveUserSettingAsync(string userId, string key)
        {
            var setting = await GetUserSettingAsync(userId, key);
            if (setting != null)
            {
                _dbSet.Remove(setting);
            }
        }
    }

    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Report>> GetUserReportsAsync(string userId)
        {
            return await _dbSet
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.GeneratedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetReportsByTypeAsync(string userId, ReportType type)
        {
            return await _dbSet
                .Where(r => r.UserId == userId && r.Type == type)
                .OrderByDescending(r => r.GeneratedAt)
                .ToListAsync();
        }

        public async Task<Report?> GetLatestReportAsync(string userId, ReportType type)
        {
            return await _dbSet
                .Where(r => r.UserId == userId && r.Type == type)
                .OrderByDescending(r => r.GeneratedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Report>> GetPendingReportsAsync()
        {
            return await _dbSet
                .Where(r => !r.IsGenerated)
                .OrderBy(r => r.GeneratedAt)
                .ToListAsync();
        }
    }
}