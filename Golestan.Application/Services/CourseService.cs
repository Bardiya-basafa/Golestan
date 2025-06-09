namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.Course;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;


public class CourseService : ICourseService {

    private readonly AppDbContext _context;

    private readonly IFacultyService _facultyService;

    public CourseService(AppDbContext context, IFacultyService facultyService)
    {
        _context = context;
        _facultyService = facultyService;
    }

    public async Task<CourseManagementDto> GetFacultyCourses(int facultyId)
    {
        try{
            var dto = new CourseManagementDto();

            dto.Courses = await _context.Courses.Select(c => new CourseDetailsDto()
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

            var facultyDetails = await _facultyService.GetDetailsFacultyById(facultyId);


            dto.FacultyId = facultyDetails.Id;
            dto.FacultyName = facultyDetails.MajorName;

            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<Dictionary<int, string>>? GetCourseInstructors(int courseId)
    {
        try{
            var instructorOptions = await _context.Instructors
                .Where(i => i.Courses.Any(c => c.Id == courseId))
                .Select(i => new { i.Id, i.FullName })
                .Distinct()
                .ToDictionaryAsync(c => c.Id, c => c.FullName);

            return instructorOptions;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<Result> AddCourse(AddCourseDto dto)
    {
        var finalResult = new Result();

        try{
            var course = new Course()
            {
                Name = dto.Name,
                Description = dto.Description,
                FacultyId = dto.FacultyId,
                Unit = dto.Unit,
                ExamTime = dto.ExamTime,
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            finalResult.Succeeded = true;
            finalResult.Message = "Course added";
        }
        catch (Exception e){
            Console.WriteLine(e);
            finalResult.Message = "Failed to create course";
        }

        return finalResult;
    }

}
