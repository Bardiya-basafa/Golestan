namespace Golestan.Application.Interfaces;

using DTOs.ExamResult;
using DTOs.Objection;
using DTOs.Student;
using DTOs.Term;
using Shared.Helpers;


public interface IStudentService {

    Task<List<StudentDto>> GetFacultyStudents(int facultyId);

    Task<StudentDto> GetStudentSections(int studentId);

    Task<List<TermDto>> GetAllStudentTerms(int studentId);

    Task<List<ExamResultDto>> GetTermExamResults(int termId, int studentId);

    Task<List<ExamResultDto>?> GetActiveExamResults(int studentId);

    Task<Result> SubmitObjection(ObjectionDto dto);

}
