using System.ComponentModel.DataAnnotations;
using DentalHealthApp.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace DentalHealthApp.Core.DTOs;

public class GoalCreateDto
{
    [Required(ErrorMessage = "Hedef başlığı gereklidir.")]
    [StringLength(255, ErrorMessage = "Hedef başlığı en fazla 255 karakter olabilir.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Periyot seçimi gereklidir.")]
    public GoalPeriod Period { get; set; }

    [Required(ErrorMessage = "Önem derecesi seçimi gereklidir.")]
    public ImportanceLevel ImportanceLevel { get; set; }
}

public class GoalDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public GoalPeriod Period { get; set; }
    public ImportanceLevel ImportanceLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool HasActivities { get; set; }
}

public class ActivityCreateDto
{
    [Required(ErrorMessage = "Hedef seçimi gereklidir.")]
    public int GoalId { get; set; }

    [Required(ErrorMessage = "Tarih seçimi gereklidir.")]
    [DataType(DataType.Date)]
    public DateTime ActivityDate { get; set; } = DateTime.Today;

    public bool IsCompleted { get; set; }

    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir.")]
    public string? Notes { get; set; }

    public IFormFile? Image { get; set; }
}

public class ActivityDto
{
    public int Id { get; set; }
    public int GoalId { get; set; }
    public string GoalTitle { get; set; } = string.Empty;
    public DateTime ActivityDate { get; set; }
    public bool IsCompleted { get; set; }
    public string? Notes { get; set; }
    public string? ImagePath { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class NoteCreateDto
{
    [Required(ErrorMessage = "Not başlığı gereklidir.")]
    [StringLength(255, ErrorMessage = "Not başlığı en fazla 255 karakter olabilir.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Not içeriği gereklidir.")]
    [StringLength(2000, ErrorMessage = "Not içeriği en fazla 2000 karakter olabilir.")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tarih seçimi gereklidir.")]
    [DataType(DataType.Date)]
    public DateTime NoteDate { get; set; } = DateTime.Today;
}

public class NoteDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime NoteDate { get; set; }
    public DateTime CreatedAt { get; set; }
}