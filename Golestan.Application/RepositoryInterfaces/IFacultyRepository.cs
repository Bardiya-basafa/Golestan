namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using DTOs.Faculty;
using Shared.Helpers;


public interface IFacultyRepository {

    Task<List<FacultyDto>> GetFaculties();

    Task<FacultyDto> GetFacultyById(int facultyId);

    Task<Dictionary<int, string>?> GetFacultyMajorNamesOptions();

    Task<Dictionary<int, string>?> GetFacultyClassroomsOptions(int facultyId);

    Task<Dictionary<int, string>?> GetFacultyInstructorsOptions(int facultyId);

    Task<Dictionary<int, string>?> GetFacultyCoursesOptions(int facultyId);

    Task<Result> UpdateFaculty(Faculty faculty);

    Task<Result> AddFaculty(Faculty faculty);

    Task<bool> MajorNameExist(string majorName);

    Task<bool> BuildingNameExist(string buildingName);

}
