namespace Golestan.Application.Services;

using DTOs.Classroom;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


public class ClassroomService : IClassroomService {

    private readonly AppDbContext _context;

    public ClassroomService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ManageFucultyClassroomDto> GetFacultyClassrooms(int facultyId)
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

            var dto = new ManageFucultyClassroomDto()
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

}
