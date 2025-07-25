using Microsoft.EntityFrameworkCore;
using AgizDisSagligiApp.Core.Entities;

namespace AgizDisSagligiApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.UserName).IsUnique();
            });

            // Configure Activity
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Notes).HasMaxLength(2000);
                entity.Property(e => e.Tags).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Activities)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.UserId, e.CompletedAt });
            });

            // Configure Goal
            modelBuilder.Entity<Goal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Tags).HasMaxLength(500);
                entity.Property(e => e.RecurrencePattern).HasMaxLength(100);
                entity.Property(e => e.Reward).HasMaxLength(500);
                entity.Property(e => e.RewardPoints).HasPrecision(18, 2);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Goals)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.UserId, e.Status });
            });

            // Configure Appointment
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.DoctorName).HasMaxLength(200);
                entity.Property(e => e.ClinicName).HasMaxLength(200);
                entity.Property(e => e.ClinicAddress).HasMaxLength(500);
                entity.Property(e => e.ClinicPhone).HasMaxLength(50);
                entity.Property(e => e.Notes).HasMaxLength(2000);
                entity.Property(e => e.TreatmentType).HasMaxLength(200);
                entity.Property(e => e.Cost).HasPrecision(18, 2);
                entity.Property(e => e.InsuranceInfo).HasMaxLength(500);
                entity.Property(e => e.Prescription).HasMaxLength(2000);
                entity.Property(e => e.Diagnosis).HasMaxLength(2000);
                entity.Property(e => e.NextAppointmentRecommendation).HasMaxLength(1000);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Appointments)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.UserId, e.ScheduledDate });
            });

            // Configure Notification
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.ActionUrl).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.Data).HasMaxLength(4000);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.UserId, e.IsRead });
                entity.HasIndex(e => e.ScheduledFor);
            });

            // Configure UserSettings
            modelBuilder.Entity<UserSettings>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Key).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Value).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.UserSettings)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.UserId, e.Key }).IsUnique();
            });

            // Configure Report
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Data).IsRequired();
                entity.Property(e => e.FilePath).HasMaxLength(500);
                entity.Property(e => e.FileFormat).HasMaxLength(10);
                entity.Property(e => e.Error).HasMaxLength(2000);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Reports)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.UserId, e.Type, e.GeneratedAt });
            });
        }
    }
}