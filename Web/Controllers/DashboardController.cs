using Microsoft.AspNetCore.Mvc;
using AgizDisSagligiApp.Application.Services;
using AgizDisSagligiApp.Core.Enums;
using AgizDisSagligiApp.Web.Models;

namespace AgizDisSagligiApp.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly IGoalService _goalService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IActivityService activityService,
            IGoalService goalService,
            ILogger<DashboardController> logger)
        {
            _activityService = activityService;
            _goalService = goalService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // For demo purposes, using a sample user ID
                // In a real application, this would come from authentication
                var userId = GetCurrentUserId();

                var viewModel = new DashboardViewModel
                {
                    TodayActivities = await GetTodayActivitiesCountAsync(userId),
                    ActiveGoals = await _goalService.GetActiveGoalsAsync(userId),
                    WeeklyProgress = await GetWeeklyProgressAsync(userId),
                    CompletionRate = await _goalService.GetGoalCompletionRateAsync(userId),
                    ActivityStreak = await GetActivityStreakAsync(userId),
                    TodayGoals = await GetTodayGoalsAsync(userId),
                    RecentActivities = await GetRecentActivitiesAsync(userId)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dashboard yüklenirken hata oluştu");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetWeeklyChart()
        {
            try
            {
                var userId = GetCurrentUserId();
                var startDate = DateTime.UtcNow.AddDays(-7);
                var endDate = DateTime.UtcNow;

                var statistics = await _activityService.GetUserActivityStatisticsAsync(userId, startDate, endDate);

                var chartData = new
                {
                    labels = GetLast7Days(),
                    datasets = new[]
                    {
                        new
                        {
                            label = "Günlük Aktiviteler",
                            data = GetLast7Days().Select(date => 
                                statistics.ContainsKey(date) ? statistics[date] : 0
                            ).ToArray(),
                            backgroundColor = "#007bff",
                            borderColor = "#0056b3",
                            borderWidth = 2,
                            fill = true
                        }
                    }
                };

                return Json(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haftalık grafik verileri alınırken hata oluştu");
                return Json(new { error = "Grafik verileri alınamadı" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> QuickActivity([FromBody] QuickActivityModel model)
        {
            try
            {
                var userId = GetCurrentUserId();

                var createActivityDto = new AgizDisSagligiApp.Core.DTOs.CreateActivityDto
                {
                    Type = model.Type,
                    Name = GetActivityName(model.Type),
                    Duration = model.Duration,
                    CompletedAt = DateTime.UtcNow,
                    Notes = model.Notes
                };

                var activity = await _activityService.CreateActivityAsync(userId, createActivityDto);

                return Json(new { success = true, message = "Aktivite başarıyla kaydedildi!", activity });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hızlı aktivite kaydedilirken hata oluştu");
                return Json(new { success = false, message = "Aktivite kaydedilemedi" });
            }
        }

        private string GetCurrentUserId()
        {
            // Demo user ID - in real app, get from authentication
            return "demo-user-001";
        }

        private async Task<int> GetTodayActivitiesCountAsync(string userId)
        {
            var today = DateTime.UtcNow.Date;
            var endDate = today.AddDays(1);
            var activities = await _activityService.GetUserActivitiesByDateRangeAsync(userId, today, endDate);
            return activities.Count();
        }

        private async Task<Dictionary<string, int>> GetWeeklyProgressAsync(string userId)
        {
            var startDate = DateTime.UtcNow.AddDays(-7);
            var endDate = DateTime.UtcNow;
            var statistics = await _activityService.GetUserActivityStatisticsAsync(userId, startDate, endDate);
            
            return statistics.ToDictionary(
                kvp => kvp.Key.ToString("dd/MM"),
                kvp => kvp.Value
            );
        }

        private async Task<int> GetActivityStreakAsync(string userId)
        {
            return await _activityService.GetUserActivityStreakAsync(userId, ActivityType.Brushing);
        }

        private async Task<IEnumerable<Core.DTOs.GoalDto>> GetTodayGoalsAsync(string userId)
        {
            var goals = await _goalService.GetActiveGoalsAsync(userId);
            return goals.Where(g => g.EndDate.Date >= DateTime.UtcNow.Date).Take(3);
        }

        private async Task<IEnumerable<Core.DTOs.ActivityDto>> GetRecentActivitiesAsync(string userId)
        {
            var activities = await _activityService.GetUserActivitiesAsync(userId);
            return activities.Take(5);
        }

        private List<DateTime> GetLast7Days()
        {
            var days = new List<DateTime>();
            for (int i = 6; i >= 0; i--)
            {
                days.Add(DateTime.UtcNow.AddDays(-i).Date);
            }
            return days;
        }

        private string GetActivityName(ActivityType type)
        {
            return type switch
            {
                ActivityType.Brushing => "Diş Fırçalama",
                ActivityType.Flossing => "Diş İpi Kullanımı",
                ActivityType.Mouthwash => "Ağız Gargarası",
                ActivityType.DentistVisit => "Diş Hekimi Ziyareti",
                _ => "Genel Aktivite"
            };
        }
    }
}