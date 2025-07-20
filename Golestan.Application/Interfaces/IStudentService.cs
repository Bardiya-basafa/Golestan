namespace Golestan.Application.Interfaces;

using DTOs.ExamResult;
using DTOs.Objection;
using DTOs.Student;
using DTOs.Term;
using Shared.Helpers;


public interface IStudentService {

    Task<StudentDto> GetStudentInfo(string studentId);

    Task<List<StudentDto>> GetFacultyStudents(int facultyId);

    Task<StudentDto> GetStudentSections(string studentId);

    Task<List<TermDto>> GetAllStudentTerms(string studentId);


    Task<Result> Rmove(int studentId);

}
