namespace Golestan.Application.DTOs.Course;

using Instructor;


public class CourseActionsDto {

    public string CourseName { get; set; }

    public int CourseId { get; set; }

    public int FacultyId { get; set; }

    public int Unit { get; set; }

    public DateTime ExamTime { get; set; }

    public string FacultyName { get; set; }

    public List<InstructorDetailsDto> Instructors { get; set; }

}
