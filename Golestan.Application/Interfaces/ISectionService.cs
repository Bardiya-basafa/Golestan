namespace Golestan.Application.Interfaces;

using DTOs.Section;
using Shared.Helpers;


public interface ISectionService {

    Task<SectionManagementDto> GetFacultySections(int facultyId);

    Task<Result> AddNewSection(AddSectionDto dto);

}
