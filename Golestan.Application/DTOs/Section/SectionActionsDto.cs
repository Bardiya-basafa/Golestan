namespace Golestan.Application.DTOs.Section;

using Student;


public class SectionActionsDto {

    public int Id { get; set; }

    public int CourseId { get; set; }

    public string CourseName { get; set; }

    public string CourseCode { get; set; }

    public int InstructorId { get; set; }

    public string InstructorName { get; set; }

    public int ClassroomId { get; set; }

    public string ClassroomName { get; set; }

    public int ClassroomCapacity { get; set; }

    public string DayOfWeek { get; set; }

    public string TimeSlotDescription { get; set; }

    public List<StudentDetailsDto> Students { get; set; } = new List<StudentDetailsDto>();

}
