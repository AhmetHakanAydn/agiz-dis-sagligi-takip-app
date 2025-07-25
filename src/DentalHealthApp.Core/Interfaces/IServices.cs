using DentalHealthApp.Core.DTOs;
using DentalHealthApp.Core.Entities;

namespace DentalHealthApp.Core.Interfaces;

public interface IUserService
{
    Task<User?> RegisterAsync(RegisterDto registerDto);
    Task<User?> LoginAsync(LoginDto loginDto);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> UpdateProfileAsync(int userId, ProfileUpdateDto profileDto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> EmailExistsAsync(string email, int excludeUserId);
}

public interface IGoalService
{
    Task<IEnumerable<GoalDto>> GetUserGoalsAsync(int userId);
    Task<GoalDto?> GetGoalByIdAsync(int goalId, int userId);
    Task<GoalDto?> CreateGoalAsync(int userId, GoalCreateDto goalDto);
    Task<bool> DeleteGoalAsync(int goalId, int userId);
    Task<bool> HasActivitiesAsync(int goalId);
}

public interface IActivityService
{
    Task<IEnumerable<ActivityDto>> GetUserActivitiesAsync(int userId);
    Task<IEnumerable<ActivityDto>> GetUserActivitiesForPeriodAsync(int userId, DateTime startDate, DateTime endDate);
    Task<ActivityDto?> CreateActivityAsync(int userId, ActivityCreateDto activityDto);
    Task<bool> DeleteActivityAsync(int activityId, int userId);
    Task<IDictionary<string, int>> GetLast7DaysStatsAsync(int userId);
}

public interface INoteService
{
    Task<IEnumerable<NoteDto>> GetUserNotesAsync(int userId);
    Task<IEnumerable<NoteDto>> GetUserNotesForPeriodAsync(int userId, DateTime startDate, DateTime endDate);
    Task<NoteDto?> CreateNoteAsync(int userId, NoteCreateDto noteDto);
    Task<bool> DeleteNoteAsync(int noteId, int userId);
}

public interface IRecommendationService
{
    Task<IEnumerable<Recommendation>> GetAllRecommendationsAsync();
    Task<IEnumerable<Recommendation>> GetRandomRecommendationsAsync(int count = 3);
}

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string toEmail, string firstName);
    Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
}