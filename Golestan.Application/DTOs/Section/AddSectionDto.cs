namespace Golestan.Application.DTOs.Section;

using System.ComponentModel.DataAnnotations;


public class AddSectionDto {

    [Required]
    public int FacultyId { get; set; }

    [Required]
    public int InstructorId { get; set; }

    [Required]
    public int CourseId { get; set; }

    [Required]
    public int ClassroomId { get; set; }

    [Required]
    public int TimeSlotId { get; set; }

    public Dictionary<int, string>? FacultyMajorNames { get; set; }

}
