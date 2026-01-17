using System.ComponentModel.DataAnnotations;

namespace BeFitMAUI.Models
{
    public class ExerciseStatisticsViewModel
    {
        [Display(Name = "Exercise Name")]
        public string ExerciseName { get; set; }

        [Display(Name = "Frequency (Last 4 Weeks)")]
        public int Frequency { get; set; }

        [Display(Name = "Total Repetitions")]
        public int TotalRepetitions { get; set; }

        [Display(Name = "Average Load (kg)")]
        public double AverageLoad { get; set; }

        [Display(Name = "Max Load (kg)")]
        public double MaxLoad { get; set; }
    }
}
