using System.ComponentModel.DataAnnotations;

namespace DentalHealthApp.Core.Entities;

public enum GoalPeriod
{
    Daily = 1,
    Weekly = 2,
    Monthly = 3
}

public enum ImportanceLevel
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public class Goal
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public GoalPeriod Period { get; set; }
    
    [Required]
    public ImportanceLevel ImportanceLevel { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
}