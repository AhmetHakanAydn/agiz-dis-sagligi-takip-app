namespace AgizDisSagligiApp.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IActivityRepository Activities { get; }
        IGoalRepository Goals { get; }
        IAppointmentRepository Appointments { get; }
        INotificationRepository Notifications { get; }
        IUserSettingsRepository UserSettings { get; }
        IReportRepository Reports { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}