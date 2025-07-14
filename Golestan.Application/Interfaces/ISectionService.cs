namespace Golestan.Application.Interfaces;

using DTOs.Section;
using DTOs.Student;
using Shared.Helpers;


public interface ISectionService {

    Task<List<SectionDto>> GetFacultySections(int facultyId);

    Task<SectionDto> GetSectionById(int sectionId);

    Task<List<StudentDto>> GetAvailableStudents(int sectionId);


    Task<Result> AddStudentsToSection(List<int> studentIds, int sectionId);

    Task<Result> RemoveStudentFromSection(int studentId, int sectionId);

    Task<Result> AddSection(AddSectionDto dto);


}
