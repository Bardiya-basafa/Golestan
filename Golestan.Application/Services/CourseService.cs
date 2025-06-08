namespace Golestan.Application.Services;

using DTOs.Course;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;


public class CourseService : ICourseService {

    private readonly AppDbContext _context;

    public CourseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CourseDto>> GetCoursesDto()
    {
        try{
            var dto = await _context.Courses.Select(c => new CourseDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ExamTime = c.ExamTime,
                    FacultyId = c.FacultyId,
                    Unit = c.Unit,
                    FacultyName = c.Faculty.MajorName,
                    SectionsCount = c.Sections.Count,
                })
                .Take(10)
                .ToListAsync();

            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

}
