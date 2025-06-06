namespace Golestan.Application.Interfaces;

using DTOs.Instructor;
using Shared.Helpers;


public interface IUserService {

    Task<Result> RegisterNewInstructor(AddInstructorDto dto);

}
