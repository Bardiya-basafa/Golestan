namespace Golestan.Application.Services;

using DTOs.Selection;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;


public class SelectionService : ISelectionService {

    private readonly AppDbContext _context;

    private readonly ITermService _termService;

    private readonly ICourseService _courseService;

    public SelectionService(AppDbContext context, ITermService termService, ICourseService courseService)
    {
        _context = context;
        _termService = termService;
        _courseService = courseService;
    }

    public async Task<SelectionDetailsDto> SelectionTermDetails()
    {
        var currentTerm = await _termService.GetCurrentTermEntity();

        var model = new SelectionDetailsDto();

        if (currentTerm == null){
            model.Result.Message = "There is no current term right now";

            return model;
        }

        var currentDate = DateTime.UtcNow;

        if (currentDate > currentTerm.SectionSelectionEndTime){
            model.Result.Message = "The current selection has ended and no available selection";

            return model;
        }

        model.StartDate = currentTerm.SectionSelectionStartTime;
        model.EndDate = currentTerm.SectionSelectionEndTime;
        model.Term = currentTerm.TermText;
        model.Result.Succeeded = true;

        return model;
    }

    public async Task<List<AvailableSectionsDto>?> GetAvailableSectionsForSelection(int studentId)
    {
        var currentTerm = await _termService.GetCurrentTermEntity();

        if (currentTerm == null){
            throw new ArgumentException("There is no current term right now");
        }

        var currentDate = DateTime.UtcNow;

        if (currentTerm.SectionSelectionStartTime > currentDate || currentDate > currentTerm.SectionSelectionEndTime){
            return null;
        }

        var availableCourses = await _courseService.GetAvailableCoursesForStudent(studentId);

        var sections = await _context.Sections
            .Where(s => availableCourses.Select(c => c.Id).Contains(s.CourseId))
            .Select(s => new AvailableSectionsDto()
            {
                ClassroomNumber = s.Classroom.ClassNumber,
                CourseName = s.Course.Name,
                DayOfWeek = s.DayOfWeek,
                TimeSlot = s.TimeSlot,
                SectionId = s.Id,
                InstructorFullName = s.Instructor.FullName,
            })
            .ToListAsync();

        return sections;
    }

}
