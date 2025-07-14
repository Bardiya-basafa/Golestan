namespace Golestan.Application.DTOs.Instructor;

using Course;
using Domain.Entities;
using ExamResult;
using Faculty;
using Section;


public class InstructorDto {

    public int Id { get; set; }

    public AppUser AppUser { get; set; }

    public string FullName { get; set; }

    public string InstructorNumber { get; set; }

    public DateTime HireDate { get; set; }

    public int Salary { get; set; }


    public FacultyDto Faculty { get; set; }


    public List<SectionDto> Sections { get; set; } = new List<SectionDto>();

    public List<CourseDto> Courses { get; set; } = new List<CourseDto>();

    public List<ExamResultDto> ExamResults { get; set; } = new List<ExamResultDto>();

}
