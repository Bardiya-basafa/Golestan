namespace Golestan.Application.DTOs.Section;

using Domain.Enums;
using Student;


public class SectionActionsDto {

    public int Id { get; set; }

    public int FacultyId { get; set; }

    public int CourseId { get; set; }

    public string CourseName { get; set; }


    public int InstructorId { get; set; }

    public string InstructorName { get; set; }

    public int ClassroomId { get; set; }

    public string ClassroomNumber { get; set; }

    public int ClassroomCapacity { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeSlot TimeSlot { get; set; }


    public List<StudentDetailsDto> Students { get; set; } = new List<StudentDetailsDto>();

}
