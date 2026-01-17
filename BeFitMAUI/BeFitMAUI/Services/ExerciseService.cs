using BeFitMAUI.Models;
using BeFitMAUI.Data;
using Microsoft.EntityFrameworkCore;

namespace BeFitMAUI.Services
{
    public class ExerciseService
    {
        private readonly BeFitDbContext _context;

        public ExerciseService(BeFitDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExerciseType>> GetExerciseTypesAsync()
        {
            return await _context.ExerciseTypes.ToListAsync();
        }

        public async Task<ExerciseType> GetExerciseTypeAsync(int id)
        {
            return await _context.ExerciseTypes.FindAsync(id);
        }

        public async Task SaveExerciseTypeAsync(ExerciseType exerciseType)
        {
            if (exerciseType.Id == 0)
            {
                _context.ExerciseTypes.Add(exerciseType);
            }
            else
            {
                _context.ExerciseTypes.Update(exerciseType);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExerciseTypeAsync(ExerciseType exerciseType)
        {
            _context.ExerciseTypes.Remove(exerciseType);
            await _context.SaveChangesAsync();
        }
    }
}
