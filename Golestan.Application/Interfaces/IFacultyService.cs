namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs;


public interface IFacultyService {

    Task<List<Faculty>> GetFaculties();

    Task<DetailsFacultyDto?> GetDetailsFacultyById(int id);

    Task<EditFacultyDto?> GetEditFacultyById(int id);

    Task<bool> AddFaculty(AddFacultyDto addFacultyDto);

    Task<bool> VerifyMajor(string major);

    Task<bool> VerifyBuilding(string buildingName);

}
