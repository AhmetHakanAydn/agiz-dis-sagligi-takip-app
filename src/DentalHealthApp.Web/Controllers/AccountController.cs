using DentalHealthApp.Core.DTOs;
using DentalHealthApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DentalHealthApp.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Check if email already exists
        if (await _userService.EmailExistsAsync(model.Email))
        {
            ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılmaktadır.");
            return View(model);
        }

        var user = await _userService.RegisterAsync(model);
        if (user != null)
        {
            TempData["SuccessMessage"] = "Kayıt işlemi başarılı! Giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        ModelState.AddModelError("", "Kayıt işlemi sırasında bir hata oluştu.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userService.LoginAsync(model);
        if (user != null)
        {
            // Set session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.SetString("UserEmail", user.Email);

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "E-posta adresi veya parola hatalı.");
        return View(model);
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userService.GetByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("Email", "Bu e-posta adresine sahip kullanıcı bulunamadı.");
            return View(model);
        }

        // In a real application, you would generate a secure token and send it via email
        // For now, we'll redirect to reset password with email
        TempData["Email"] = model.Email;
        return RedirectToAction("ResetPassword");
    }

    [HttpGet]
    public IActionResult ResetPassword()
    {
        var email = TempData["Email"] as string;
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("ForgotPassword");
        }

        var model = new ResetPasswordDto { Email = email };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var success = await _userService.ResetPasswordAsync(model);
        if (success)
        {
            TempData["SuccessMessage"] = "Parolanız başarıyla güncellendi. Giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        ModelState.AddModelError("", "Parola sıfırlama işlemi sırasında bir hata oluştu.");
        return View(model);
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        var user = await _userService.GetByIdAsync(userId.Value);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        var model = new ProfileUpdateDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileUpdateDto model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Check if email is being changed and if new email already exists
        var currentUser = await _userService.GetByIdAsync(userId.Value);
        if (currentUser != null && currentUser.Email != model.Email && 
            await _userService.EmailExistsAsync(model.Email, userId.Value))
        {
            ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılmaktadır.");
            return View(model);
        }

        var success = await _userService.UpdateProfileAsync(userId.Value, model);
        if (success)
        {
            // Update session data
            HttpContext.Session.SetString("UserName", $"{model.FirstName} {model.LastName}");
            HttpContext.Session.SetString("UserEmail", model.Email);

            TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }

        ModelState.AddModelError("", "Profil güncelleme işlemi sırasında bir hata oluştu.");
        return View(model);
    }
}