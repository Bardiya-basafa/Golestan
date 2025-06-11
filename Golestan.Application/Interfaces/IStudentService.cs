namespace Golestan.Application.Interfaces;

using DTOs.Student;
using Shared.Helpers;


public interface IStudentService {

    Task<StudentManagementDto> GetFacultyStudents(int facultyId);


}
