namespace Golestan.Application.Services;

using Domain.Entities;
using Domain.Enums;
using DTOs.Student;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;


public class StudentService : IStudentService {

    private readonly AppDbContext _context;

    private readonly UserManager<AppUser> _userManager;

    public StudentService(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<StudentManagementDto> GetFacultyStudents(int facultyId)
    {
        try{
            var dto = new StudentManagementDto();

            dto.Students = await _context.Students
                .Where(s => s.FacultyId == facultyId)
                .Select(s => new StudentDetailsDto()
                {
                    Id = s.Id,
                    AppUserId = s.AppUserId,
                    Email = s.AppUser.Email,
                    FacultyId = s.FacultyId,
                    FacultyName = s.Faculty.MajorName,
                    FullName = s.FullName,
                    StudentNumber = s.StudentNumber,
                })
                .ToListAsync();

            var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Id == facultyId);

            if (faculty == null){
                throw new ArgumentException();
            }

            dto.FacultyId = faculty.Id;
            dto.FacultyName = faculty.MajorName;

            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

}
