using BeFitMAUI.Models;
using BeFitMAUI.Data;
using Microsoft.EntityFrameworkCore;

namespace BeFitMAUI.Services
{
    public class StatsService
    {
        private readonly BeFitDbContext _context;

        public StatsService(BeFitDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExerciseStatisticsViewModel>> GetStatsAsync()
        {
             var fourWeeksAgo = DateTime.Now.AddDays(-28);

            var exercises = await _context.ExercisePerformeds
                .Include(e => e.ExerciseType)
                .Include(e => e.TrainingSession)
                .ToListAsync();

            var stats = exercises
                .GroupBy(e => e.ExerciseType.Name)
                .Select(g => new ExerciseStatisticsViewModel
                {
                    ExerciseName = g.Key,
                    Frequency = g.Count(e => e.TrainingSession.StartTime >= fourWeeksAgo),
                    TotalRepetitions = g.Sum(e => e.Sets * e.Repetitions),
                    AverageLoad = g.Average(e => e.Load),
                    MaxLoad = g.Max(e => e.Load)
                })
                .ToList();
            
            return stats;
        }
    }
}
