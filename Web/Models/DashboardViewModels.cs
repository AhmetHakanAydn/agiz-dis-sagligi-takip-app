using AgizDisSagligiApp.Core.DTOs;
using AgizDisSagligiApp.Core.Enums;

namespace AgizDisSagligiApp.Web.Models
{
    public class DashboardViewModel
    {
        public int TodayActivities { get; set; }
        public IEnumerable<GoalDto> ActiveGoals { get; set; } = new List<GoalDto>();
        public Dictionary<string, int> WeeklyProgress { get; set; } = new Dictionary<string, int>();
        public decimal CompletionRate { get; set; }
        public int ActivityStreak { get; set; }
        public IEnumerable<GoalDto> TodayGoals { get; set; } = new List<GoalDto>();
        public IEnumerable<ActivityDto> RecentActivities { get; set; } = new List<ActivityDto>();
    }

    public class QuickActivityModel
    {
        public ActivityType Type { get; set; }
        public int Duration { get; set; }
        public string? Notes { get; set; }
    }
}