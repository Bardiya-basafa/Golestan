namespace Golestan.Application.DTOs.Course;

using System.ComponentModel.DataAnnotations;
using Validations;


public class AddCourseDto {

    [Required(ErrorMessage = "Course Name is required")]
    [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Must contain at least one letter")]
    [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Course Name must be between 3 and 50 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Course Description is required")]
    [Range(minimum: 1, maximum: 6)]
    public int Unit { get; set; }

    [Required(ErrorMessage = "Course description is required")]
    [StringLength(maximumLength: 150, MinimumLength = 3, ErrorMessage = "Course Name must be between 3 and 50 characters")]

    public string Description { get; set; }

    [Required(ErrorMessage = "Course exam time is required")]
    [NotInPast]
    public DateTime ExamTime { get; set; }

    [Required]
    public int FacultyId { get; set; }
    public string? FacultyMajorName { get; set; }

}
