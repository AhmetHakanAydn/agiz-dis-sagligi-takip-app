using System.ComponentModel.DataAnnotations;

namespace DentalHealthApp.Core.Entities;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}