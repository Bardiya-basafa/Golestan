namespace Golestan.Application.Services;

using DTOs.Student;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;


public class StudentService : IStudentService {

    private readonly AppDbContext _context;

    public StudentService(AppDbContext context)
    {
        _context = context;
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
