namespace Golestan.Application.Services;

using DTOs.Section;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;


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

}
