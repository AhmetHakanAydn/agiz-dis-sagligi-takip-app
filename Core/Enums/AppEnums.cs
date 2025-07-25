namespace AgizDisSagligiApp.Core.Enums
{
    public enum ActivityType
    {
        Brushing = 1,
        Flossing = 2,
        Mouthwash = 3,
        DentistVisit = 4,
        CustomActivity = 5
    }
    
    public enum GoalStatus
    {
        Active = 1,
        Completed = 2,
        Paused = 3,
        Cancelled = 4
    }
    
    public enum AppointmentStatus
    {
        Scheduled = 1,
        Confirmed = 2,
        InProgress = 3,
        Completed = 4,
        Cancelled = 5,
        NoShow = 6
    }
    
    public enum NotificationType
    {
        Reminder = 1,
        Achievement = 2,
        Warning = 3,
        Info = 4,
        System = 5
    }
    
    public enum Priority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Urgent = 4
    }
    
    public enum ThemeType
    {
        Light = 1,
        Dark = 2,
        Blue = 3,
        Green = 4,
        Purple = 5
    }
    
    public enum ReportType
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Yearly = 4,
        Custom = 5
    }
}