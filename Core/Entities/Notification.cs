using AgizDisSagligiApp.Core.Enums;

namespace AgizDisSagligiApp.Core.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }
        public DateTime? ScheduledFor { get; set; }
        public string? ActionUrl { get; set; }
        public string? ImageUrl { get; set; }
        public Priority Priority { get; set; } = Priority.Medium;
        public bool IsSent { get; set; } = false;
        public DateTime? SentAt { get; set; }
        public string? Data { get; set; } // JSON data for additional info
        
        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        
        // Calculated properties
        public bool IsScheduled => ScheduledFor.HasValue && ScheduledFor > DateTime.UtcNow;
        public bool ShouldBeSent => ScheduledFor.HasValue && ScheduledFor <= DateTime.UtcNow && !IsSent;
    }
}