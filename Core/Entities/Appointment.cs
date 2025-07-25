using AgizDisSagligiApp.Core.Enums;

namespace AgizDisSagligiApp.Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int Duration { get; set; } = 60; // in minutes
        public string? DoctorName { get; set; }
        public string? ClinicName { get; set; }
        public string? ClinicAddress { get; set; }
        public string? ClinicPhone { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public string? Notes { get; set; }
        public string? TreatmentType { get; set; }
        public decimal? Cost { get; set; }
        public string? InsuranceInfo { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public string? Prescription { get; set; }
        public string? Diagnosis { get; set; }
        public string? NextAppointmentRecommendation { get; set; }
        public bool RemindByEmail { get; set; } = true;
        public bool RemindBySms { get; set; } = false;
        public int RemindBefore { get; set; } = 1440; // 24 hours in minutes
        
        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        
        // Calculated properties
        public DateTime EndTime => ScheduledDate.AddMinutes(Duration);
        public bool IsUpcoming => ScheduledDate > DateTime.UtcNow && Status == AppointmentStatus.Scheduled;
        public bool IsToday => ScheduledDate.Date == DateTime.UtcNow.Date;
        public int DaysUntilAppointment => (int)(ScheduledDate.Date - DateTime.UtcNow.Date).TotalDays;
    }
}