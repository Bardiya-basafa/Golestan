namespace Golestan.Application.Interfaces;

using DTOs.ExamResult;
using DTOs.Student;
using DTOs.Term;
using Shared.Helpers;


public interface IStudentService {

    Task<StudentManagementDto> GetFacultyStudents(int facultyId);

    Task<StudentSectionsDto> GetStudentSections(int studentId);

    Task<List<TermDetailsDto>> GetAllStudentTerms(int studentId);

    Task<List<ExamResultDetailsDto>> GetTermExamResults(int termId, int studentId);

    Task<List<ExamResultDetailsDto>?> GetActiveExamResults(int studentId);

    Task<Result> SubmitObjection(SubmitObjectionDto dto);

}
