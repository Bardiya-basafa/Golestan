namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.Instructor;
using DTOs.Section;
using DTOs.Student;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;


public class InstructorService : IInstructorService {

    private readonly AppDbContext _context;

    public InstructorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<InstructorDetailsDto>> GetFacultyInstructors()
    {
        try{
            var instructors = await _context.Instructors
                .Select(i => new InstructorDetailsDto()
                {
                    Id = i.Id,
                    AppUserId = i.AppUserId,
                    Email = i.AppUser.Email,
                    FacultyId = i.FacultyId,
                    FacultyName = i.Faculty.MajorName,
                    FullName = $"{i.AppUser.FirstName} {i.AppUser.LastName}",
                    HireDate = i.HireDate,
                    Salary = i.Salary,
                })
                .OrderBy(i => i.Id)
                .Take(10)
                .ToListAsync();

            return instructors;
        }
        catch (Exception e){
            Console.WriteLine(e);

            return new List<InstructorDetailsDto>();
        }
    }

    public async Task<List<SectionDetailsDto>> GetInstructorSections(int instructorId)
    {
        var model = await _context.Instructors
            .Where(i => i.Id == instructorId)
            .SelectMany(i => i.Sections)
            .Select(s => new SectionDetailsDto()
            {
                Id = s.Id,
                ClassCapacity = s.Classroom.Capacity,
                ClassNumber = s.Classroom.ClassNumber,
                DayOfWeek = s.DayOfWeek,
                TimeSlot = s.TimeSlot,
                CurrentStudents = s.Students.Count,
            })
            .ToListAsync();

        return model;
    }

    public Task<List<StudentDetailsDto>> GetInstructorStudentsForSection(int sectionId)
    {
        var model = _context.Sections
            .Where(s => s.Id == sectionId)
            .SelectMany(s => s.Students)
            .Select(s => new StudentDetailsDto()
            {
                Id = s.Id,
                FullName = s.FullName,
                StudentNumber = s.StudentNumber,
            })
            .ToListAsync();

        return model;
    }

    public async Task<Result> RemoveCourseInstructor(int instructorId, int courseId)
    {
        var result = new Result();

        try{
            var course = await _context.Courses
                .Where(c => c.Id == courseId)
                .Include(c => c.Instructors)
                .FirstOrDefaultAsync();

            if (course == null){
                result.Message = "Course not found";

                return result;
            }

            var instructor = await _context.Instructors.FindAsync(instructorId);

            if (instructor == null){
                result.Message = "Instructor not found";

                return result;
            }

            var sections = await _context.Sections
                .Where(s => s.InstructorId == instructorId && s.CourseId == courseId)
                .ToListAsync();

            _context.Sections.RemoveRange(sections);
            course.Instructors.Remove(instructor);
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            result.Message = "Instructor removed";
            result.Succeeded = true;

            return result;
        }
        catch (Exception e){
            Console.WriteLine(e);
            result.Message = e.Message;

            return result;
        }
    }

    public async Task<Result> RemoveInstructor(int instructorId)
    {
        var result = new Result();

        try{
            var instructor = await _context.Instructors.Where(i => i.Id == instructorId)
                .Include(i => i.Sections)
                .FirstOrDefaultAsync();

            if (instructor == null){
                result.Message = "Instructor not found";

                return result;
            }

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            result.Message = "Instructor removed";
            result.Succeeded = true;
        }
        catch (Exception e){
            Console.WriteLine(e);

            result.Message = e.Message;
        }

        return result;
    }

}
