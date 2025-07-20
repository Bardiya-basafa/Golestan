namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.Classroom;
using Application.DTOs.Course;
using Application.DTOs.Exam;
using Application.DTOs.ExamResult;
using Application.DTOs.Faculty;
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


public class StudentRepository(AppDbContext context, IUserRepository userRepository) : IStudentRepository {

    public async Task<StudentDto> GetStudentInfo(string studentId)
    {
        var student = await context.Students
            .AsNoTracking()
            .Where(s => s.AppUserId == studentId)
            .Select(s => new StudentDto()
            {
                Id = s.Id,
                FullName = s.FullName,
                StudentNumber = s.StudentNumber,
                AppUser = new AppUser()
                {
                    Id = s.AppUserId,
                },
                Email = s.AppUser.UserName,
                Faculty = new FacultyDto()
                {
                    MajorName = s.Faculty.MajorName,
                },
                Sections = s.Sections.Select(sec => new SectionDto()
                {
                    Course = new CourseDto()
                    {
                        Unit = sec.Course.Unit,
                    }
                }).ToList(),
                Terms = s.Terms.Select(term => new TermDto()
                    {
                        Id = term.Id,
                    })
                    .ToList(),
            }).FirstOrDefaultAsync() ?? throw new Exception($"student with id {studentId} not found");

        var gpa = await GetStudentTotalGpa(student.AppUser.Id);
        student.Gpa = gpa;

        return student;
    }

    public async Task<Student> GetStudentEntityById(string studentId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.AppUserId == studentId)
            .Include(s => s.Sections).ThenInclude(s => s.Course)
            .FirstOrDefaultAsync() ?? throw new Exception($"Student with id {studentId} not found");
    }

    public async Task<StudentDto> GetStudentDtoById(string studentId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.AppUserId == studentId)
            .Select(s => new StudentDto()
            {
                Id = s.Id,
                FullName = s.FullName,
                StudentNumber = s.StudentNumber,
                Email = s.AppUser.UserName,
                Gpa = GetStudentTotalGpa(studentId).GetAwaiter().GetResult(),
                Faculty = new FacultyDto()
                {
                    MajorName = s.Faculty.MajorName,
                },
                Sections = s.Sections.Select(sec => new SectionDto()
                {
                    Id = sec.Id,
                    Classroom = new ClassroomDto()
                    {
                        ClassroomNumber = sec.Classroom.ClassNumber,
                        Capacity = sec.Classroom.Capacity,
                        Id = sec.ClassroomId,
                        Faculty = new FacultyDto()
                        {
                            BuildingName = sec.Classroom.Faculty.BuildingName,
                        }
                    },
                    Course = new CourseDto()
                    {
                        CourseName = sec.Course.CourseName,
                        Unit = sec.Course.Unit,
                        Exam = new ExamDto()
                        {
                            ExamDateTime = sec.Course.Exam.ExamDateTime,
                            TimeSlot = sec.Course.Exam.TimeSlot,
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

    public async Task<List<TermDto>> GetAllStudentTerms(string studentId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.AppUserId == studentId)
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

    public async Task<List<ExamResultDto>> GetAllTermExamResults(string studentId, int termId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.AppUserId == studentId)
            .SelectMany(s => s.ExamResults)
            .Where(e => e.TermId == termId)
            .Select(e => new ExamResultDto()
            {
                Score = e.Score,
                Description = e.Description,
            })
            .ToListAsync();
    }

    public async Task<List<ExamResultDto>> GetAllExamResults(string studentId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.AppUserId == studentId)
            .SelectMany(s => s.ExamResults)
            .Select(e => new ExamResultDto()
            {
                Score = e.Score,
            })
            .ToListAsync();
    }

    public async Task<ExamResult> GetExamResultForObjection(ObjectionDto objection, string userId)
    {
        return await context.ExamResults
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == objection.ExamResultId && e.Student.AppUserId == userId) ?? throw new Exception($"Exam result with id {objection.ExamResultId} not found");
    }


    public async Task<decimal> GetStudentTotalGpa(string studentId)
    {
        var scores = await context.ExamResults
            .AsNoTracking()
            .Where(e => e.Student.AppUserId == studentId && e.Score != -1)
            .Select(e => e.Score)
            .ToListAsync();

        if (scores.Count == 0){
            return -1;
        }

        return scores.Sum() / scores.Count;
    }

    public async Task<Result> RemoveStudent(int studentId)
    {
        var student = await context.Students
            .Where(s => s.Id == studentId)
            .Include(s => s.ExamResults)
            .FirstOrDefaultAsync() ?? throw new Exception($"Student with id {studentId} not found");

        if (student.ExamResults?.Count != 0){
            context.ExamResults.RemoveRange(student.ExamResults);
        }

        if (!await userRepository.DeleteUser(student.AppUserId)){
            return new Result()
            {
                Message = "Something went wrong.",
            };
        }

        context.Remove(student);
        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Student removed successfully.",
        };
    }

    public async Task<Result> UpdateStudent(Student student)
    {
        context.Update(student);
        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Student updated successfully.",
        };
    }

}
