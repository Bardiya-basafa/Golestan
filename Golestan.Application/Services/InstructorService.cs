namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.Instructor;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;


public class InstructorService : IInstructorService {

    private readonly AppDbContext _context;

    public InstructorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<InstructorsDto>> GetInstructors()
    {
        try{
            var instructors = await _context.Instructors
                .Select(i => new InstructorsDto()
                {
                    Id = i.Id,
                    AppUserId = i.AppUserId,
                    Email = i.AppUser.Email,
                    FacultyId = i.FacultyId,
                    FacultyName = i.Faculty.MajorName,
                    FullName = $"{i.AppUser.FirstName} {i.AppUser.LastName}",
                    HireDate = i.HireDate,
                    Salary = i.Salary,
                })
                .OrderBy(i => i.Id)
                .Take(10)
                .ToListAsync();

            return instructors;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

}
