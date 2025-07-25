using AgizDisSagligiApp.Core.Enums;

namespace AgizDisSagligiApp.Core.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int Duration { get; set; }
        public string? DoctorName { get; set; }
        public string? ClinicName { get; set; }
        public string? ClinicAddress { get; set; }
        public string? ClinicPhone { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
        public string? TreatmentType { get; set; }
        public decimal? Cost { get; set; }
        public string? InsuranceInfo { get; set; }
        public string? Prescription { get; set; }
        public string? Diagnosis { get; set; }
        public string? NextAppointmentRecommendation { get; set; }
        public bool RemindByEmail { get; set; }
        public bool RemindBySms { get; set; }
        public int RemindBefore { get; set; }
        
        // Calculated properties
        public DateTime EndTime { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsToday { get; set; }
        public int DaysUntilAppointment { get; set; }
    }
    
    public class CreateAppointmentDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int Duration { get; set; } = 60;
        public string? DoctorName { get; set; }
        public string? ClinicName { get; set; }
        public string? ClinicAddress { get; set; }
        public string? ClinicPhone { get; set; }
        public string? Notes { get; set; }
        public string? TreatmentType { get; set; }
        public decimal? Cost { get; set; }
        public string? InsuranceInfo { get; set; }
        public bool RemindByEmail { get; set; } = true;
        public bool RemindBySms { get; set; } = false;
        public int RemindBefore { get; set; } = 1440;
    }
    
    public class UpdateAppointmentDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int Duration { get; set; }
        public string? DoctorName { get; set; }
        public string? ClinicName { get; set; }
        public string? ClinicAddress { get; set; }
        public string? ClinicPhone { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
        public string? TreatmentType { get; set; }
        public decimal? Cost { get; set; }
        public string? InsuranceInfo { get; set; }
        public string? Prescription { get; set; }
        public string? Diagnosis { get; set; }
        public string? NextAppointmentRecommendation { get; set; }
        public bool RemindByEmail { get; set; }
        public bool RemindBySms { get; set; }
        public int RemindBefore { get; set; }
    }
}