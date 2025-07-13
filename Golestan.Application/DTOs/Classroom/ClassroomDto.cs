namespace Golestan.Application.DTOs.Classroom;

using Domain.Entities;
using Faculty;
using Section;


public class ClassroomDto {

    public int Id { get; set; }

    public string ClassroomNumber { get; set; }

    public int ClassroomId { get; set; }

    public FacultyDto Faculty { get; set; }

    public int FacultyId { get; set; }

    public int Capacity { get; set; }

    public List<SectionDto> Sections { get; set; }

}
