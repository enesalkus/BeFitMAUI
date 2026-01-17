using BeFitMAUI.Models;
using Microsoft.EntityFrameworkCore;

namespace BeFitMAUI.Data
{
    public class BeFitDbContext : DbContext
    {
        public DbSet<ExerciseType> ExerciseTypes { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<ExercisePerformed> ExercisePerformeds { get; set; }

        public BeFitDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "BeFitV2.db3");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ExercisePerformed>()
                .HasOne(ep => ep.TrainingSession)
                .WithMany(ts => ts.Exercises)
                .HasForeignKey(ep => ep.TrainingSessionId);

            builder.Entity<ExercisePerformed>()
                .HasOne(ep => ep.ExerciseType)
                .WithMany()
                .HasForeignKey(ep => ep.ExerciseTypeId);

            builder.Entity<ExerciseType>()
                .Property(et => et.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
