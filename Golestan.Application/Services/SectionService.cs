namespace Golestan.Application.Services;

using Domain.Entities;
using Domain.Enums;
using DTOs.Course;
using DTOs.Section;
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
