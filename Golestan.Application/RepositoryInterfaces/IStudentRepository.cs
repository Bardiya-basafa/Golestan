namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using DTOs.ExamResult;
using DTOs.Objection;
using DTOs.Student;
using DTOs.Term;
using Shared.Helpers;


public interface IStudentRepository {

    Task<StudentDto> GetStudentUserApp(string studentId);

    Task<StudentDto> GetStudentInfo(int studentId);

    Task<Student> GetStudentEntityById(int studentId);

    Task<StudentDto> GetStudentDtoById(int studentId);

    Task<List<StudentDto>> GetFacultyStudents(int facultyId);

    Task<List<TermDto>> GetAllStudentTerms(int studentId);

    Task<List<ExamResultDto>> GetAllTermExamResults(int studentId, int termId);

    Task<List<ExamResultDto>> GetAllExamResults(int studentId);

    Task<ExamResult> GetExamResultForObjection(ObjectionDto objection);

    Task<Result> SubmitObjection(ExamResult examResult, string objection);

    Task<decimal> GetStudentTotalGpa(int studentId);

    Task<Result> RemoveStudent(int studentId);

    Task<Result> UpdateStudent(Student student);

}
