namespace Golestan.Application.Services;

using DTOs.Classroom;


public interface IClassroomService {

    Task<ManageFucultyClassroomDto> GetFacultyClassrooms(int facultyId);

}
