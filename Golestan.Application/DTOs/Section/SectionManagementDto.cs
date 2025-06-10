namespace Golestan.Application.DTOs.Section;

public class SectionManagementDto {

    public int FacultyId { get; set; }

    public string FacultyMajorName { get; set; }

    public List<SectionDetailsDto> Sections { get; set; }

}
