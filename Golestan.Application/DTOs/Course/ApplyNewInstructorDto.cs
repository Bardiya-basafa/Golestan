namespace Golestan.Application.DTOs.Course;

using System.ComponentModel.DataAnnotations;
using Instructor;


public class ApplyNewInstructorDto {

    [Required]
    public int InstructorId { get; set; }

    [Required]
    public int CourseId { get; set; }

    public string CourseName { get; set; }


    public List<InstructorDetailsDto> Instructors { get; set; }

}
