using System.ComponentModel.DataAnnotations;

namespace BeFitMAUI.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Exercise Name")]
        public string Name { get; set; }
    }
}
