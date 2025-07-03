namespace Golestan.Application.Services;

using Domain.Entities;
using Domain.Enums;
using DTOs.Course;
using DTOs.Instructor;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;


public class CourseService : ICourseService {

    private readonly AppDbContext _context;

    private readonly IFacultyService _facultyService;

    private readonly ISectionService _sectionService;

    public CourseService(AppDbContext context, IFacultyService facultyService, ISectionService sectionService)
    {
        _context = context;
        _facultyService = facultyService;
        _sectionService = sectionService;
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

                    // ExamTime = c.ExamTime,
                    FacultyId = c.FacultyId,
                    Unit = c.Unit,
                    FacultyName = c.Faculty.Major,
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
                .Include(c => c.Exam)
                .FirstOrDefaultAsync();

            if (course == null){
                throw new Exception($"Course with id {courseId} not found");
            }

            dto.CourseId = course.Id;
            dto.CourseName = course.Name;
            dto.Unit = course.Unit;
            dto.ExamDateTime = course.Exam.ExamDateTime;
            dto.ExamTimeSlot = course.Exam.TimeSlot;
            dto.FacultyName = course.Faculty.Major;
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

    public async Task<List<CourseDetailsDto>> GetAvailableCoursesForPrerequisite(int courseId)
    {
        var course = await _context.Courses
            .Include(c => c.PrerequisiteCourses)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null){
            throw new ArgumentException("Course not found");
        }

        var model = await _context.Courses
            .Where(c => c.Id != courseId && course.PrerequisiteCourses.All(id => id != c.Id))
            .Select(c => new CourseDetailsDto()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                FacultyId = c.FacultyId,
                Unit = c.Unit,
                FacultyName = c.Faculty.Major,
                SectionsCount = c.Sections.Count,
                ExamTime = c.Exam.ExamDateTime,
            })
            .ToListAsync();

        return model;
    }


    public async Task<List<Course>> GetAvailableCoursesForStudent(int studentId)
    {
        var studentPassedCourses = await _context.Students
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.PassedCourses)
            .ToListAsync();

        var availableCourses = await _context.Courses
            .Where(c => !studentPassedCourses.Contains(c))
            .Where(c => c.PrerequisiteCourses.All(pc => studentPassedCourses.Select(passed => passed.Id).Contains(pc)))
            .ToListAsync();

        return availableCourses;
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
            };

            if (await IsExamExistInClass(dto)){
                finalResult.Message = "There is already an exam in that time";

                return finalResult;
            }

            var exam = new Exam()
            {
                ClassroomId = dto.ExamClassroomId,
                ExamDateTime = dto.ExamDateTime,
                TimeSlot = GetTimeSlot(dto.ExamTimeSlotId),
            };

            course.Exam = exam;

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

    public async Task<Result> AddPrerequisiteToCourse(int courseId, int prerequisiteCourseId)
    {
        var result = new Result();

        try{
            var course = await _context.Courses
                .Where(c => c.Id == courseId)
                .Include(c => c.PrerequisiteCourses)
                .FirstOrDefaultAsync();

            if (course.PrerequisiteCourses.Contains(prerequisiteCourseId)){
                result.Message = $"Course with id {courseId} already prerequisite";

                return result;
            }

            course.PrerequisiteCourses.Add(prerequisiteCourseId);
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            result.Succeeded = true;
            result.Message = "Prerequisite course added";
        }
        catch (Exception e){
            Console.WriteLine(e);
            result.Message = e.Message;
        }

        return result;
    }

    private async Task<bool> IsExamExistInClass(AddCourseDto dto)
    {
        var timeSlot = GetTimeSlot(dto.ExamTimeSlotId);
        var examDateTime = dto.ExamDateTime;

        var isExamExistInClass = await _context.Exams
            .Where(e => e.TimeSlot == timeSlot && e.ExamDateTime == examDateTime)
            .Where(e => e.ClassroomId == dto.ExamClassroomId)
            .AnyAsync();

        return isExamExistInClass;
    }

    private TimeSlot GetTimeSlot(int timeSlotId)
    {
        switch (timeSlotId){
            case 1: return TimeSlot.First; break;

            case 2: return TimeSlot.Second; break;

            case 3: return TimeSlot.Third; break;

            case 4: return TimeSlot.Fourth; break;

            case 5: return TimeSlot.Fifth; break;

            case 6: return TimeSlot.Sixth; break;

            default: return TimeSlot.First;
        }
    }

}
