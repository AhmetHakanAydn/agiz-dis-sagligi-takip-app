using DentalHealthApp.Core.DTOs;
using DentalHealthApp.Core.Entities;
using DentalHealthApp.Core.Interfaces;
using DentalHealthApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DentalHealthApp.Business.Services;

public class UserService : IUserService
{
    private readonly DentalHealthDbContext _context;
    private readonly IEmailService _emailService;

    public UserService(DentalHealthDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<User?> RegisterAsync(RegisterDto registerDto)
    {
        // Check if email already exists
        if (await EmailExistsAsync(registerDto.Email))
        {
            return null;
        }

        // Create user
        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            PasswordHash = HashPassword(registerDto.Password),
            DateOfBirth = registerDto.DateOfBirth,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Send welcome email
        try
        {
            await _emailService.SendWelcomeEmailAsync(user.Email, user.FirstName);
        }
        catch
        {
            // Log email error but don't fail registration
        }

        return user;
    }

    public async Task<User?> LoginAsync(LoginDto loginDto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> UpdateProfileAsync(int userId, ProfileUpdateDto profileDto)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;

        // Check if email is being changed and if new email already exists
        if (user.Email != profileDto.Email && await EmailExistsAsync(profileDto.Email, userId))
        {
            return false;
        }

        user.FirstName = profileDto.FirstName;
        user.LastName = profileDto.LastName;
        user.Email = profileDto.Email;
        user.DateOfBirth = profileDto.DateOfBirth;
        user.UpdatedAt = DateTime.UtcNow;

        // Update password if provided
        if (!string.IsNullOrEmpty(profileDto.NewPassword))
        {
            user.PasswordHash = HashPassword(profileDto.NewPassword);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await GetByEmailAsync(resetPasswordDto.Email);
        if (user == null)
            return false;

        user.PasswordHash = HashPassword(resetPasswordDto.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email, int excludeUserId)
    {
        return await _context.Users.AnyAsync(u => u.Email == email && u.Id != excludeUserId);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "dental_health_salt"));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private bool VerifyPassword(string password, string hash)
    {
        var hashedPassword = HashPassword(password);
        return hashedPassword == hash;
    }
}