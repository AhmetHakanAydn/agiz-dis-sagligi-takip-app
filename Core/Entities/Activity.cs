using AgizDisSagligiApp.Core.Enums;

namespace AgizDisSagligiApp.Core.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ActivityType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Duration { get; set; } // in seconds
        public string? Notes { get; set; }
        public string? Tags { get; set; }
        public int? Rating { get; set; } // 1-5 stars
        public bool IsCustom { get; set; } = false;
        public string? ImageUrl { get; set; }
        
        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        
        public bool IsCompleted => CompletedAt <= DateTime.UtcNow;
    }
}