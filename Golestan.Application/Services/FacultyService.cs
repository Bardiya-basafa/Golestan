namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs;
using DTOs.Faculty;
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


    public async Task<DetailsFacultyDto?> GetDetailsFacultyById(int id)
    {
        try{
            var faculty = await _context.Faculties
                .Where(f => f.Id == id)
                .Select(f => new DetailsFacultyDto()
                {
                    Id = f.Id,
                    MajorName = f.MajorName,
                    BuildingName = f.BuildingName,
                    Budget = f.Badge,
                    StartDate = f.StartDate,
                    StudentsCount = f.Students.Count,
                    InstructorsCount = f.Instructors.Count,
                    ClassesCount = f.Classrooms.Count,
                    CoursesCount = f.Courses.Count,
                })
                .FirstOrDefaultAsync();

            return faculty;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<EditFacultyDto?> GetEditFacultyById(int id)
    {
        try{
            var faculty = await _context.Faculties
                .Where(f => f.Id == id)
                .Select(f => new EditFacultyDto()
                {
                    Id = f.Id,
                    MajorName = f.MajorName,
                    BuildingName = f.BuildingName,
                    Budget = f.Badge,
                })
                .FirstOrDefaultAsync();

            return faculty;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<Dictionary<int, string>?> GetFacultiesMajorNames()
    {
        var facultyOptions = await _context.Faculties.Select(f => new { f.Id, f.MajorName }).Distinct().ToDictionaryAsync(f => f.Id, f => f.MajorName);

        return facultyOptions;
    }

    public async Task<bool> EditFaculty(EditFacultyDto dto)
    {
        try{
            var buildingExist = await VerifyBuilding(dto.BuildingName);
            var majorExist = await VerifyMajor(dto.MajorName);

            if (buildingExist || majorExist){
                return false;
            }

            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (faculty == null){
                return false;
            }

            faculty.MajorName = dto.MajorName;
            faculty.BuildingName = dto.BuildingName;
            faculty.StartDate = dto.StartDate;
            faculty.Badge = dto.Budget;
            _context.Faculties.Update(faculty);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e){
            Console.WriteLine(e);

            return false;
        }
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
