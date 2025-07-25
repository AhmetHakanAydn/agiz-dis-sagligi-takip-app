using Microsoft.EntityFrameworkCore.Storage;
using AgizDisSagligiApp.Core.Interfaces;
using AgizDisSagligiApp.Infrastructure.Data;
using AgizDisSagligiApp.Infrastructure.Repositories;

namespace AgizDisSagligiApp.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Activities = new ActivityRepository(_context);
            Goals = new GoalRepository(_context);
            Appointments = new AppointmentRepository(_context);
            Notifications = new NotificationRepository(_context);
            UserSettings = new UserSettingsRepository(_context);
            Reports = new ReportRepository(_context);
        }

        public IActivityRepository Activities { get; }
        public IGoalRepository Goals { get; }
        public IAppointmentRepository Appointments { get; }
        public INotificationRepository Notifications { get; }
        public IUserSettingsRepository UserSettings { get; }
        public IReportRepository Reports { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}