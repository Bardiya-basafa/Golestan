namespace Golestan.Application.DTOs.Section;

using Domain.Entities;
using Domain.Enums;


public class SectionDetailsDto {

    public int Id { get; set; }

    public AppUser InstructorAppUser { get; set; }

    public int InstructorId { get; set; }

    public string InstructorFullName { get; set; }

    public TimeSlot TimeSlot { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public DateTime ExamDate { get; set; }

    public TimeSlot ExamTimeSlot { get; set; }

    public string CourseName { get; set; }

    public string ClassNumber { get; set; }

    public int ClassCapacity { get; set; }

    public int RemainCapacity { get; set; }

    public int CurrentStudents { get; set; }

}
