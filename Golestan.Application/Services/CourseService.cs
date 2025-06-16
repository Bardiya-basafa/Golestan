namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.Course;
using DTOs.Instructor;
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

    public async Task<CourseActionsDto> GetCourseActionsDto(int courseId)
    {
        try{
            var dto = new CourseActionsDto();

            var course = await _context.Courses
                .Where(c => c.Id == courseId)
                .Include(c => c.Faculty)
                .Include(c => c.Instructors)
                .FirstOrDefaultAsync();

            if (course == null){
                throw new Exception($"Course with id {courseId} not found");
            }

            dto.CourseId = course.Id;
            dto.CourseName = course.Name;
            dto.Unit = course.Unit;
            dto.ExamTime = course.ExamTime;
            dto.FacultyName = course.Faculty.MajorName;
            dto.FacultyId = course.FacultyId;

            dto.Instructors = course.Instructors.Select(i => new InstructorDetailsDto()
            {
                Id = i.Id,
                FullName = i.FullName,
            }).ToList();

            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<ApplyNewInstructorDto> GetAllFacultyInstructors(int facultyId, int courseId)
    {
        try{
            var dto = new ApplyNewInstructorDto();

            dto.Instructors = await _context.Instructors
                .Where(i => i.FacultyId == facultyId && i.Courses.All(c => c.Id != courseId))
                .Select(i => new InstructorDetailsDto()
                {
                    Id = i.Id,
                    FullName = i.FullName,
                })
                .ToListAsync();

            dto.CourseId = courseId;
            dto.CourseName = _context.Courses.FindAsync(courseId).Result.Name;

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

    public async Task<Result> ApplyNewInstructorToCourse(ApplyNewInstructorDto dto)
    {
        var finalResult = new Result();

        try{
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == dto.CourseId);
            var instructor = await _context.Instructors.FirstOrDefaultAsync(i => i.Id == dto.InstructorId);

            if (course == null || instructor == null){
                finalResult.Message = $"Course with id {dto.CourseId} not found";

                return finalResult;
            }

            course.Instructors.Add(instructor);
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            finalResult.Succeeded = true;
            finalResult.Message = "Instructor applied";

            return finalResult;
        }
        catch (Exception e){
            Console.WriteLine(e);

            finalResult.Message = e.Message;

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

    public async Task<Result> RemoveCourse(int courseId)
    {
        var result = new Result();

        try{
            var course = await _context.Courses
                .Where(c => c.Id == courseId)
                .Include(c => c.Sections)
                .FirstOrDefaultAsync();

            if (course == null){
                result.Message = $"Course with id {courseId} not found";

                return result;
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            result.Succeeded = true;
            result.Message = "Course removed";

            return result;
        }
        catch (Exception e){
            Console.WriteLine(e);

            result.Message = e.Message;
        }

        return result;
    }

}
