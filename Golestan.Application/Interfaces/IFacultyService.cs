namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs;
using DTOs.Faculty;
using DTOs.Section;


public interface IFacultyService {

    Task<List<FacultyDetailsDto>> GetFaculties();

    Task<FacultyDetailsDto?> GetDetailsFacultyById(int id);

    Task<EditFacultyDto?> GetEditFacultyById(int id);

    Task<Dictionary<int, string>?> GetFacultiesMajorNames();

    Task<Dictionary<int, string>> GetFacultyClassrooms(int facultyId);

    Task<Dictionary<int, string>?> GetFacultyInstructors(int facultyId);

    Task<Dictionary<int, string>?> GetFacultyCourses(int facultyId);


    Task<bool> EditFaculty(EditFacultyDto dto);

    Task<bool> AddFaculty(AddFacultyDto addFacultyDto);

    Task<bool> VerifyMajor(string major);

    Task<bool> VerifyBuilding(string buildingName);

}
