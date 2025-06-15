namespace Golestan.Application.Services;

using Domain.Entities;
using Domain.Enums;
using DTOs.Course;
using DTOs.Section;
using DTOs.Student;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;


public class SectionService : ISectionService {

    private readonly AppDbContext _context;

    private readonly IFacultyService _facultyService;

    public SectionService(AppDbContext context, IFacultyService facultyService)
    {
        _context = context;
        _facultyService = facultyService;
    }

    public async Task<SectionManagementDto> GetFacultySections(int facultyId)
    {
        try{
            var dto = new SectionManagementDto()
            {
                FacultyId = facultyId,
            };

            var faculty = await _facultyService.GetDetailsFacultyById(facultyId);

            if (faculty == null){
                throw new ApplicationException($"Faculty {facultyId} not found");
            }

            dto.FacultyMajorName = faculty.MajorName;

            dto.Sections = await _context.Sections
                .Where(s => s.Id == facultyId)
                .Select(s => new SectionDetailsDto()
                {
                    Id = s.Id,
                    CourseName = s.Course.Name,
                    TimeSlot = s.TimeSlot,
                    InstructorAppUser = s.Instructor.AppUser,
                    InstructorId = s.InstructorId,
                    ClassNumber = s.Classroom.ClassNumber,
                })
                .ToListAsync();

            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<SectionActionsDto> GetSectionActionsDto(int sectionId)
    {
        try{
            var dto = new SectionActionsDto();

            dto = await _context.Sections
                .Where(s => s.Id == sectionId)
                .Select(s => new SectionActionsDto()
                {
                    Id = s.Id,
                    CourseName = s.Course.Name,
                    TimeSlot = s.TimeSlot,
                    InstructorId = s.InstructorId,
                    InstructorName = s.Instructor.FullName,
                    ClassroomNumber = s.Classroom.ClassNumber,
                    ClassroomCapacity = s.Classroom.Capacity,
                    DayOfWeek = s.DayOfWeek,
                    FacultyId = s.Course.FacultyId,
                })
                .FirstOrDefaultAsync();

            dto.Students = await _context.Students
                .Where(s => s.Sections.Any(s => s.Id == sectionId))
                .Select(s => new StudentDetailsDto()
                {
                    Id = s.Id,
                    AppUserId = s.AppUserId,
                    Email = s.AppUser.Email,
                    StudentNumber = s.StudentNumber,
                    FullName = s.FullName,
                    FacultyId = s.FacultyId,
                    FacultyName = s.Faculty.MajorName,
                })
                .ToListAsync();

            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<List<StudentDetailsDto>> GetAvailableStudents(int sectionId, int facultyId)
    {
        try{
            // var courseId = await _context.Courses
            //     .Where(c => c.Sections.Any(s => s.Id == sectionId))
            //     .Select(s => s.Id)
            //     .FirstOrDefaultAsync();


            var dto = await _context.Students
                .Where(s => s.FacultyId == facultyId && s.Sections.All(s => s.Id != sectionId))
                .Select(s => new StudentDetailsDto()
                {
                    Id = s.Id,
                    AppUserId = s.AppUserId,
                    Email = s.AppUser.Email,
                    StudentNumber = s.StudentNumber,
                    FullName = s.FullName,
                    FacultyId = s.FacultyId,
                })
                .ToListAsync();

            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<SectionDetailsDto> GetSectionDetailsById(int sectionId)
    {
        try{
            var dto = await _context.Sections
                .Where(s => s.Id == sectionId)
                .Select(s => new SectionDetailsDto()
                {
                    Id = s.Id,
                    CourseName = s.Course.Name,
                    TimeSlot = s.TimeSlot,
                    InstructorAppUser = s.Instructor.AppUser,
                    ClassNumber = s.Classroom.ClassNumber,
                    DayOfWeek = s.DayOfWeek,
                    InstructorId = s.InstructorId,
                    ClassCapacity = s.Classroom.Capacity,
                    RemainCapacity = s.Classroom.Capacity - s.Students.Count
                })
                .FirstOrDefaultAsync();

            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<Result> AddStudentsToSection(List<int> studentIds, int sectionId)
    {
        var finalResult = new Result();

        try{
            if (studentIds.Count == 0){
                finalResult.Message = "No students found";

                return finalResult;
            }

            var sectionStudents = await _context.Sections.Where(s => s.Id == sectionId).SelectMany(s => s.Students).ToListAsync();
            var section = await _context.Sections.Where(s => s.Id == sectionId).Include(s => s.Students).Include(s => s.Classroom).FirstOrDefaultAsync();
            var capacity = section.Classroom.Capacity;
            var currentStudentCount = section.Students.Count;
            var students = await _context.Students.Where(s => studentIds.Contains(s.Id) && s.Sections.All(section1 => section1.Id != sectionId)).Take(capacity - currentStudentCount).ToListAsync();
            sectionStudents.AddRange(students);
            section.Students = sectionStudents;
            _context.Sections.Update(section);
            await _context.SaveChangesAsync();
            finalResult.Succeeded = true;
            finalResult.Message = "Students added";

            return finalResult;
        }
        catch (Exception e){
            Console.WriteLine(e);
            finalResult.Message = e.Message;

            throw;
        }

        return finalResult;
    }

    public async Task<Result> AddNewSection(AddSectionDto dto)
    {
        var finalResult = new Result();

        try{
            if (0 < dto.TimeSlotId && dto.TimeSlotId <= 6 && 0 < dto.DayOfWeekId && dto.DayOfWeekId <= 7){
                finalResult.Message = "time ranges must valid";

                return finalResult;
            }

            var sectionExist = await IsValidSection(dto);

            if (sectionExist){
                finalResult.Message = "The classroom in that time is taken";

                return finalResult;
            }

            var section = new Section()
            {
                CourseId = dto.CourseId,
                ClassroomId = dto.ClassroomId,
                InstructorId = dto.InstructorId,
                TimeSlot = GetTimeSlot(dto.TimeSlotId),
                DayOfWeek = GetDayOfWeek(dto.DayOfWeekId),
            };

            await _context.Sections.AddAsync(section);
            await _context.SaveChangesAsync();
            finalResult.Succeeded = true;
            finalResult.Message = "Section added successfully";

            return finalResult;
        }
        catch (Exception e){
            Console.WriteLine(e);

            finalResult.Message = e.Message;

            throw;
        }
    }

    private TimeSlot GetTimeSlot(int timeSlotId)
    {
        switch (timeSlotId){
            case 1: return TimeSlot.First; break;

            case 2: return TimeSlot.Second; break;

            case 3: return TimeSlot.Third; break;

            case 4: return TimeSlot.Fourth; break;

            case 5: return TimeSlot.Fifth; break;

            case 6: return TimeSlot.Sixth; break;

            default: return TimeSlot.First;
        }
    }

    private DayOfWeek GetDayOfWeek(int dayOfWeekId)
    {
        switch (dayOfWeekId){
            case 1: return DayOfWeek.Saturday;

            case 2: return DayOfWeek.Sunday;

            case 3: return DayOfWeek.Monday;

            case 4: return DayOfWeek.Tuesday;

            case 5: return DayOfWeek.Wednesday;

            case 6: return DayOfWeek.Thursday;

            case 7: return DayOfWeek.Friday;

            default: return DayOfWeek.Saturday;
        }
    }

    private async Task<bool> IsValidSection(AddSectionDto dto)
    {
        var sectionExist = await _context.Sections
            .AnyAsync(s => s.ClassroomId == dto.ClassroomId && s.DayOfWeek == GetDayOfWeek(dto.DayOfWeekId) && s.TimeSlot == GetTimeSlot(dto.TimeSlotId));

        if (sectionExist){
            return true;
        }

        return false;
    }

}
