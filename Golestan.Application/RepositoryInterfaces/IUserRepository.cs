namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using Shared.Helpers;


public interface IUserRepository {

    Task<Result> AddInstructor(Instructor instructor);

    Task<Result> AddStudent(Student student);

    Task<int> StudentCount(int facultyId);

    Task<int> InstructorCount(int facultyId);

}
