namespace Golestan.Application.DTOs.Course;

using System.ComponentModel.DataAnnotations;


public class SetExamForCourseDto {

    [Required(ErrorMessage = "Course exam time is required")]
    public DateTime ExamDate { get; set; }

    [Required(ErrorMessage = "Course exam time is required")]
    public int TimeSlotId { get; set; }

    [Required]
    public int ClassroomId { get; set; }

    [Required]
    public int CourseId { get; set; }

    public DateTime ExamStartDate { get; set; }

    public DateTime ExamEndDate { get; set; }

    public string CourseName { get; set; }

    public Dictionary<int, string>? Classrooms { get; set; }

}
