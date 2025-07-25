using AgizDisSagligiApp.Core.DTOs;
using AgizDisSagligiApp.Core.Entities;
using AgizDisSagligiApp.Core.Enums;
using AgizDisSagligiApp.Core.Interfaces;
using AutoMapper;

namespace AgizDisSagligiApp.Application.Services
{
    public interface IActivityService
    {
        Task<ActivityDto> CreateActivityAsync(string userId, CreateActivityDto createActivityDto);
        Task<ActivityDto> UpdateActivityAsync(int id, UpdateActivityDto updateActivityDto);
        Task<bool> DeleteActivityAsync(int id);
        Task<ActivityDto?> GetActivityByIdAsync(int id);
        Task<IEnumerable<ActivityDto>> GetUserActivitiesAsync(string userId);
        Task<IEnumerable<ActivityDto>> GetUserActivitiesByDateRangeAsync(string userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<ActivityDto>> GetUserActivitiesByTypeAsync(string userId, ActivityType type);
        Task<int> GetUserActivityStreakAsync(string userId, ActivityType type);
        Task<Dictionary<DateTime, int>> GetUserActivityStatisticsAsync(string userId, DateTime startDate, DateTime endDate);
    }

    public class ActivityService : IActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActivityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ActivityDto> CreateActivityAsync(string userId, CreateActivityDto createActivityDto)
        {
            var activity = _mapper.Map<Activity>(createActivityDto);
            activity.UserId = userId;
            activity.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Activities.AddAsync(activity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<ActivityDto> UpdateActivityAsync(int id, UpdateActivityDto updateActivityDto)
        {
            var activity = await _unitOfWork.Activities.GetByIdAsync(id);
            if (activity == null)
                throw new ArgumentException("Activity not found");

            _mapper.Map(updateActivityDto, activity);
            await _unitOfWork.Activities.UpdateAsync(activity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<bool> DeleteActivityAsync(int id)
        {
            var activity = await _unitOfWork.Activities.GetByIdAsync(id);
            if (activity == null)
                return false;

            await _unitOfWork.Activities.DeleteAsync(activity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<ActivityDto?> GetActivityByIdAsync(int id)
        {
            var activity = await _unitOfWork.Activities.GetByIdAsync(id);
            return activity == null ? null : _mapper.Map<ActivityDto>(activity);
        }

        public async Task<IEnumerable<ActivityDto>> GetUserActivitiesAsync(string userId)
        {
            var activities = await _unitOfWork.Activities.GetUserActivitiesAsync(userId);
            return _mapper.Map<IEnumerable<ActivityDto>>(activities);
        }

        public async Task<IEnumerable<ActivityDto>> GetUserActivitiesByDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            var activities = await _unitOfWork.Activities.GetUserActivitiesByDateRangeAsync(userId, startDate, endDate);
            return _mapper.Map<IEnumerable<ActivityDto>>(activities);
        }

        public async Task<IEnumerable<ActivityDto>> GetUserActivitiesByTypeAsync(string userId, ActivityType type)
        {
            var activities = await _unitOfWork.Activities.GetUserActivitiesByTypeAsync(userId, type);
            return _mapper.Map<IEnumerable<ActivityDto>>(activities);
        }

        public async Task<int> GetUserActivityStreakAsync(string userId, ActivityType type)
        {
            return await _unitOfWork.Activities.GetUserActivityStreakAsync(userId, type);
        }

        public async Task<Dictionary<DateTime, int>> GetUserActivityStatisticsAsync(string userId, DateTime startDate, DateTime endDate)
        {
            return await _unitOfWork.Activities.GetUserActivityStatisticsAsync(userId, startDate, endDate);
        }
    }
}