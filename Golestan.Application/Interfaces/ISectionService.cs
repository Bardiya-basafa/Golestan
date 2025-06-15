namespace Golestan.Application.Interfaces;

using DTOs.Section;
using DTOs.Student;
using Shared.Helpers;


public interface ISectionService {

    Task<SectionManagementDto> GetFacultySections(int facultyId);

    Task<SectionActionsDto> GetSectionActionsDto(int sectionId);

    Task<List<StudentDetailsDto>> GetAvailableStudents(int sectionId, int facultyId);

    Task<SectionDetailsDto> GetSectionDetailsById(int sectionId);

    Task<Result> AddStudentsToSection(List<int> studentIds, int sectionId);

    Task<Result> AddNewSection(AddSectionDto dto);

}
