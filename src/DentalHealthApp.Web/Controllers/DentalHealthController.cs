using DentalHealthApp.Core.DTOs;
using DentalHealthApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DentalHealthApp.Web.Controllers;

public class DentalHealthController : Controller
{
    private readonly IGoalService _goalService;
    private readonly IActivityService _activityService;
    private readonly INoteService _noteService;
    private readonly IRecommendationService _recommendationService;

    public DentalHealthController(
        IGoalService goalService,
        IActivityService activityService,
        INoteService noteService,
        IRecommendationService recommendationService)
    {
        _goalService = goalService;
        _activityService = activityService;
        _noteService = noteService;
        _recommendationService = recommendationService;
    }

    private int? GetUserId()
    {
        return HttpContext.Session.GetInt32("UserId");
    }

    private IActionResult RedirectToLogin()
    {
        return RedirectToAction("Login", "Account");
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToLogin();
        }

        // Get data for Status tab (last 7 days)
        var endDate = DateTime.Today;
        var startDate = endDate.AddDays(-6);
        
        var activities = await _activityService.GetUserActivitiesForPeriodAsync(userId.Value, startDate, endDate);
        var notes = await _noteService.GetUserNotesForPeriodAsync(userId.Value, startDate, endDate);
        var goals = await _goalService.GetUserGoalsAsync(userId.Value);
        var recommendations = await _recommendationService.GetRandomRecommendationsAsync(3);

        ViewBag.Activities = activities;
        ViewBag.Notes = notes;
        ViewBag.Goals = goals;
        ViewBag.Recommendations = recommendations;

        return View();
    }

    [HttpGet]
    public IActionResult CreateGoal()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToLogin();
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateGoal(GoalCreateDto model)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToLogin();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var goal = await _goalService.CreateGoalAsync(userId.Value, model);
        if (goal != null)
        {
            TempData["SuccessMessage"] = "Hedef başarıyla oluşturuldu.";
            return RedirectToAction("Index", new { tab = "goals" });
        }

        ModelState.AddModelError("", "Hedef oluşturma işlemi sırasında bir hata oluştu.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteGoal(int id)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToLogin();
        }

        // Check if goal has activities
        if (await _goalService.HasActivitiesAsync(id))
        {
            TempData["ErrorMessage"] = "Bu hedefle ilgili aktiviteler bulunduğu için silinemez.";
            return RedirectToAction("Index", new { tab = "goals" });
        }

        var success = await _goalService.DeleteGoalAsync(id, userId.Value);
        if (success)
        {
            TempData["SuccessMessage"] = "Hedef başarıyla silindi.";
        }
        else
        {
            TempData["ErrorMessage"] = "Hedef silme işlemi sırasında bir hata oluştu.";
        }

        return RedirectToAction("Index", new { tab = "goals" });
    }

    [HttpGet]
    public async Task<IActionResult> CreateActivity()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToLogin();
        }

        var goals = await _goalService.GetUserGoalsAsync(userId.Value);
        ViewBag.Goals = goals;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateActivity(ActivityCreateDto model)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToLogin();
        }

        if (!ModelState.IsValid)
        {
            var goals = await _goalService.GetUserGoalsAsync(userId.Value);
            ViewBag.Goals = goals;
            return View(model);
        }

        var activity = await _activityService.CreateActivityAsync(userId.Value, model);
        if (activity != null)
        {
            TempData["SuccessMessage"] = "Aktivite başarıyla kaydedildi.";
            return RedirectToAction("Index", new { tab = "status" });
        }

        ModelState.AddModelError("", "Aktivite kaydetme işlemi sırasında bir hata oluştu.");
        var goalsForView = await _goalService.GetUserGoalsAsync(userId.Value);
        ViewBag.Goals = goalsForView;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToLogin();
        }

        var success = await _activityService.DeleteActivityAsync(id, userId.Value);
        if (success)
        {
            TempData["SuccessMessage"] = "Aktivite başarıyla silindi.";
        }
        else
        {
            TempData["ErrorMessage"] = "Aktivite silme işlemi sırasında bir hata oluştu.";
        }

        return RedirectToAction("Index", new { tab = "status" });
    }

    [HttpGet]
    public IActionResult CreateNote()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToLogin();
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateNote(NoteCreateDto model)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToLogin();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var note = await _noteService.CreateNoteAsync(userId.Value, model);
        if (note != null)
        {
            TempData["SuccessMessage"] = "Not başarıyla kaydedildi.";
            return RedirectToAction("Index", new { tab = "status" });
        }

        ModelState.AddModelError("", "Not kaydetme işlemi sırasında bir hata oluştu.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteNote(int id)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToLogin();
        }

        var success = await _noteService.DeleteNoteAsync(id, userId.Value);
        if (success)
        {
            TempData["SuccessMessage"] = "Not başarıyla silindi.";
        }
        else
        {
            TempData["ErrorMessage"] = "Not silme işlemi sırasında bir hata oluştu.";
        }

        return RedirectToAction("Index", new { tab = "status" });
    }
}