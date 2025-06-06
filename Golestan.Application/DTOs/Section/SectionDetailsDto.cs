namespace Golestan.Application.DTOs.Section;

using Domain.Entities;


public class SectionDetailsDto {

    public int Id { get; set; }

    public AppUser InstructorAppUser { get; set; }

    public int InstructorId { get; set; }

    public TimeSlot TimeSlot { get; set; }

    public Course Course { get; set; }

}
