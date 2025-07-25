namespace AgizDisSagligiApp.Core.Constants
{
    public static class AppConstants
    {
        public const string ApplicationName = "Ağız ve Diş Sağlığı Takip Uygulaması";
        public const string Version = "1.0.0";
        
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string User = "User";
            public const string Doctor = "Doctor";
        }
        
        public static class ActivityTypes
        {
            public const string Brushing = "Fırçalama";
            public const string Flossing = "Diş İpi";
            public const string Mouthwash = "Ağız Gargarası";
            public const string DentistVisit = "Diş Hekimi Kontrolü";
        }
        
        public static class GoalCategories
        {
            public const string DailyBrushing = "Günlük Fırçalama";
            public const string DailyFlossing = "Günlük Diş İpi";
            public const string WeeklyMouthwash = "Haftalık Ağız Gargarası";
            public const string MonthlyDentistVisit = "Aylık Diş Hekimi Kontrolü";
        }
        
        public static class NotificationTypes
        {
            public const string Reminder = "Hatırlatma";
            public const string Achievement = "Başarı";
            public const string Warning = "Uyarı";
            public const string Info = "Bilgi";
        }
    }
}