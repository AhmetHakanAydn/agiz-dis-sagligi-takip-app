using AgizDisSagligiApp.Core.DTOs;
using AgizDisSagligiApp.Core.Entities;
using AgizDisSagligiApp.Core.Enums;
using AgizDisSagligiApp.Core.Interfaces;
using AutoMapper;

namespace AgizDisSagligiApp.Application.Services
{
    public interface IGoalService
    {
        Task<GoalDto> CreateGoalAsync(string userId, CreateGoalDto createGoalDto);
        Task<GoalDto> UpdateGoalAsync(int id, UpdateGoalDto updateGoalDto);
        Task<bool> DeleteGoalAsync(int id);
        Task<GoalDto?> GetGoalByIdAsync(int id);
        Task<IEnumerable<GoalDto>> GetUserGoalsAsync(string userId);
        Task<IEnumerable<GoalDto>> GetActiveGoalsAsync(string userId);
        Task<IEnumerable<GoalDto>> GetCompletedGoalsAsync(string userId);
        Task<IEnumerable<GoalDto>> GetOverdueGoalsAsync(string userId);
        Task<bool> UpdateGoalProgressAsync(int goalId, int progress);
        Task<bool> CompleteGoalAsync(int goalId);
        Task<decimal> GetGoalCompletionRateAsync(string userId);
    }

    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GoalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GoalDto> CreateGoalAsync(string userId, CreateGoalDto createGoalDto)
        {
            var goal = _mapper.Map<Goal>(createGoalDto);
            goal.UserId = userId;
            goal.CreatedAt = DateTime.UtcNow;
            goal.Status = GoalStatus.Active;

            await _unitOfWork.Goals.AddAsync(goal);
            await _unitOfWork.SaveChangesAsync();

            var goalDto = _mapper.Map<GoalDto>(goal);
            MapCalculatedProperties(goal, goalDto);
            return goalDto;
        }

        public async Task<GoalDto> UpdateGoalAsync(int id, UpdateGoalDto updateGoalDto)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(id);
            if (goal == null)
                throw new ArgumentException("Goal not found");

            _mapper.Map(updateGoalDto, goal);
            await _unitOfWork.Goals.UpdateAsync(goal);
            await _unitOfWork.SaveChangesAsync();

            var goalDto = _mapper.Map<GoalDto>(goal);
            MapCalculatedProperties(goal, goalDto);
            return goalDto;
        }

        public async Task<bool> DeleteGoalAsync(int id)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(id);
            if (goal == null)
                return false;

            await _unitOfWork.Goals.DeleteAsync(goal);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<GoalDto?> GetGoalByIdAsync(int id)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(id);
            if (goal == null)
                return null;

            var goalDto = _mapper.Map<GoalDto>(goal);
            MapCalculatedProperties(goal, goalDto);
            return goalDto;
        }

        public async Task<IEnumerable<GoalDto>> GetUserGoalsAsync(string userId)
        {
            var goals = await _unitOfWork.Goals.GetUserGoalsAsync(userId);
            return goals.Select(g =>
            {
                var dto = _mapper.Map<GoalDto>(g);
                MapCalculatedProperties(g, dto);
                return dto;
            });
        }

        public async Task<IEnumerable<GoalDto>> GetActiveGoalsAsync(string userId)
        {
            var goals = await _unitOfWork.Goals.GetActiveGoalsAsync(userId);
            return goals.Select(g =>
            {
                var dto = _mapper.Map<GoalDto>(g);
                MapCalculatedProperties(g, dto);
                return dto;
            });
        }

        public async Task<IEnumerable<GoalDto>> GetCompletedGoalsAsync(string userId)
        {
            var goals = await _unitOfWork.Goals.GetCompletedGoalsAsync(userId);
            return goals.Select(g =>
            {
                var dto = _mapper.Map<GoalDto>(g);
                MapCalculatedProperties(g, dto);
                return dto;
            });
        }

        public async Task<IEnumerable<GoalDto>> GetOverdueGoalsAsync(string userId)
        {
            var goals = await _unitOfWork.Goals.GetOverdueGoalsAsync(userId);
            return goals.Select(g =>
            {
                var dto = _mapper.Map<GoalDto>(g);
                MapCalculatedProperties(g, dto);
                return dto;
            });
        }

        public async Task<bool> UpdateGoalProgressAsync(int goalId, int progress)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(goalId);
            if (goal == null)
                return false;

            goal.CurrentCount = progress;
            
            // Auto-complete goal if target is reached
            if (goal.CurrentCount >= goal.TargetCount && goal.Status == GoalStatus.Active)
            {
                goal.Status = GoalStatus.Completed;
                goal.CompletedAt = DateTime.UtcNow;
            }

            await _unitOfWork.Goals.UpdateAsync(goal);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteGoalAsync(int goalId)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(goalId);
            if (goal == null)
                return false;

            goal.Status = GoalStatus.Completed;
            goal.CompletedAt = DateTime.UtcNow;
            goal.CurrentCount = goal.TargetCount;

            await _unitOfWork.Goals.UpdateAsync(goal);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetGoalCompletionRateAsync(string userId)
        {
            return await _unitOfWork.Goals.GetGoalCompletionRateAsync(userId);
        }

        private static void MapCalculatedProperties(Goal goal, GoalDto goalDto)
        {
            goalDto.ProgressPercentage = goal.ProgressPercentage;
            goalDto.IsCompleted = goal.IsCompleted;
            goalDto.DaysRemaining = goal.DaysRemaining;
            goalDto.IsOverdue = goal.IsOverdue;
        }
    }
}