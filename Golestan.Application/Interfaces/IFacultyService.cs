namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs;
using DTOs.Faculty;
using DTOs.Section;
using Shared.Helpers;


public interface IFacultyService {

    Task<List<FacultyDto>> GetFaculties();

    Task<FacultyDto> GetFacultyById(int facultyId);


    Task<Dictionary<int, string>?> GetFacultiesMajorNamesOptions();

    Task<Dictionary<int, string>?> GetFacultyClassroomsOptions(int facultyId);

    Task<Dictionary<int, string>?> GetFacultyInstructorsOptions(int facultyId);

    Task<Dictionary<int, string>?> GetFacultyCoursesOptions(int facultyId);


    Task<Result> UpdateFacutly(FacultyDto faculty);

    Task<Result> AddFaculty(FacultyDto faculty);

    Task<bool> VerifyMajorName(string majorName);

    Task<bool> VerifyBuildingName(string buildingName);

}
