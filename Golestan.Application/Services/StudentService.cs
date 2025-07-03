namespace Golestan.Application.Services;

using Domain.Entities;
using Domain.Enums;
using DTOs.ExamResult;
using DTOs.Section;
using DTOs.Student;
using DTOs.Term;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;


public class StudentService : IStudentService {

    private readonly AppDbContext _context;

    private readonly UserManager<AppUser> _userManager;

    private readonly ITermService _termService;

    public StudentService(AppDbContext context, UserManager<AppUser> userManager, ITermService termService)
    {
        _context = context;
        _userManager = userManager;
        _termService = termService;
    }

    public async Task<StudentManagementDto> GetFacultyStudents(int facultyId)
    {
        try{
            var dto = new StudentManagementDto();

            dto.Students = await _context.Students
                .Where(s => s.FacultyId == facultyId)
                .Select(s => new StudentDetailsDto()
                {
                    Id = s.Id,
                    AppUserId = s.AppUserId,
                    Email = s.AppUser.Email,
                    FacultyId = s.FacultyId,
                    FacultyName = s.Faculty.Major,
                    FullName = s.FullName,
                    StudentNumber = s.StudentNumber,
                })
                .ToListAsync();

            var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Id == facultyId);

            if (faculty == null){
                throw new ArgumentException();
            }

            dto.FacultyId = faculty.Id;
            dto.FacultyName = faculty.Major;

            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<StudentSectionsDto> GetStudentSections(int studentId)
    {
        try{
            var dto = await _context.Students
                .Where(s => s.Id == studentId)
                .Select(s => new StudentSectionsDto()
                {
                    StudentId = s.Id,
                    StudentFullName = s.FullName,
                    StudentNumber = s.StudentNumber,
                })
                .FirstOrDefaultAsync();

            dto.Sections = await _context.Students
                .Where(s => s.Id == studentId)
                .SelectMany(s => s.Sections)
                .Select(s => new SectionDetailsDto()
                {
                    ClassCapacity = s.Classroom.Capacity,
                    ClassNumber = s.Classroom.ClassNumber,
                    CourseName = s.Course.Name,
                    DayOfWeek = s.DayOfWeek,
                    TimeSlot = s.TimeSlot,
                    InstructorFullName = s.Instructor.FullName,
                })
                .ToListAsync();

            return dto;
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw new ArgumentException();
        }
    }

    public async Task<List<TermDetailsDto>> GetAllStudentTerms(int studentId)
    {
        var model = await _context.Students
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.Terms)
            .Select(t => new TermDetailsDto()
            {
                Id = t.Id,
                Year = t.Year,
                TermNumber = t.TermNumber,
            })
            .ToListAsync();

        return model;
    }

    public async Task<List<ExamResultDetailsDto>> GetTermExamResults(int termId, int studentId)
    {
        var model = await _context.Students
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.ExamResults)
            .Where(e => e.TermId == termId)
            .Select(e => new ExamResultDetailsDto()
            {
                Score = e.Score,
                Description = e.Description,
            })
            .ToListAsync();

        return model;
    }

    public async Task<List<ExamResultDetailsDto>?> GetActiveExamResults(int studentId)
    {
        var currentTerm = await _termService.GetCurrentTermEntity();

        if (currentTerm == null){
            return null;
        }


        var model = await _context.Students
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.ExamResults)
            .Where(e => e.TermId == currentTerm.Id)
            .Select(e => new ExamResultDetailsDto()
            {
                Score = e.Score,
                Description = e.Description,
                Objection = e.Objection,
            })
            .ToListAsync();

        return model;
    }

    public async Task<Result> SubmitObjection(SubmitObjectionDto dto)
    {
        var result = new Result();
        var isInsideTerm = await _termService.IsInsideAnyTermCurrently();

        if (!isInsideTerm){
            result.Message = "You cant submit an objection right now";

            return result;
        }

        if (dto.Objection == string.Empty){
            result.Message = "You must provide an objection";

            return result;
        }

        var examResult = await _context.Students
            .Where(s => s.Id == dto.StudentId)
            .SelectMany(s => s.ExamResults)
            .Where(e => e.Id == dto.ExamResultId)
            .FirstOrDefaultAsync();

        if (examResult == null){
            result.Message = "Somthing went wrong";

            return result;
        }

        examResult.Objection = dto.Objection;
        _context.Update(examResult);
        await _context.SaveChangesAsync();
        result.Message = "Objection submitted";
        result.Succeeded = true;
        return result;
    }

}
