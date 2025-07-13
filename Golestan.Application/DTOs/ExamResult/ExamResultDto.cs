namespace Golestan.Application.DTOs.ExamResult;

using Course;
using Instructor;
using Section;
using Student;
using Term;


public class ExamResultDto {

    public int Id { get; set; }
    
    public decimal Score { get; set; } = decimal.Zero;

    public string Description { get; set; } = String.Empty;

    public string Objection { get; set; } = String.Empty;

    public DateTime ExamDate { get; set; }

    public TermDto Term { get; set; }

    public StudentDto Student { get; set; }
    
    public InstructorDto Instructor { get; set; }

    public CourseDto Course { get; set; }

    public SectionDto Section { get; set; }

}
