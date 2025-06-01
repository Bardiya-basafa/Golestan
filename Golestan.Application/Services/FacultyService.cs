namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;


public class FacultyService : IFacultyService {

    private readonly AppDbContext _context;

    public FacultyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Faculty>> GetFaculties()
    {
        var faculties = await _context.Faculties.ToListAsync();

        return faculties;
    }

    public async Task<bool> AddFaculty(AddFacultyDto addFacultyDto)
    {
        try{
            var faculty = new Faculty()
            {
                Badge = addFacultyDto.Budget,
                BuildingName = addFacultyDto.BuildingName,
                MajorName = addFacultyDto.MajorName,
                StartDate = addFacultyDto.StartDate,
            };

            _context.Faculties.Add(faculty);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e){
            Console.WriteLine(e);

            return false;
        }
    }

    public async Task<bool> VerifyMajor(string major)
    {
        var exist = await _context.Faculties.AnyAsync(f => f.MajorName == major);

        return exist;
    }

    public Task<bool> VerifyBuilding(string buildingName)
    {
        var exist = _context.Faculties.AnyAsync(f => f.BuildingName == buildingName);

        return exist;
    }

}
