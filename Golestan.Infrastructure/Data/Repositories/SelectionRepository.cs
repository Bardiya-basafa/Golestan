namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.Classroom;
using Application.DTOs.Course;
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
        return await context.Sections
            .AsNoTracking()
            .Where(s => availableCourses.Select(c => c.Id).Contains(s.CourseId) && student.Sections.All(section => section.Id != s.Id))
            .Select(s => new SectionDto()
            {
                Classroom = new ClassroomDto()
                {
                    ClassroomNumber = s.Classroom.ClassNumber,
                },
                Course = new CourseDto()
                {
                    CourseName = s.Course.CourseName,
                },
                DayOfWeek = s.DayOfWeek,
                TimeSlot = s.TimeSlot,
                Id = s.Id,
                Instructor = new InstructorDto()
                {
                    FullName = s.Instructor.FullName,
                }
            })
            .ToListAsync();
    }

    public async Task<List<SectionDto>> GetSelectedSections(int studentId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.Id == studentId)
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
                },
                Instructor = new InstructorDto()
                {
                    FullName = sec.Instructor.FullName,
                },
                ExamDate = sec.Course.Exam.ExamDateTime,
            })
            .ToListAsync();
    }


    public async Task<bool> IsStudentTimeTaken(int studentId, Section section)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.Sections)
            .AnyAsync(sec => sec.TimeSlot == section.TimeSlot && sec.DayOfWeek == section.DayOfWeek);
    }

    public async Task<bool> IsExamTimeTaken(int studentId, DateTime examDate, TimeSlot examTimeSlot)
    {
        return await context.Students
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.Sections)
            .Select(s => s.Course.Exam)
            .AnyAsync(e => examDate == e.ExamDateTime && e.TimeSlot == examTimeSlot);
    }

}
