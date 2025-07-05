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
        try{
            return await facultyRepository.GetFaculties();
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<FacultyDto> GetFacultyById(int id)
    {
        try{
            return await facultyRepository.GetFacultyById(id);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }


    public async Task<Dictionary<int, string>?> GetFacultiesMajorNamesOptions()
    {
        try{
            return await facultyRepository.GetFacultyMajorNamesOptions();
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Dictionary<int, string>?> GetFacultyClassroomsOptions(int facultyId)
    {
        try{
            return await facultyRepository.GetFacultyClassroomsOptions(facultyId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Dictionary<int, string>?> GetFacultyInstructorsOptions(int facultyId)
    {
        try{
            return await facultyRepository.GetFacultyInstructorsOptions(facultyId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Dictionary<int, string>?> GetFacultyCoursesOptions(int facultyId)
    {
        try{
            return await facultyRepository.GetFacultyCoursesOptions(facultyId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> UpdateFacutly(FacultyDto faculty)
    {
        try{
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
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }


    public async Task<Result> AddFaculty(FacultyDto faculty)
    {
        try{
            return await facultyRepository.AddFaculty(faculty.Adapt<Faculty>());
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
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
