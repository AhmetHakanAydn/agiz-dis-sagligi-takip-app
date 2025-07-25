using DentalHealthApp.Core.Entities;
using DentalHealthApp.Core.Interfaces;
using DentalHealthApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthApp.Business.Services;

public class RecommendationService : IRecommendationService
{
    private readonly DentalHealthDbContext _context;

    public RecommendationService(DentalHealthDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Recommendation>> GetAllRecommendationsAsync()
    {
        return await _context.Recommendations
            .Where(r => r.IsActive)
            .OrderBy(r => r.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recommendation>> GetRandomRecommendationsAsync(int count = 3)
    {
        var allRecommendations = await _context.Recommendations
            .Where(r => r.IsActive)
            .ToListAsync();

        if (allRecommendations.Count <= count)
            return allRecommendations;

        var random = new Random();
        return allRecommendations
            .OrderBy(x => random.Next())
            .Take(count)
            .ToList();
    }
}