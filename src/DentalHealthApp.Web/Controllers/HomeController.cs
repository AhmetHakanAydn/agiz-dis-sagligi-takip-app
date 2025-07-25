using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DentalHealthApp.Web.Models;
using DentalHealthApp.Core.Interfaces;

namespace DentalHealthApp.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IActivityService _activityService;
    private readonly IRecommendationService _recommendationService;

    public HomeController(ILogger<HomeController> logger, IActivityService activityService, IRecommendationService recommendationService)
    {
        _logger = logger;
        _activityService = activityService;
        _recommendationService = recommendationService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Get last 7 days statistics
        var stats = await _activityService.GetLast7DaysStatsAsync(userId.Value);
        
        // Get random recommendations
        var recommendations = await _recommendationService.GetRandomRecommendationsAsync(3);

        ViewBag.Stats = stats;
        ViewBag.Recommendations = recommendations;
        ViewBag.UserName = HttpContext.Session.GetString("UserName");

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
