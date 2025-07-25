using AgizDisSagligiApp.Core.Entities;
using AgizDisSagligiApp.Core.Enums;

namespace AgizDisSagligiApp.Core.Interfaces
{
    public interface IActivityRepository : IRepository<Activity>
    {
        Task<IEnumerable<Activity>> GetUserActivitiesAsync(string userId);
        Task<IEnumerable<Activity>> GetUserActivitiesByDateRangeAsync(string userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Activity>> GetUserActivitiesByTypeAsync(string userId, ActivityType type);
        Task<Activity?> GetLatestActivityAsync(string userId, ActivityType type);
        Task<int> GetUserActivityCountAsync(string userId, DateTime date);
        Task<int> GetUserActivityStreakAsync(string userId, ActivityType type);
        Task<Dictionary<DateTime, int>> GetUserActivityStatisticsAsync(string userId, DateTime startDate, DateTime endDate);
    }
    
    public interface IGoalRepository : IRepository<Goal>
    {
        Task<IEnumerable<Goal>> GetUserGoalsAsync(string userId);
        Task<IEnumerable<Goal>> GetActiveGoalsAsync(string userId);
        Task<IEnumerable<Goal>> GetCompletedGoalsAsync(string userId);
        Task<IEnumerable<Goal>> GetOverdueGoalsAsync(string userId);
        Task<IEnumerable<Goal>> GetGoalsByTypeAsync(string userId, ActivityType type);
        Task<int> GetCompletedGoalsCountAsync(string userId);
        Task<decimal> GetGoalCompletionRateAsync(string userId);
    }
    
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId);
        Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(string userId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateRangeAsync(string userId, DateTime startDate, DateTime endDate);
        Task<Appointment?> GetNextAppointmentAsync(string userId);
        Task<IEnumerable<Appointment>> GetTodayAppointmentsAsync(string userId);
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(string userId, AppointmentStatus status);
    }
    
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId);
        Task<IEnumerable<Notification>> GetScheduledNotificationsAsync();
        Task<int> GetUnreadCountAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(string userId);
        Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(string userId, NotificationType type);
    }
    
    public interface IUserSettingsRepository : IRepository<UserSettings>
    {
        Task<UserSettings?> GetUserSettingAsync(string userId, string key);
        Task<Dictionary<string, string>> GetUserSettingsAsync(string userId);
        Task SetUserSettingAsync(string userId, string key, string value);
        Task RemoveUserSettingAsync(string userId, string key);
    }
    
    public interface IReportRepository : IRepository<Report>
    {
        Task<IEnumerable<Report>> GetUserReportsAsync(string userId);
        Task<IEnumerable<Report>> GetReportsByTypeAsync(string userId, ReportType type);
        Task<Report?> GetLatestReportAsync(string userId, ReportType type);
        Task<IEnumerable<Report>> GetPendingReportsAsync();
    }
}