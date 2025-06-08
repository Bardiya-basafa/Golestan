namespace Golestan.Application.Interfaces;

using DTOs.Section;


public interface ISectionService {

    Task<SectionManagementDto> GetFacultySections(int facultyId);

}
