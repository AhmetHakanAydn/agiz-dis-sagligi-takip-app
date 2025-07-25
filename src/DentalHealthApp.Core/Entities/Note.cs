using System.ComponentModel.DataAnnotations;

namespace DentalHealthApp.Core.Entities;

public class Note
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(2000)]
    public string Content { get; set; } = string.Empty;
    
    public DateTime NoteDate { get; set; } = DateTime.Today;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
}