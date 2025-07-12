namespace Golestan.Application.DTOs.Course;

using System.ComponentModel.DataAnnotations;
using Instructor;


public class CourseInstructorDto {

    [Required]
    public int InstructorId { get; set; }

    [Required]
    public int CourseId { get; set; }

    public string CourseName { get; set; }


    public List<InstructorDto> Instructors { get; set; }

}
