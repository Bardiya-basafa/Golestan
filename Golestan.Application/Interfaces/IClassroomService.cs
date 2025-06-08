namespace Golestan.Application.Interfaces;

using Golestan.Application.DTOs.Classroom;
using Golestan.Shared.Helpers;


public interface IClassroomService {

    Task<FacultiesClassroomsDto> GetFacultyClassrooms(int facultyId);

    Task<ClassroomManagementDto> GetClassroomManagementDto(int classroomId);


    Task<Result> AddClassroom(AddClassroomDto dto);

    Task<bool> VerifyClassroomNumber(string classNumber, int facultyId);

}
