namespace Golestan.Application.DTOs.Student;

using Section;


public class StudentSectionsDto {

    public int StudentId { get; set; }

    public string StudentFullName { get; set; }

    public string StudentNumber { get; set; }

    public List<SectionDetailsDto> Sections { get; set; }

}
