namespace Golestan.Application.DTOs.Selection;

using Domain.Enums;


public class AvailableSectionsDto {

    public int SectionId { get; set; }

    public string CourseName { get; set; }

    public string InstructorFullName { get; set; }

    public TimeSlot TimeSlot { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public string ClassroomNumber { get; set; }

}
