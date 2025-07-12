namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using Shared.Helpers;


public interface IClassroomRepository {

    Task<List<Classroom>> GetFacultyClassrooms(int facultyId);

    Task<Classroom?> GetClassroomById(int classroomId);

    Task<Result> AddClassroom(Classroom classroom);

    Task<Result> RemoveClassroom(int classroomId);
    
    Task<bool> VerifyClassroomNumber(string classroomNumber,int facultyId);

}
