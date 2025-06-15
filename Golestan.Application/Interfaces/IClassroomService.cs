namespace Golestan.Application.Interfaces;

using Golestan.Application.DTOs.Classroom;
using Golestan.Shared.Helpers;


public interface IClassroomService {

    Task<ClassroomManagement> GetFacultyClassrooms(int facultyId);

    Task<ClassroomDto> GetClassroomManagementDto(int classroomId);
    
    Task<Result> AddClassroom(AddClassroomDto dto);

    Task<Result> RemoveClassroom(int classroomId);
    Task<bool> VerifyClassroomNumber(string classNumber, int facultyId);

}
