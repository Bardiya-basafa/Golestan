namespace Golestan.Application.DTOs.Student;

using Course;
using Domain.Entities;
using ExamResult;
using Faculty;
using Section;
using Term;


public class StudentDto {

    public int Id { get; set; }
    
    public string FullName { get; set; }

    public string Email { get; set; }

    public string StudentNumber { get; set; }
    
    public string FacultyName { get; set; }


    public AppUser AppUser { get; set; }
    public decimal Gpa { get; set; }


    public DateTime EnteredDate { get; set; }


    public FacultyDto Faculty { get; set; }

    public List<SectionDto> Sections { get; set; } = new List<SectionDto>();

    public List<CourseDto> PassedCourses { get; set; } = new List<CourseDto>();

    public List<ExamResultDto> ExamResults { get; set; } = new List<ExamResultDto>();

    public List<TermDto> Terms { get; set; } = new List<TermDto>();

}
