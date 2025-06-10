namespace Golestan.Application.Interfaces;

using DTOs.Student;


public interface IStudentService {

    Task<StudentManagementDto> GetFacultyStudents(int facultyId);

}
