namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.Classroom;
using DTOs.Section;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;


public class ClassroomService : IClassroomService {

    private readonly AppDbContext _context;

    public ClassroomService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClassroomManagement> GetFacultyClassrooms(int facultyId)
    {
        try{
            var classroomsDetailsDto = await _context.Classrooms
                .Where(c => c.FacultyId == facultyId)
                .Select(c => new ClassroomDetailsDto()
                {
                    Id = c.Id,
                    ClassNumber = c.ClassNumber,
                    Capacity = c.Capacity,
                })
                .Take(10)
                .ToListAsync();

            var dto = new ClassroomManagement()
            {
                Classrooms = classroomsDetailsDto,
                FacultyId = facultyId,
            };

            dto.FacultyName = await _context.Faculties.Where(f => f.Id == facultyId).Select(f => f.MajorName).FirstOrDefaultAsync();


            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<ClassroomDto> GetClassroomManagementDto(int classroomId)
    {
        try{
            var dto = await _context.Classrooms
                .Where(c => c.Id == classroomId)
                .Select(classroom => new ClassroomDto()
                {
                    FacultyId = classroom.FacultyId,
                    ClassroomId = classroom.Id,
                    ClassroomNumber = classroom.ClassNumber,
                    Capacity = classroom.Capacity,
                    FacultyName = classroom.Faculty.MajorName,
                })
                .FirstOrDefaultAsync();

            if (dto == null){
                throw new NullReferenceException("Class room not found");
            }

            dto.Sections = await _context.Sections
                .Where(s => s.ClassroomId == classroomId)
                .Select(s => new SectionDetailsDto()
                {
                    Id = s.Id,
                    InstructorAppUser = s.Instructor.AppUser,
                    InstructorId = s.InstructorId,
                    CourseName = s.Course.Name,
                    TimeSlot = s.TimeSlot,
                })
                .ToListAsync();


            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw new ArgumentException();
        }
    }

    public async Task<Result> AddClassroom(AddClassroomDto dto)
    {
        var finalResult = new Result();

        try{
            var classroom = new Classroom()
            {
                ClassNumber = dto.ClassNumber,
                Capacity = dto.Capacity,
                FacultyId = dto.FacultyId,
            };

            _context.Classrooms.Add(classroom);
            await _context.SaveChangesAsync();
            finalResult.Succeeded = true;
            finalResult.Message = "Classroom added";

            return finalResult;
        }
        catch (Exception e){
            Console.WriteLine(e);

            finalResult.Message = "Failed to create classroom";

            return finalResult;
        }
    }

    public async Task<bool> VerifyClassroomNumber(string classNumber, int facultyId)
    {
        var classroom = await _context.Classrooms
            .Where(c => c.ClassNumber == classNumber && c.FacultyId == facultyId)
            .FirstOrDefaultAsync();

        if (classroom == null){
            return false;
        }

        return true;
    }

}
