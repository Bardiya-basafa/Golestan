namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.Classroom;
using Application.DTOs.Course;
using Application.DTOs.Faculty;
using Application.DTOs.Instructor;
using Application.DTOs.Section;
using Application.DTOs.Student;
using Application.DTOs.Term;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class SectionRepository(AppDbContext context) : ISectionRepository {

    public async Task<List<Section>> GetClassroomSections(int classroomId)
    {
        return await context.Sections
            .AsNoTracking()
            .Where(s => s.ClassroomId == classroomId)
            .ToListAsync();
    }

    public async Task<List<SectionDto>> GetFacultySections(int facultyId)
    {
        return await context.Sections
            .AsNoTracking()
            .Where(s => s.Course.FacultyId == facultyId)
            .Select(s => new SectionDto()
            {
                Id = s.Id,
                Course = new CourseDto()
                {
                    CourseName = s.Course.CourseName,
                },
                Instructor = new InstructorDto()
                {
                    Id = s.InstructorId,
                    FullName = s.Instructor.FullName,
                },
                Classroom = new ClassroomDto()
                {
                    ClassroomNumber = s.Classroom.ClassNumber,
                },
                TimeSlot = s.TimeSlot,
                InstructorAppUser = s.Instructor.AppUser,
            })
            .ToListAsync();
    }

    public async Task<SectionDto> GetSectionById(int sectionId)
    {
        return await context.Sections
            .AsNoTracking()
            .Where(s => s.Id == sectionId)
            .Select(s => new SectionDto()
            {
                Id = s.Id,
                Course = new CourseDto()
                {
                    Id = s.CourseId,
                    CourseName = s.Course.CourseName,
                    Faculty = new FacultyDto()
                    {
                        Id = s.Course.FacultyId,
                    }
                },
                TimeSlot = s.TimeSlot,
                Instructor = new InstructorDto()
                {
                    Id = s.InstructorId,
                    FullName = s.Instructor.FullName,
                },
                Classroom = new ClassroomDto()
                {
                    ClassroomNumber = s.Classroom.ClassNumber,
                    Capacity = s.Classroom.Capacity,
                },
                Students = s.Students.Select(st => new StudentDto()
                {
                    Id = st.Id,
                    FullName = st.FullName,
                    StudentNumber = st.StudentNumber,
                    Email = st.AppUser.Email,
                }).ToList(),

                DayOfWeek = s.DayOfWeek,
            })
            .FirstOrDefaultAsync() ?? throw new Exception($"Section not found with id: {sectionId}");
    }

    public async Task<Section> GetSectionEntityById(int sectionId)
    {
        return await context.Sections
            .AsNoTracking()
            .Where(s => s.Id == sectionId)
            .Include(s => s.Course).ThenInclude(c => c.Exam)
            .Include(s => s.Students)
            .Include(s => s.Classroom)
            .FirstOrDefaultAsync() ?? throw new Exception($"Section not found with id: {sectionId}");
    }

    public async Task<List<StudentDto>> GetSectionStudents(int sectionId)
    {
        return await context.Sections
            .AsNoTracking()
            .Where(s => s.Id == sectionId)
            .SelectMany(s => s.Students)
            .Select(s => new StudentDto()
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.AppUser.UserName,
                StudentNumber = s.StudentNumber,
            })
            .ToListAsync();
    }

    public async Task<List<StudentDto>> GetAvailableStudentsForSection(SectionDto section, CourseDto course, int facultyId, List<int> prerequisiteCourses)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.FacultyId == facultyId
                        && !s.Sections.Any(sec => sec.Id == section.Id)// Changed to Any for clarity
                        && !s.Sections.Any(section1 => section1.DayOfWeek == section.DayOfWeek && section1.TimeSlot == section.TimeSlot)// Changed to Any for clarity
                        && s.PassedCourses.All(p => prerequisiteCourses.Contains(p.Id))// Ensure this is structured correctly
            )
            .Select(s => new StudentDto()
            {
                Id = s.Id,
                AppUser = s.AppUser,
                Email = s.AppUser.Email,
                StudentNumber = s.StudentNumber,
                FullName = s.FullName,
                Faculty = new FacultyDto()
                {
                    Id = s.FacultyId,
                }
            })
            .ToListAsync();
    }

    public async Task<Course> GetCourseBySectionId(int sectionId)
    {
        return await context.Sections
            .AsNoTracking()
            .Include(s => s.Course.Exam)
            .Where(s => s.Id == sectionId)
            .Select(s => s.Course)
            .FirstOrDefaultAsync() ?? throw new Exception($"course not found with section id: {sectionId}");
    }

    public async Task<Exam?> GetCourseExam(int courseId)
    {
        return await context.Exams
            .AsNoTracking()
            .Where(s => s.CourseId == courseId)
            .FirstOrDefaultAsync();
    }

    public async Task<Result> AddStudentsToSection(List<int> studentIds, int sectionId, List<int> prerequisitesCourses, TermDto term, Course course)
    {
        var sectionStudents = await context.Sections
            .Where(s => s.Id == sectionId)
            .SelectMany(s => s.Students)
            .ToListAsync();

        var section = await context.Sections
            .Where(s => s.Id == sectionId)
            .Include(s => s.Students)
            .Include(section => section.Classroom)
            .FirstOrDefaultAsync() ?? throw new Exception($"section not found with section id: {sectionId}");


        var capacity = section.Classroom.Capacity;
        var currentStudentCount = section.Students.Count;

        var students = await context.Students
            .Where(s => studentIds.Contains(s.Id)
                        && s.Sections.All(section1 => section1.Id != sectionId)
                        && s.PassedCourses.All(p => prerequisitesCourses.Contains(p.Id)))// Use inline expression
            .Include(s => s.ExamResults)
            .Take(capacity - currentStudentCount)
            .ToListAsync();

        sectionStudents.AddRange(students);
        section.Students = sectionStudents;
        context.Sections.Update(section);

        var newExamResult = new ExamResult()
        {
            CourseId = course.Id,
            SectionId = sectionId,
            ExamDate =  course.Exam.ExamDateTime,
            InstructorId = section.InstructorId,
            TermId = term.Id,
        };

        foreach (var student in students){
            student.ExamResults.Add(newExamResult);
            context.Students.Update(student);
        }

        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Students added",
        };
    }

    public async Task<Result> RemoveExamResult(int sectionId, int studentId)
    {
        var examResult = await context.ExamResults
            .Where(e => e.StudentId == studentId && e.SectionId == sectionId)
            .FirstOrDefaultAsync() ?? throw new Exception($"Exam result not found with for student id: {studentId}");

        context.ExamResults.Remove(examResult);
        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Student removed",
        };
    }

    public async Task<Result> AddSection(Section section)
    {
        context.Sections.Add(section);
        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Section added",
        };
    }

    public async Task<bool> IsClassroomTakenAtTime(int classroomId, TimeSlot timeSlot, DayOfWeek dayOfWeek)

    {
        return await context.Sections
            .AnyAsync(s => s.ClassroomId == classroomId && s.DayOfWeek == dayOfWeek && s.TimeSlot == timeSlot);
    }

    public async Task<bool> IsInstructorTakenAtTime(int instructorId, TimeSlot timeSlot, DayOfWeek dayOfWeek)
    {
        var instructor = await context.Instructors
            .Where(i => i.Id == instructorId)
            .Include(i => i.Sections)
            .FirstOrDefaultAsync() ?? throw new Exception($"instructor not found with id: {instructorId}");

        return instructor.Sections
            .Any(s => s.TimeSlot == timeSlot && s.DayOfWeek == dayOfWeek);
    }

    public async Task RemoveStudent(Section section, Student student)
    {
        section.Students.Remove(student);
        context.Sections.Update(section);
        student.Sections.Remove(section);
        context.Students.Update(student);
        await context.SaveChangesAsync();
    }

    public async Task<Student> GetStudent(int studentId)
    {
        return await context.Students
            .Include(s => s.Sections)
            .FirstOrDefaultAsync(s => s.Id == studentId) ?? throw new Exception($"student not found with id: {studentId}");
    }

    public async Task<Section> GetSection(int sectionId)
    {
        return await context.Sections
            .Include(s => s.Students)
            .FirstOrDefaultAsync(s => s.Id == sectionId) ?? throw new Exception($"section not found with id: {sectionId}");
    }

}
