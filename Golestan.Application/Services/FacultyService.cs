namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.Faculty;
using Interfaces;
using Mapster;
using RepositoryInterfaces;
using Shared.Helpers;


public class FacultyService(IFacultyRepository facultyRepository) : IFacultyService {

    public async Task<List<FacultyDto>> GetFaculties()
    {
        return await facultyRepository.GetFaculties();
    }

    public async Task<FacultyDto> GetFacultyDtoById(int id)
    {
        return await facultyRepository.GetFacultyById(id);
    }


    public async Task<Dictionary<int, string>?> GetFacultiesMajorNamesOptions()
    {
        return await facultyRepository.GetFacultyMajorNamesOptions();
    }

    public async Task<Dictionary<int, string>?> GetFacultyClassroomsOptions(int facultyId)
    {
        return await facultyRepository.GetFacultyClassroomsOptions(facultyId);
    }

    public async Task<Dictionary<int, string>?> GetFacultyInstructorsOptions(int facultyId)
    {
        return await facultyRepository.GetFacultyInstructorsOptions(facultyId);
    }

    public async Task<Dictionary<int, string>?> GetFacultyCoursesOptions(int facultyId)
    {
        return await facultyRepository.GetFacultyCoursesOptions(facultyId);
    }

    public async Task<Result> UpdateFacutly(FacultyDto faculty)
    {
        if (await facultyRepository.BuildingNameExist(faculty.BuildingName)){
            return new Result()
            {
                Message = "Building name already exist",
            };
        }

        if (await facultyRepository.MajorNameExist(faculty.MajorName)){
            return new Result()
            {
                Message = "Major name already exist",
            };
        }


        return await facultyRepository.UpdateFaculty(faculty.Adapt<Faculty>());
    }


    public async Task<Result> AddFaculty(FacultyDto faculty)
    {
        return await facultyRepository.AddFaculty(faculty.Adapt<Faculty>());
    }

    public async Task<bool> VerifyMajorName(string majorName)
    {
        return await facultyRepository.MajorNameExist(majorName);
    }

    public async Task<bool> VerifyBuildingName(string buildingName)
    {
        return await facultyRepository.BuildingNameExist(buildingName);
    }

}
