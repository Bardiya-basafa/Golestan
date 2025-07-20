namespace Golestan.Application.Interfaces;

using DTOs.Account;
using DTOs.Instructor;
using DTOs.Student;
using Shared.Helpers;


public interface IUserService {

    Task<Result> RegisterNewInstructor(AddInstructorDto dto);

    Task<Result> RegisterNewStudent(AddStudentDto dto);

    Task<Result> UniversalNumberLogin(UniNumberLoginDto model);

}
