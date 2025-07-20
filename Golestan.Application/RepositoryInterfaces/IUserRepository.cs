namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using Shared.Helpers;


public interface IUserRepository {

    Task<AppUser?> GetUserByUniversalNumber(string universalNumber);

    Task<Result> AddInstructor(Instructor instructor);

    Task<Result> AddStudent(Student student);

    Task<bool> DeleteUser(string id);

    Task<int> StudentCount(int facultyId);

    Task<int> InstructorCount(int facultyId);

}
