namespace Golestan.Application.DTOs.Course;

using Domain.Enums;
using Instructor;


public class CourseActionsDto {

    public string CourseName { get; set; }

    public int CourseId { get; set; }

    public int FacultyId { get; set; }

    public int Unit { get; set; }

    public DateTime ExamDateTime { get; set; }

    public TimeSlot ExamTimeSlot { get; set; }

    public string FacultyName { get; set; }

    public List<InstructorDetailsDto> Instructors { get; set; }

}
