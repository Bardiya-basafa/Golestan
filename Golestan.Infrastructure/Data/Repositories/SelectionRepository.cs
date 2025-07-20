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


public class SelectionRepository(AppDbContext context) : ISelectionRepository {

    public async Task<List<SectionDto>> GetAvailableSections(List<Course> availableCourses, Student student)
    {
        var sections = await context.Sections
            .AsNoTracking()
            .Include(section => section.Classroom).ThenInclude(c => c.Faculty)
            .Include(section => section.Course).ThenInclude(c => c.Exam)
            .Include(section => section.Instructor)
            .ToListAsync();

        return sections
            .AsEnumerable()
            .Where(s => availableCourses.Any(c => c.Id == s.CourseId)
                        && s.Course.Exam != null
                        && !student.Sections.Any(section => section.Id == s.Id))
            .Select(s => new SectionDto()
            {
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
                    CourseName = s.Course.CourseName,
                    Unit = s.Course.Unit,
                    Exam = new ExamDto()
                    {
                        TimeSlot = s.Course.Exam.TimeSlot,
                        ExamDateTime = s.Course.Exam.ExamDateTime,
                    }
                },
                DayOfWeek = s.DayOfWeek,
                TimeSlot = s.TimeSlot,
                Id = s.Id,
                Instructor = new InstructorDto()
                {
                    FullName = s.Instructor.FullName,
                }
            }).ToList();
    }

    public async Task<List<SectionDto>> GetSelectedSections(string studentId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.AppUserId == studentId)
            .SelectMany(s => s.Sections)
            .Select(sec => new SectionDto()
            {
                Id = sec.Id,
                TimeSlot = sec.TimeSlot,
                DayOfWeek = sec.DayOfWeek,
                Classroom = new ClassroomDto()
                {
                    ClassroomNumber = sec.Classroom.ClassNumber,
                },
                Course = new CourseDto()
                {
                    CourseName = sec.Course.CourseName,
                    Unit = sec.Course.Unit,
                },
                Instructor = new InstructorDto()
                {
                    FullName = sec.Instructor.FullName,
                },
                ExamDate = sec.Course.Exam.ExamDateTime,
            })
            .ToListAsync();
    }


    public async Task<bool> IsStudentTimeTaken(string studentId, Section section)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.AppUserId == studentId)
            .SelectMany(s => s.Sections)
            .AnyAsync(sec => sec.TimeSlot == section.TimeSlot && sec.DayOfWeek == section.DayOfWeek);
    }

    public async Task<bool> IsExamTimeTaken(string studentId, DateTime examDate, TimeSlot examTimeSlot)
    {
        return await context.Students
            .Where(s => s.AppUserId == studentId)
            .SelectMany(s => s.Sections)
            .Select(s => s.Course.Exam)
            .AnyAsync(e => examDate == e.ExamDateTime && e.TimeSlot == examTimeSlot);
    }

}
