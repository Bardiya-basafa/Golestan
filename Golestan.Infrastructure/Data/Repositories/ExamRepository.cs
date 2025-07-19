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
            .Include(e => e.Course.Exam)
            .FirstOrDefaultAsync() ?? throw new Exception($"No exam result found for student  id:{examResult.StudentId}");

        if (DateTime.UtcNow <= updateExamResult.Course.Exam.ExamDateTime + TimeSpan.FromDays(1)){
            return new Result()
            {
                Message = "This course exam date not reached yet",
            };
        }


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
                        Id = r.InstructorId,
                        FullName = r.Instructor.FullName,
                        InstructorNumber = r.Instructor.InstructorNumber,
                    },
                    Course = new CourseDto()
                    {
                        CourseName = r.Course.CourseName,
                        Exam = new ExamDto()
                        {
                            ExamDateTime = r.Course.Exam.ExamDateTime,
                            Classroom = new ClassroomDto()
                            {
                                ClassroomNumber = r.Course.Exam.Classroom.ClassNumber
                            }
                        }
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

    public async Task<List<ExamResultDto>> GetTermFinalResults(int termId, int instructorId)
    {
        return await context.ExamResults
            .AsNoTracking()
            .Where(e => e.InstructorId == instructorId && e.TermId == termId)
            .Select(e => new ExamResultDto()
            {
                Student = new StudentDto()
                {
                    Id = e.StudentId,
                    FullName = e.Student.FullName,
                    StudentNumber = e.Student.StudentNumber,
                },
                Course = new CourseDto()
                {
                    CourseName = e.Course.CourseName,
                    Exam = new ExamDto()
                    {
                        ExamDateTime = e.Course.Exam.ExamDateTime,
                        TimeSlot = e.Course.Exam.TimeSlot,
                        Classroom = new ClassroomDto()
                        {
                            ClassroomNumber = e.Course.Exam.Classroom.ClassNumber
                        }
                    }
                },
                Score = e.Score,
                Objection = e.Objection,
                Description = e.Description,
            })
            .OrderByDescending(e => e.Course.CourseName)
            .ToListAsync();
    }

    public async Task<Result> SubmitObjection(ExamResult examResult, string objection)
    {
        if (examResult.Score == -1){
            return new Result()
            {
                Message = "Exam score not set yet",
                Succeeded = false,
            };
        }

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
