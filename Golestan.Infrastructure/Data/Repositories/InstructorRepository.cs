namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.ExamResult;
using Application.DTOs.Faculty;
using Application.DTOs.Instructor;
using Application.DTOs.Score;
using Application.DTOs.Student;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class InstructorRepository(AppDbContext context) : IInstructorRepository {

    public async Task<List<InstructorDto>> GetFacultyInstructors()
    {
        return await context.Instructors
            .Select(i => new InstructorDto()
            {
                Id = i.Id,
                AppUser = new AppUser()
                {
                    Id = i.AppUserId,
                    Email = i.AppUser.Email,
                },
                Faculty = new FacultyDto()
                {
                    Id = i.FacultyId,
                    MajorName = i.Faculty.MajorName,
                },
                FullName = $"{i.AppUser.FirstName} {i.AppUser.LastName}",
                HireDate = i.HireDate,
                Salary = i.Salary,
            })
            .OrderBy(i => i.Id)
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<StudentDto>> GetInstructorStudentsOfSection(int sectionId)
    {
        return await context.Sections
            .Where(s => s.Id == sectionId)
            .SelectMany(s => s.Students)
            .Select(s => new StudentDto()
            {
                Id = s.Id,
                FullName = s.FullName,
                StudentNumber = s.StudentNumber,
            })
            .ToListAsync();
    }

    public async Task<List<ExamResultDto>> GetExamResultsOfSection(int sectionId)
    {
        return await context.ExamResults
            .Where(r => r.SectionId == sectionId)
            .Select(r => new ExamResultDto()
            {
                Student = new StudentDto()
                {
                    Id = r.StudentId,
                    FullName = r.Student.FullName,
                    StudentNumber = r.Student.StudentNumber,
                },
                Score = r.Score,
                Objection = r.Objection,
                Description = r.Description,
            })
            .ToListAsync();
    }

    public async Task<Result> SubmitExamResult(ExamResult examResult)
    {
        var updateExamResult = await context.ExamResults
            .Where(e => e.StudentId == examResult.StudentId && e.SectionId == examResult.SectionId)
            .FirstOrDefaultAsync() ?? throw new Exception($"No exam result found for student  id:{examResult.StudentId}");


        examResult.Score = examResult.Score;
        examResult.Description = examResult.Description;

        return new Result()
        {
            Message = "Exam result has been saved",
            Succeeded = true
        };
    }

    public async Task<Result> RemoveCourseInstructor(int instructorId, int courseId)
    {
        var course = await context.Courses
            .Where(c => c.Id == courseId)
            .Include(c => c.Instructors)
            .FirstOrDefaultAsync() ?? throw new Exception($"No course found for {courseId}");


        var instructor = await context.Instructors
            .Include(i => i.Sections)
            .FirstOrDefaultAsync(i => i.Id == instructorId) ?? throw new Exception($"No instructor found for {instructorId}");


        var sections = await context.Sections
            .Where(s => s.InstructorId == instructorId && s.CourseId == courseId)
            .ToListAsync();

        context.Sections.RemoveRange(sections);
        course.Instructors.Remove(instructor);
        context.Courses.Update(course);
        await context.SaveChangesAsync();

        return new Result()
        {
            Message = "Course has been removed",
            Succeeded = true
        };
    }

    public async Task<Result> RemoveInstructor(int instructorId)
    {
        var instructor = await context.Instructors.Where(i => i.Id == instructorId)
            .Include(i => i.Sections)
            .FirstOrDefaultAsync() ?? throw new Exception($"No instructor found for {instructorId}");


        context.Instructors.Remove(instructor);
        await context.SaveChangesAsync();

        return new Result()
        {
            Message = "Instructor has been removed",
            Succeeded = true
        };
    }

}
