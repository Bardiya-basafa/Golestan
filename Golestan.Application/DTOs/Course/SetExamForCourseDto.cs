namespace Golestan.Application.DTOs.Course;

using System.ComponentModel.DataAnnotations;



public class SetExamForCourseDto {

    [Required(ErrorMessage = "Course exam time is required")]
    public DateTime ExamDateTime { get; set; }

    [Required(ErrorMessage = "Course exam time is required")]
    public int ExamTimeSlotId { get; set; }

    [Required]
    public int ExamClassroomId { get; set; }

    [Required]
    public int CourseId { get; set; }

    public DateTime ExamStartDate { get; set; }

    public DateTime ExamEndDate { get; set; }

    public string CourseName { get; set; }

}
