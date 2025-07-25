using AgizDisSagligiApp.Core.Enums;

namespace AgizDisSagligiApp.Core.DTOs
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ActivityType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CompletedAt { get; set; }
        public int Duration { get; set; }
        public string? Notes { get; set; }
        public string? Tags { get; set; }
        public int? Rating { get; set; }
        public bool IsCustom { get; set; }
        public string? ImageUrl { get; set; }
    }
    
    public class CreateActivityDto
    {
        public ActivityType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
        public int Duration { get; set; }
        public string? Notes { get; set; }
        public string? Tags { get; set; }
        public int? Rating { get; set; }
        public bool IsCustom { get; set; }
    }
    
    public class UpdateActivityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CompletedAt { get; set; }
        public int Duration { get; set; }
        public string? Notes { get; set; }
        public string? Tags { get; set; }
        public int? Rating { get; set; }
    }
}