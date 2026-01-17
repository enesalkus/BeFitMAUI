using System.ComponentModel.DataAnnotations;

namespace BeFitMAUI.Models
{
    public class ExercisePerformedCreateDto
    {
        [Required]
        public int TrainingSessionId { get; set; }

        [Required]
        [Display(Name = "Exercise Type")]
        public int ExerciseTypeId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Load must be a positive number.")]
        [Display(Name = "Load (kg)")]
        public double Load { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Sets must be between 1 and 100.")]
        public int Sets { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Repetitions must be between 1 and 1000.")]
        public int Repetitions { get; set; }
    }
}
