using BeFitMAUI.Models;
using BeFitMAUI.Data;
using Microsoft.EntityFrameworkCore;

namespace BeFitMAUI.Services
{
    public class TrainingService
    {
        private readonly BeFitDbContext _context;

        public TrainingService(BeFitDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainingSession>> GetSessionsAsync()
        {
            return await _context.TrainingSessions
                .OrderByDescending(t => t.StartTime)
                .ToListAsync();
        }

        public async Task<TrainingSession> GetSessionAsync(int id)
        {
            return await _context.TrainingSessions
                .Include(t => t.Exercises)
                    .ThenInclude(e => e.ExerciseType)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task SaveSessionAsync(TrainingSession session)
        {
            if (session.Id == 0)
            {
                _context.TrainingSessions.Add(session);
            }
            else
            {
                _context.TrainingSessions.Update(session);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSessionAsync(TrainingSession session)
        {
            _context.TrainingSessions.Remove(session);
            await _context.SaveChangesAsync();
        }

        public async Task SaveExerciseAsync(ExercisePerformed exercise)
        {
            if (exercise.Id == 0)
            {
                _context.ExercisePerformeds.Add(exercise);
            }
            else
            {
                _context.ExercisePerformeds.Update(exercise);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExerciseAsync(ExercisePerformed exercise)
        {
            _context.ExercisePerformeds.Remove(exercise);
            await _context.SaveChangesAsync();
        }
        
        public async Task<ExercisePerformed> GetExerciseAsync(int id)
        {
             return await _context.ExercisePerformeds
                .Include(e => e.TrainingSession)
                .Include(e => e.ExerciseType)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
