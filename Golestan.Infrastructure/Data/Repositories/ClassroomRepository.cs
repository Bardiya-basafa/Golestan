namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.Classroom;
using Application.DTOs.Course;
using Application.DTOs.Faculty;
using Application.DTOs.Instructor;
using Application.DTOs.Section;
using Application.DTOs.Student;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class ClassroomRepository(AppDbContext context) : IClassroomRepository {

    public async Task<List<Classroom>> GetFacultyClassrooms(int facultyId)
    {
        return await context.Classrooms
            .Where(c => c.FacultyId == facultyId)
            .Take(10)
            .ToListAsync();
    }

    public async Task<ClassroomDto> GetClassroomDtoById(int classroomId)
    {
        return await context.Classrooms
            .AsNoTracking()
            .Where(c => c.Id == classroomId)
            .Select(c => new ClassroomDto()
            {
                Id = c.Id,
                ClassroomNumber = c.ClassNumber,
                Capacity = c.Capacity,
                Faculty = new FacultyDto()
                {
                    Id = c.FacultyId,
                    MajorName = c.Faculty.MajorName,
                },
                Sections = c.Sections.Select(s => new SectionDto()
                {
                    Id = s.Id,
                    Course = new CourseDto()
                    {
                        Id = s.CourseId,
                        CourseName = s.Course.CourseName,
                    },
                    Students = s.Students.Select(st => new StudentDto()
                    {
                        Id = st.Id,
                    }).ToList(),
                    TimeSlot = s.TimeSlot,
                    DayOfWeek = s.DayOfWeek,
                    Instructor = new InstructorDto()
                    {
                        Id = s.InstructorId,
                        FullName = s.Instructor.FullName,
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync() ?? throw new Exception("Class room not found");
    }

    public async Task<Result> AddClassroom(Classroom classroom)
    {
        context.Classrooms.Add(classroom);
        await context.SaveChangesAsync();


        return new Result()
        {
            Succeeded = true,
            Message = "Class room added"
        };
    }

    public async Task<Result> RemoveClassroom(int classroomId)
    {
        var result = new Result();

        var classroom = await context.Classrooms
            .Where(c => c.Id == classroomId)
            .Include(s => s.Sections)
            .FirstOrDefaultAsync();

        if (classroom == null){
            result.Message = "Class room not found";

            return result;
        }

        context.Sections.RemoveRange(classroom.Sections);
        context.Classrooms.Remove(classroom);
        await context.SaveChangesAsync();
        result.Succeeded = true;
        result.Message = "Class room removed";

        return result;
    }

    public async Task<bool> VerifyClassroomNumber(string classroomNumber, int facultyId)
    {
        return await context.Classrooms.AnyAsync(c => c.FacultyId == facultyId && c.ClassNumber == classroomNumber);
    }

}
