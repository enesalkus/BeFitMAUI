using System.ComponentModel.DataAnnotations;

namespace BeFitMAUI.Models
{
    public class ExerciseStatisticsViewModel
    {
        [Display(Name = "Ćwiczenie")]
        public string ExerciseName { get; set; }

        [Display(Name = "Częstotliwość (Ostatnie 4 tyg.)")]
        public int Frequency { get; set; }

        [Display(Name = "Suma Powtórzeń")]
        public int TotalRepetitions { get; set; }

        [Display(Name = "Śr. Obciążenie (kg)")]
        public double AverageLoad { get; set; }

        [Display(Name = "Maks. Obciążenie (kg)")]
        public double MaxLoad { get; set; }
    }
}
