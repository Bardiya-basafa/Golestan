namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.Classroom;
using Application.DTOs.Course;
using Application.DTOs.Exam;
using Application.DTOs.ExamResult;
using Application.DTOs.Instructor;
using Application.DTOs.Objection;
using Application.DTOs.Section;
using Application.DTOs.Student;
using Application.DTOs.Term;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class StudentRepository(AppDbContext context) : IStudentRepository {

    public async Task<StudentDto> GetStudentUserApp(string studentId)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id == studentId)
            .Select(u => new StudentDto()
            {
                Id = u.StudentId,
                AppUser = u,
                FullName = u.StudentProfile.FullName,
            })
            .FirstOrDefaultAsync() ?? throw new Exception($"student with id {studentId} not found");
    }

    public async Task<Student> GetStudentEntityById(int studentId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.Id == studentId)
            .Include(s => s.Sections).ThenInclude(s => s.Course)
            .FirstOrDefaultAsync() ?? throw new Exception($"Student with id {studentId} not found");
    }

    public async Task<StudentDto> GetStudentDtoById(int studentId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.Id == studentId)
            .Select(s => new StudentDto()
            {
                Id = s.Id,
                FullName = s.FullName,
                StudentNumber = s.StudentNumber,
                Sections = s.Sections.Select(sec => new SectionDto()
                {
                    Id = sec.Id,
                    Classroom = new ClassroomDto()
                    {
                        ClassroomNumber = sec.Classroom.ClassNumber,
                        Capacity = sec.Classroom.Capacity,
                        Id = sec.ClassroomId
                    },
                    Course = new CourseDto()
                    {
                        CourseName = sec.Course.CourseName,
                        Unit = sec.Course.Unit,
                        Exam = new ExamDto()
                        {
                            ExamDateTime = sec.Course.Exam.ExamDateTime
                        }
                    },
                    Instructor = new InstructorDto()
                    {
                        FullName = sec.Instructor.FullName,
                    },
                    DayOfWeek = sec.DayOfWeek,
                    TimeSlot = sec.TimeSlot,
                }).ToList()
            })
            .FirstOrDefaultAsync() ?? throw new Exception($"Student with id {studentId} not found");
    }

    public async Task<List<StudentDto>> GetFacultyStudents(int facultyId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.FacultyId == facultyId)
            .Select(s => new StudentDto()
            {
                Id = s.Id,
                AppUser = s.AppUser,
                Email = s.AppUser.Email,
                FullName = s.FullName,
                StudentNumber = s.StudentNumber,
            })
            .ToListAsync();
    }

    public async Task<List<TermDto>> GetAllStudentTerms(int studentId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.Terms)
            .Select(t => new TermDto()
            {
                Id = t.Id,
                Year = t.Year,
                TermIdentifier = t.TermIdentifier,
                SelectionEndTime = t.SectionSelectionEndTime,
                SelectionStartTime = t.SectionSelectionStartTime,
                ExamsEndTime = t.ExamsEndTime,
                ExamsStartTime = t.ExamsStartTime,
            })
            .ToListAsync();
    }

    public async Task<List<ExamResultDto>> GetAllTermExamResults(int studentId, int termId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.ExamResults)
            .Where(e => e.TermId == termId)
            .Select(e => new ExamResultDto()
            {
                Score = e.Score,
                Description = e.Description,
            })
            .ToListAsync();
    }

    public async Task<ExamResult> GetExamResultForObjection(ObjectionDto objection)
    {
        return await context.Students
            .Where(s => s.Id == objection.StudentId)
            .SelectMany(s => s.ExamResults)
            .Where(e => e.Id == objection.ExamResultId)
            .FirstOrDefaultAsync() ?? throw new Exception($"Student with id {objection.StudentId} not found");
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

}
