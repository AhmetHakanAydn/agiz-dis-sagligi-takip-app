using AgizDisSagligiApp.Core.Enums;

namespace AgizDisSagligiApp.Core.Entities
{
    public class Goal
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ActivityType ActivityType { get; set; }
        public int TargetCount { get; set; }
        public int CurrentCount { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GoalStatus Status { get; set; } = GoalStatus.Active;
        public Priority Priority { get; set; } = Priority.Medium;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public string? Tags { get; set; }
        public bool IsRecurring { get; set; } = false;
        public string? RecurrencePattern { get; set; } // daily, weekly, monthly
        public int? RemindBefore { get; set; } // minutes before
        public string? Reward { get; set; }
        public decimal? RewardPoints { get; set; }
        
        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        
        // Calculated properties
        public decimal ProgressPercentage => TargetCount > 0 ? (decimal)CurrentCount / TargetCount * 100 : 0;
        public bool IsCompleted => Status == GoalStatus.Completed || CurrentCount >= TargetCount;
        public int DaysRemaining => (int)(EndDate - DateTime.UtcNow).TotalDays;
        public bool IsOverdue => DateTime.UtcNow > EndDate && !IsCompleted;
    }
}