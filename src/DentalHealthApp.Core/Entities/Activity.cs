using System.ComponentModel.DataAnnotations;

namespace DentalHealthApp.Core.Entities;

public class Activity
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int GoalId { get; set; }
    
    public DateTime ActivityDate { get; set; } = DateTime.Today;
    
    public bool IsCompleted { get; set; }
    
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    [StringLength(500)]
    public string? ImagePath { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Goal Goal { get; set; } = null!;
}