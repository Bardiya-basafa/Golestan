namespace Golestan.Application.DTOs.Course;

using Exam;
using Faculty;
using Instructor;
using Section;


public class CourseDto {

    public int Id { get; set; }

    public string CourseName { get; set; }

    public int Unit { get; set; }

    public string Description { get; set; }


    public ExamDto Exam { get; set; }

    public FacultyDto Faculty { get; set; }

    public List<InstructorDto> Instructors { get; set; } = new List<InstructorDto>();


    public List<SectionDto> Sections { get; set; } = new List<SectionDto>();

    public List<int> PrerequisiteCourses { get; set; } = new List<int>();

}
