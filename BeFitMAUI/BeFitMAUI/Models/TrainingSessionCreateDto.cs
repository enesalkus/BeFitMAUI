using System.ComponentModel.DataAnnotations;

namespace BeFitMAUI.Models
{
    public class TrainingSessionCreateDto
    {
        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; } = DateTime.Now.AddHours(1);
    }
}
