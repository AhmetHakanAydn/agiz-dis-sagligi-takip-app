using System.ComponentModel.DataAnnotations;

namespace DentalHealthApp.Core.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Ad alanı gereklidir.")]
    [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı gereklidir.")]
    [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta adresi gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola gereklidir.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Parola en az 8 karakter olmalıdır.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", 
        ErrorMessage = "Parola en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola tekrarı gereklidir.")]
    [Compare("Password", ErrorMessage = "Parolalar uyuşmuyor.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Doğum tarihi gereklidir.")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
}

public class LoginDto
{
    [Required(ErrorMessage = "E-posta adresi gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola gereklidir.")]
    public string Password { get; set; } = string.Empty;
}

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "E-posta adresi gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordDto
{
    [Required(ErrorMessage = "E-posta adresi gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yeni parola gereklidir.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Parola en az 8 karakter olmalıdır.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", 
        ErrorMessage = "Parola en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola tekrarı gereklidir.")]
    [Compare("NewPassword", ErrorMessage = "Parolalar uyuşmuyor.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

public class ProfileUpdateDto
{
    [Required(ErrorMessage = "Ad alanı gereklidir.")]
    [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı gereklidir.")]
    [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta adresi gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Doğum tarihi gereklidir.")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [StringLength(100, MinimumLength = 8, ErrorMessage = "Parola boş bırakılabilir veya en az 8 karakter olmalıdır.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", 
        ErrorMessage = "Parola en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
    public string? NewPassword { get; set; }

    [Compare("NewPassword", ErrorMessage = "Parolalar uyuşmuyor.")]
    public string? ConfirmNewPassword { get; set; }
}