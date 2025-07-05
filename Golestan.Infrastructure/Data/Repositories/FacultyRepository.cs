namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.Faculty;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class FacultyRepository(AppDbContext context) : IFacultyRepository {

    public async Task<List<FacultyDto>> GetFaculties()
    {
        return await context.Faculties
            .Select(f => new FacultyDto()
            {
                Id = f.Id,
                MajorName = f.MajorName,
                BuildingName = f.BuildingName,
                Budget = f.Budget,
                StartDate = f.StartDate,
                StudentsCount = f.Students.Count,
                InstructorsCount = f.Instructors.Count,
                ClassesCount = f.Classrooms.Count,
                CoursesCount = f.Courses.Count,
            }).ToListAsync();
    }

    public async Task<FacultyDto> GetFacultyById(int facultyId)
    {
        return await context.Faculties
            .Where(f => f.Id == facultyId)
            .Select(f => new FacultyDto()
            {
                Id = f.Id,
                MajorName = f.MajorName,
                BuildingName = f.BuildingName,
                Budget = f.Budget,
                StartDate = f.StartDate,
                StudentsCount = f.Students.Count,
                InstructorsCount = f.Instructors.Count,
                ClassesCount = f.Classrooms.Count,
                CoursesCount = f.Courses.Count,
            })
            .FirstOrDefaultAsync() ?? throw new Exception($"Faculty not found id:{facultyId}");
    }

    public async Task<Dictionary<int, string>?> GetFacultyMajorNamesOptions()
    {
        return await context.Faculties
            .Select(f => new { f.Id, MajorName = f.MajorName })
            .Distinct()
            .ToDictionaryAsync(f => f.Id, f => f.MajorName);
    }

    public async Task<Dictionary<int, string>?> GetFacultyClassroomsOptions(int facultyId)
    {
        return await context.Classrooms
            .Where(c => c.FacultyId == facultyId)
            .Select(c => new { c.Id, c.ClassNumber })
            .Distinct()
            .ToDictionaryAsync(c => c.Id, c => c.ClassNumber);
    }

    public async Task<Dictionary<int, string>?> GetFacultyInstructorsOptions(int facultyId)
    {
        return await context.Instructors
            .Where(i => i.FacultyId == facultyId)
            .Select(i => new { i.Id, i.FullName })
            .Distinct()
            .ToDictionaryAsync(c => c.Id, c => c.FullName);
    }

    public async Task<Dictionary<int, string>?> GetFacultyCoursesOptions(int facultyId)
    {
        return await context.Courses
            .Where(c => c.FacultyId == facultyId)
            .Select(c => new { c.Id, c.CourseName })
            .Distinct()
            .ToDictionaryAsync(c => c.Id, c => c.CourseName);
    }

    public async Task<Result> UpdateFaculty(Faculty faculty)
    {
        var updateFaculty = await context.Faculties
            .FirstOrDefaultAsync(f => f.Id == faculty.Id) ?? throw new Exception($"Faculty not found id:{faculty.Id}");


        faculty.MajorName = faculty.MajorName;
        faculty.BuildingName = faculty.BuildingName;
        faculty.StartDate = faculty.StartDate;
        faculty.Budget = faculty.Budget;
        context.Faculties.Update(updateFaculty);
        await context.SaveChangesAsync();

        return new Result()
        {
            Message = "Faculty updated",
            Succeeded = true
        };
    }

    public async Task<Result> AddFaculty(Faculty faculty)
    {
        context.Faculties.Add(faculty);
        await context.SaveChangesAsync();

        return new Result()
        {
            Message = "Faculty added",
            Succeeded = true
        };
    }

    public async Task<bool> MajorNameExist(string majorName)
    {
        return await context.Faculties.AnyAsync(f => f.MajorName == majorName);
    }

    public async Task<bool> BuildingNameExist(string buildingName)
    {
        return await context.Faculties.AnyAsync(f => f.BuildingName == buildingName);
    }

}
