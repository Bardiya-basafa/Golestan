namespace Golestan.Application.Interfaces;

using Golestan.Application.DTOs.Classroom;
using Golestan.Shared.Helpers;


public interface IClassroomService {

    Task<ManageFucultyClassroomDto> GetFacultyClassrooms(int facultyId);

    Task<Result> AddClassroom(AddClassroomDto dto);

    Task<bool> VerifyClassroomNumber(string classNumber, int facultyId);

}
