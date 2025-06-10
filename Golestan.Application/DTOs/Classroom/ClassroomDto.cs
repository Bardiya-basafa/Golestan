namespace Golestan.Application.DTOs.Classroom;

using Domain.Entities;
using Section;


public class ClassroomDto {

    public string ClassroomNumber { get; set; }

    public int ClassroomId { get; set; }

    public string FacultyName { get; set; }

    public int FacultyId { get; set; }

    public int Capacity { get; set; }

    public List<SectionDetailsDto> Sections { get; set; }

}
