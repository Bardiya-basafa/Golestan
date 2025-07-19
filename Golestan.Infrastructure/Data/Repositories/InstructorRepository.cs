namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.Classroom;
using Application.DTOs.Course;
using Application.DTOs.Exam;
using Application.DTOs.ExamResult;
using Application.DTOs.Faculty;
using Application.DTOs.Instructor;
using Application.DTOs.Score;
using Application.DTOs.Section;
using Application.DTOs.Student;
using Application.DTOs.Term;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class InstructorRepository(AppDbContext context, IUserRepository userRepository) : IInstructorRepository {

    public async Task<InstructorDto> GetInstructorAppUser(string instructorId)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id == instructorId)
            .Select(i => i.InstructorProfile)
            .Select(i => new InstructorDto()
            {
                Id = i.Id,
                FullName = i.FullName,
                Salary = i.Salary,
                HireDate = i.HireDate,

                // InstructorNumber = i.InstructorNumber,
                Sections = i.Sections.Select(s => new SectionDto()
                {
                    Id = s.Id,
                }).ToList(),
            })
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"User with id {instructorId} not found");
    }

    public async Task<InstructorDto> GetInstructorInfo(int instructorId)
    {
        return await context.Instructors
            .AsNoTracking()
            .Where(u => u.Id == instructorId)
            .Select(i => new InstructorDto()
            {
                Id = i.Id,
                FullName = i.FullName,
                Salary = i.Salary,
                HireDate = i.HireDate,

                // InstructorNumber = i.InstructorNumber,
                Sections = i.Sections.Select(s => new SectionDto()
                {
                    Id = s.Id,
                }).ToList(),
            })
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"User with id {instructorId} not found");
    }

    public async Task<List<TermDto>> GetAllInstructorTerms(int instructorId)
    {
        return await context.Instructors
            .AsNoTracking()
            .Where(s => s.Id == instructorId)
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

    public async Task<List<CourseDto>> GetCourses(int instructorId)
    {
        return await context.Instructors
            .AsNoTracking()
            .Where(c => c.Id == instructorId)
            .SelectMany(i => i.Courses)
            .Select(c => new CourseDto()
            {
                Id = c.Id,
                CourseName = c.CourseName,
                Unit = c.Unit,
                Description = c.Description,
            })
            .ToListAsync();
    }


    public async Task<InstructorDto> GetInstructorDtoById(int instructorId)
    {
        return await context.Instructors
            .AsNoTracking()
            .Where(i => i.Id == instructorId)
            .Select(i => new InstructorDto()
            {
                Id = i.Id,
                FullName = i.FullName,
                AppUser = i.AppUser,
                Faculty = new FacultyDto()
                {
                    Id = i.FacultyId,
                    MajorName = i.Faculty.MajorName,
                },
                Sections = i.Sections.Select(s => new SectionDto()
                {
                    Id = s.Id,
                    DayOfWeek = s.DayOfWeek,
                    TimeSlot = s.TimeSlot,
                    Classroom = new ClassroomDto()
                    {
                        ClassroomNumber = s.Classroom.ClassNumber,
                        Faculty = new FacultyDto()
                        {
                            BuildingName = s.Classroom.Faculty.BuildingName,
                        }
                    },
                    Course = new CourseDto()
                    {
                        Id = s.CourseId,
                        Unit = s.Course.Unit,
                        CourseName = s.Course.CourseName,
                        Exam = new ExamDto()
                        {
                            ExamDateTime = s.Course.Exam.ExamDateTime,
                            TimeSlot = s.Course.Exam.TimeSlot,
                        }
                    },
                    Students = s.Students.Select(st => new StudentDto()
                    {
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync() ?? throw new NullReferenceException($"Instructor not found with id: {instructorId}");
    }

    public async Task<List<InstructorDto>> GetFacultyInstructors()
    {
        return await context.Instructors
            .AsNoTracking()
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
            .AsNoTracking()
            .Where(s => s.Id == sectionId)
            .SelectMany(s => s.Students)
            .Select(s => new StudentDto()
            {
                Id = s.Id,
                FullName = s.FullName,
                StudentNumber = s.StudentNumber,
                Email = s.AppUser.Email,
                Gpa = s.ExamResults.Count(examResult => examResult.Score != -1) == 0 ? -1 : (s.ExamResults.Where(e => e.Score != -1).Sum(e => e.Score) / s.ExamResults.Count(e => e.Score != -1)),
                Sections = s.Sections.Select(ss => new SectionDto()
                {
                    Course = new CourseDto()
                    {
                        Unit = ss.Course.Unit,
                    }
                }).ToList(),
            })
            .ToListAsync();
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

        if (instructor.Sections?.Count != 0){
            context.Sections.RemoveRange(instructor.Sections);
        }

        var userId = instructor.AppUserId;
        context.Instructors.Remove(instructor);

        if (!await userRepository.DeleteUser(userId)){
            return new Result()
            {
                Message = "Something went wrong",
            };
        }

        await context.SaveChangesAsync();


        return new Result()
        {
            Message = "Instructor has been removed",
            Succeeded = true
        };
    }

    public async Task<Result> UpdateInstructor(Instructor instructor)
    {
        context.Instructors.Update(instructor);
        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Instructor has been updated",
        };
    }

}
