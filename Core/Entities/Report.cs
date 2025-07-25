using AgizDisSagligiApp.Core.Enums;

namespace AgizDisSagligiApp.Core.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public ReportType Type { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Data { get; set; } = string.Empty; // JSON data
        public string? FilePath { get; set; }
        public string? FileFormat { get; set; } = "PDF";
        public int FileSize { get; set; } = 0;
        public bool IsGenerated { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
        public string? Error { get; set; }
        
        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        
        // Calculated properties
        public bool IsReady => IsGenerated && !string.IsNullOrEmpty(FilePath);
        public string FormattedFileSize => FileSize > 1024 * 1024 
            ? $"{FileSize / (1024 * 1024):F1} MB" 
            : FileSize > 1024 
                ? $"{FileSize / 1024:F1} KB" 
                : $"{FileSize} B";
    }
}