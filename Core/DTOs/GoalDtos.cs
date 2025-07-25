using AgizDisSagligiApp.Core.Enums;

namespace AgizDisSagligiApp.Core.DTOs
{
    public class GoalDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ActivityType ActivityType { get; set; }
        public int TargetCount { get; set; }
        public int CurrentCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GoalStatus Status { get; set; }
        public Priority Priority { get; set; }
        public string? Tags { get; set; }
        public bool IsRecurring { get; set; }
        public string? RecurrencePattern { get; set; }
        public int? RemindBefore { get; set; }
        public string? Reward { get; set; }
        public decimal? RewardPoints { get; set; }
        
        // Calculated properties
        public decimal ProgressPercentage { get; set; }
        public bool IsCompleted { get; set; }
        public int DaysRemaining { get; set; }
        public bool IsOverdue { get; set; }
    }
    
    public class CreateGoalDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ActivityType ActivityType { get; set; }
        public int TargetCount { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        public Priority Priority { get; set; } = Priority.Medium;
        public string? Tags { get; set; }
        public bool IsRecurring { get; set; }
        public string? RecurrencePattern { get; set; }
        public int? RemindBefore { get; set; }
        public string? Reward { get; set; }
        public decimal? RewardPoints { get; set; }
    }
    
    public class UpdateGoalDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TargetCount { get; set; }
        public DateTime EndDate { get; set; }
        public GoalStatus Status { get; set; }
        public Priority Priority { get; set; }
        public string? Tags { get; set; }
        public int? RemindBefore { get; set; }
        public string? Reward { get; set; }
        public decimal? RewardPoints { get; set; }
    }
}