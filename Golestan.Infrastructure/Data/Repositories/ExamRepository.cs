namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.Classroom;
using Application.DTOs.Course;
using Application.DTOs.Exam;
using Application.DTOs.ExamResult;
using Application.DTOs.Instructor;
using Application.DTOs.Section;
using Application.DTOs.Student;
using Application.DTOs.Term;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class ExamRepository(AppDbContext context) : IExamRepository {

    public async Task<Result> SubmitExamResult(ExamResult examResult)
    {
        var updateExamResult = await context.ExamResults
            .Where(e => e.StudentId == examResult.StudentId && e.SectionId == examResult.SectionId)
            .FirstOrDefaultAsync() ?? throw new Exception($"No exam result found for student  id:{examResult.StudentId}");


        updateExamResult.Score = examResult.Score;
        updateExamResult.Description = examResult.Description;
        context.Update(updateExamResult);
        await context.SaveChangesAsync();

        return new Result()
        {
            Message = "Exam result has been saved",
            Succeeded = true
        };
    }

    public async Task<List<ExamResultDto>> GetSectionExamResults(int sectionId)
    {
        return await context.ExamResults
            .AsNoTracking()
            .Where(e => e.SectionId == sectionId)
            .Select(r => new ExamResultDto()
            {
                Student = new StudentDto()
                {
                    Id = r.StudentId,
                    FullName = r.Student.FullName,
                    StudentNumber = r.Student.StudentNumber,
                },
                Section = new SectionDto()
                {
                    Id = r.SectionId,
                },
                Score = r.Score,
                Objection = r.Objection,
                Description = r.Description,
            })
            .ToListAsync();
    }

    public async Task<List<ExamResultDto>> GetTermExamResults(int termId, int studentId)
    {
        return await context.ExamResults
            .AsNoTracking()
            .Where(e => e.StudentId == studentId && e.TermId == termId)
            .Select(r => new ExamResultDto()
            {
                Id = r.Id,
                Section = new SectionDto()
                {
                    Id = r.SectionId,
                    Instructor = new InstructorDto()
                    {
                        FullName = r.Instructor.FullName,
                        InstructorNumber = r.Instructor.InstructorNumber,
                    },
                    Course = new CourseDto()
                    {
                        CourseName = r.Course.CourseName,
                    },
                    TimeSlot = r.Section.TimeSlot,
                    DayOfWeek = r.Section.DayOfWeek,
                },
                Score = r.Score,
                Objection = r.Objection,
                Description = r.Description,
            })
            .ToListAsync();
    }

    public async Task<Result> SubmitObjection(ExamResult examResult, string objection)
    {
        examResult.Objection = objection;
        context.Update(examResult);
        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Objection submitted successfully.",
        };
    }

    public async Task<ExamDto> GetExamInfo(int sectionId)
    {
        return await context.ExamResults
            .AsNoTracking()
            .Where(r => r.SectionId == sectionId)
            .Select(r => r.Course.Exam)
            .Select(e => new ExamDto()
            {
                ExamDateTime = e.ExamDateTime,
                TimeSlot = e.TimeSlot,
                Classroom = new ClassroomDto()
                {
                    ClassroomNumber = e.Classroom.ClassNumber,
                },
                Course = new CourseDto()
                {
                    CourseName = e.Course.CourseName,
                },
                Term = new TermDto()
                {
                    ExamsStartTime = e.Term.ExamsStartTime,
                    ExamsEndTime = e.Term.ExamsEndTime,
                    TermIdentifier = e.Term.TermIdentifier,
                }
            }).FirstOrDefaultAsync() ?? throw new Exception($"No exam result found for section {sectionId}");
    }

}
