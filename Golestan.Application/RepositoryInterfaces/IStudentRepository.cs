namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using DTOs.ExamResult;
using DTOs.Objection;
using DTOs.Student;
using DTOs.Term;
using Shared.Helpers;


public interface IStudentRepository {


    Task<StudentDto> GetStudentInfo(string studentId);

    Task<Student> GetStudentEntityById(string studentId);

    Task<StudentDto> GetStudentDtoById(string studentId);

    Task<List<StudentDto>> GetFacultyStudents(int facultyId);

    Task<List<TermDto>> GetAllStudentTerms(string studentId);

    Task<List<ExamResultDto>> GetAllTermExamResults(string studentId, int termId);

    Task<List<ExamResultDto>> GetAllExamResults(string studentId);

    Task<ExamResult> GetExamResultForObjection(ObjectionDto objection, string studentId);


    Task<decimal> GetStudentTotalGpa(string studentId);

    Task<Result> RemoveStudent(int studentId);

    Task<Result> UpdateStudent(Student student);

}
