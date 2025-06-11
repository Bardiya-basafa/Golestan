namespace Golestan.Application.Interfaces;

using DTOs.Instructor;
using DTOs.Student;
using Shared.Helpers;


public interface IUserService {

    Task<Result> RegisterNewInstructor(AddInstructorDto dto);

    Task<Result> RegisterNewStudent(AddStudentDto dto);

}
