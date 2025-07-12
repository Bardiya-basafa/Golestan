namespace Golestan.Application.DTOs.Faculty;

using Classroom;
using Course;
using Domain.Entities;
using Instructor;
using Student;


public class FacultyDto {

    public int Id { get; set; }

    public string BuildingName { get; set; }

    public string MajorName { get; set; }

    public int Budget { get; set; }

    public DateTime StartDate { get; set; }


    public int StudentsCount { get; set; }

    public int InstructorsCount { get; set; }

    public int CoursesCount { get; set; }

    public int ClassesCount { get; set; }

    public List<StudentDto> Students { get; set; } = new List<StudentDto>();

    public List<InstructorDto> Instructors { get; set; } = new List<InstructorDto>();

    public List<CourseDto> Courses { get; set; } = new List<CourseDto>();

    public List<ClassroomDto> Classes { get; set; } = new List<ClassroomDto>();

}
