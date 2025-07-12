namespace Golestan.Application.DTOs.Section;

using Classroom;
using Course;
using Domain.Entities;
using Domain.Enums;
using Instructor;
using Student;
using Term;


public class SectionDto {

    public int Id { get; set; }

    public AppUser InstructorAppUser { get; set; }


    public TimeSlot TimeSlot { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public DateTime ExamDate { get; set; }

    public TimeSlot ExamTimeSlot { get; set; }


    public CourseDto Course { get; set; }

    public ClassroomDto Classroom { get; set; }

    public InstructorDto Instructor { get; set; }

    public TermDto Term { get; set; }


    public List<StudentDto> Students { get; set; } = new List<StudentDto>();

}
