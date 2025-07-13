namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.Classroom;
using Application.DTOs.Course;
using Application.DTOs.Exam;
using Application.DTOs.Faculty;
using Application.DTOs.Instructor;
using Application.DTOs.Section;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class CourseRepository(AppDbContext context) : ICourseRepository {

    public async Task<Course> GetCourseEntityById(int courseId)
    {
        return await context.Courses.FindAsync(courseId) ?? throw new KeyNotFoundException($"Course with id {courseId} not found");
    }

    public async Task<List<CourseDto>> GetFacultyCourses(int facultyId)
    {
        return await context.Courses
            .AsNoTracking()
            .Where(c => c.FacultyId == facultyId)
            .Select(c => new CourseDto()
            {
                Id = c.Id,
                CourseName = c.CourseName,
                Unit = c.Unit,
                Description = c.Description,
            })
            .Take(10)
            .ToListAsync();
    }

    public async Task<CourseDto> GetCourseDtoById(int courseId)
    {
        return await context.Courses
            .AsNoTracking()
            .Where(c => c.Id == courseId)
            .Select(c => new CourseDto()
            {
                Id = c.Id,
                Unit = c.Unit,
                CourseName = c.CourseName,
                PrerequisiteCourses = c.PrerequisiteCourses,
                Faculty = new FacultyDto()
                {
                    Id = c.FacultyId,
                    MajorName = c.Faculty.MajorName,
                },
                Sections = c.Sections.Select(s => new SectionDto()
                    {
                        Id = s.Id,
                        Classroom = new ClassroomDto()
                        {
                            ClassroomNumber = s.Classroom.ClassNumber
                        },
                        Instructor = new InstructorDto()
                        {
                            Id = s.InstructorId,
                            FullName = s.Instructor.FullName,
                        },
                        TimeSlot = s.TimeSlot,
                        DayOfWeek = s.DayOfWeek,
                    })
                    .ToList(),
                Instructors = c.Instructors.Select(i => new InstructorDto()
                {
                    Id = i.Id,
                    FullName = i.FullName,
                    InstructorNumber = i.InstructorNumber,
                }).ToList()
            })
            .FirstOrDefaultAsync() ?? throw new Exception("Course not found");
    }

    public async Task<List<CourseDto>> GetPrerequisiteCourses(List<int> prerequisiteCourseIds)
    {
        return await context.Courses
            .AsNoTracking()
            .Where(c => prerequisiteCourseIds.Contains(c.Id))
            .Select(c => new CourseDto()
            {
                Id = c.Id,
                CourseName = c.CourseName,
                Unit = c.Unit,
            })
            .ToListAsync();
    }

    public async Task<Dictionary<int, string>?> GetCourseInstructors(int courseId)
    {
        return await context.Instructors
            .AsNoTracking()
            .Where(i => i.Courses.Any(c => c.Id == courseId))
            .Select(i => new { i.Id, i.FullName })
            .Distinct()
            .ToDictionaryAsync(c => c.Id, c => c.FullName);
    }

    public async Task<Dictionary<int, string>?> GetExamClassrooms(int courseId)
    {
        var facultyId = await context.Courses
            .AsNoTracking()
            .Where(c => c.Id == courseId)
            .Select(c => c.FacultyId)
            .FirstOrDefaultAsync();

        return await context.Classrooms
            .AsNoTracking()
            .Where(c => c.FacultyId == facultyId)
            .Select(c => new { c.Id, c.ClassNumber })
            .Distinct()
            .ToDictionaryAsync(c => c.Id, c => c.ClassNumber);
    }

    public async Task<List<CourseDto>> GetAvailableCoursesForPrerequisite(int courseId)
    {
        var course = await context.Courses
            .AsNoTracking()
            .Where(c => c.Id == courseId)
            .Select(c => new
            {
                PrerequisiteCourseIds = c.PrerequisiteCourses
            })
            .FirstOrDefaultAsync() ?? throw new Exception("Course not found");

        var prerequisiteCourseIds = course.PrerequisiteCourseIds;

        return await context.Courses
            .AsNoTracking()
            .Where(c => c.Id != courseId && !prerequisiteCourseIds.Contains(c.Id) && !c.PrerequisiteCourses.Contains(courseId))
            .Select(c => new CourseDto()
            {
                Id = c.Id,
                CourseName = c.CourseName,
                Description = c.Description,
                Unit = c.Unit,
                Faculty = new FacultyDto()
                {
                    Id = c.FacultyId,
                    MajorName = c.Faculty.MajorName,
                },
                Sections = c.Sections.Select(s => new SectionDto()
                {
                    Id = s.Id,
                    TimeSlot = s.TimeSlot,
                    DayOfWeek = s.DayOfWeek,
                }).ToList(),
            })
            .ToListAsync();
    }

    public async Task<List<Course>> GetAvailableCoursesForStudent(int studentId)
    {
        var studentPassedCourses = await context.Students
            .AsNoTracking()
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.PassedCourses)
            .ToListAsync();

        return await context.Courses
            .AsNoTracking()
            .Where(c => !studentPassedCourses.Contains(c))
            .Where(c => c.PrerequisiteCourses.All(pc => studentPassedCourses.Select(passed => passed.Id).Contains(pc)))
            .ToListAsync();
    }

    public async Task<CourseInstructorDto> GetAvailableInstructorsForCourse(int facultyId, int courseId)
    {
        return new CourseInstructorDto()
        {
            Instructors = await context.Instructors
                .AsNoTracking()
                .Where(i => i.FacultyId == facultyId && i.Courses.All(c => c.Id != courseId))
                .Select(i => new InstructorDto()
                {
                    Id = i.Id,
                    FullName = i.FullName,
                })
                .ToListAsync(),
            CourseId = courseId,
            CourseName = await context.Courses
                .AsNoTracking()
                .Where(c => c.Id == courseId)
                .Select(c => c.CourseName)
                .FirstOrDefaultAsync() ?? throw new Exception("Course not found")
        };
    }

    public async Task<ExamDto?> GetCourseExam(int courseId)
    {
        return await context.Exams
            .AsNoTracking()
            .Where(e => e.CourseId == courseId && !e.Term.IsClosed)
            .Select(e => new ExamDto()
            {
                Id = e.Id,
                ExamDateTime = e.ExamDateTime,
                TimeSlot = e.TimeSlot,
                Classroom = new ClassroomDto()
                {
                    ClassroomNumber = e.Classroom.ClassNumber
                }
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Result> ApplyInstructorToCourse(int courseId, int instructorId)
    {
        var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == courseId) ?? throw new Exception("Course not found");
        var instructor = await context.Instructors.FirstOrDefaultAsync(i => i.Id == instructorId) ?? throw new Exception("Instructor not found");


        course.Instructors.Add(instructor);
        context.Courses.Update(course);
        await context.SaveChangesAsync();

        return new Result()
        {
            Message = "Instructor successfully applied",
            Succeeded = true
        };
    }

    public async Task<Result> AddCourse(Course course)
    {
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        return new Result()
        {
            Message = "Course successfully added",
            Succeeded = true
        };
    }

    public async Task<Result> RemoveCourse(int courseId)
    {
        var course = await context.Courses
            .Where(c => c.Id == courseId)
            .Include(c => c.Sections)
            .FirstOrDefaultAsync() ?? throw new Exception("Course not found");


        context.Courses.Remove(course);
        await context.SaveChangesAsync();

        return new Result()
        {
            Message = "Course successfully removed",
            Succeeded = true
        };
    }

    public async Task<Result> AddPrerequisiteToCourse(int courseId, int prerequisiteId)
    {
        var course = await context.Courses
            .Where(c => c.Id == courseId)
            .FirstOrDefaultAsync() ?? throw new Exception("Course not found");

        if (course.PrerequisiteCourses.Contains(prerequisiteId)){
            return new Result() { Message = $"Course with id {courseId} already prerequisite" };
        }

        course.PrerequisiteCourses.Add(prerequisiteId);
        context.Courses.Update(course);
        await context.SaveChangesAsync();

        return new Result()
        {
            Message = "Prerequisite successfully added",
            Succeeded = true
        };
    }

    public async Task<bool> CourseNameExist(string courseName, int facultyId)
    {
        return await context.Courses.AsNoTracking().AnyAsync(c => c.CourseName == courseName && c.FacultyId == facultyId);
    }

    public async Task<Result> SetExam(Exam exam)
    {
        context.Exams.Add(exam);
        await context.SaveChangesAsync();

        return new Result() { Message = "Exam successfully set", Succeeded = true };
    }

    public async Task<Result> RemovePrerequisiteFromCourse(Course course, int prerequisiteId)
    {
        var result = new Result();

        if (!course.PrerequisiteCourses.Contains(prerequisiteId)){
            result.Message = $"Course prerequisite with id {prerequisiteId} doesn't exist";

            return result;
        }

        course.PrerequisiteCourses.Remove(prerequisiteId);
        context.Courses.Update(course);
        await context.SaveChangesAsync();

        return new Result()
        {
            Message = "Prerequisite successfully removed",
            Succeeded = true
        };
    }

    public async Task<bool> IsExamExistInClass(DateTime examDate, TimeSlot timeSlot, int classroomId)
    {
        return await context.Exams
            .AsNoTracking()
            .Where(e => e.TimeSlot == timeSlot && e.ExamDateTime == examDate)
            .Where(e => e.ClassroomId == classroomId)
            .AnyAsync();
    }

}
