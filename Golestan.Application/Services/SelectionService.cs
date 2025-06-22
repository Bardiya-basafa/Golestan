namespace Golestan.Application.Services;

using DTOs.Section;
using DTOs.Selection;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;


public class SelectionService(AppDbContext context, ITermService termService, ICourseService courseService) : ISelectionService {

    public async Task<SectionSelectionDetailsDto> SelectionTermDetails()
    {
        var currentTerm = await termService.GetCurrentTermEntity();

        var model = new SectionSelectionDetailsDto();

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
        var currentTerm = await termService.GetCurrentTermEntity();

        if (currentTerm == null){
            throw new ArgumentException("There is no current term right now");
        }

        var currentDate = DateTime.UtcNow;

        if (currentTerm.SectionSelectionStartTime > currentDate || currentDate > currentTerm.SectionSelectionEndTime){
            return null;
        }

        var availableCourses = await courseService.GetAvailableCoursesForStudent(studentId);

        var student = await context.Students
            .Where(s => s.Id == studentId)
            .Include(s => s.Sections)
            .FirstOrDefaultAsync();

        if (student is null){
            throw new ArgumentException("There is no student right now");
        }

        var sections = await context.Sections
            .Where(s => availableCourses.Select(c => c.Id).Contains(s.CourseId) && student.Sections.All(section => section.Id != s.Id))
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

    public async Task<List<SectionDetailsDto>> GetSelectedSections(int studentId)
    {
        var sections = await context.Students
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.Sections)
            .Select(sec => new SectionDetailsDto()
            {
                Id = sec.Id,
                TimeSlot = sec.TimeSlot,
                DayOfWeek = sec.DayOfWeek,
                ClassNumber = sec.Classroom.ClassNumber,
                CourseName = sec.Course.Name,
                InstructorFullName = sec.Instructor.FullName,
                ExamDate = sec.Course.Exam.ExamDateTime,
            })
            .ToListAsync();

        return sections;
    }

    public async Task<Result> SelectSection(int studentId, int sectionId)
    {
        var result = new Result();

        var sectionDetails = await context.Sections
            .Where(s => s.Id == sectionId)
            .Select(s => new DTOs.Section.SectionDetailsDto()
            {
                RemainCapacity = s.Classroom.Capacity - s.Students.Count,
                TimeSlot = s.TimeSlot,
                DayOfWeek = s.DayOfWeek,
                ExamTimeSlot = s.Course.Exam.TimeSlot,
                ExamDate = s.Course.Exam.ExamDateTime,
            })
            .FirstOrDefaultAsync();

        if (sectionDetails is null){
            result.Message = "No section found";

            return result;
        }

        if (sectionDetails.RemainCapacity == 0){
            result.Message = "Section is full and cannot be selected";

            return result;
        }

        var sectionExist = await context.Students
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.Sections)
            .AnyAsync(sec => sec.TimeSlot == sectionDetails.TimeSlot && sec.DayOfWeek == sectionDetails.DayOfWeek);

        if (sectionExist){
            result.Message = "There is another section in this time";

            return result;
        }

        var examExist = await context.Students
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.Sections)
            .Select(s => s.Course.Exam)
            .AnyAsync(e => sectionDetails.ExamDate == e.ExamDateTime && e.TimeSlot == sectionDetails.ExamTimeSlot);

        if (examExist){
            result.Message = "There is an exam in the section time exam";

            return result;
        }

        var section = await context.Sections
            .Include(s => s.Course)
            .FirstOrDefaultAsync(s => s.Id == sectionId);

        var student = await context.Students
            .FindAsync(studentId);

        if (student is null){
            result.Message = "There is no student right now";

            return result;
        }

        if (section is null){
            result.Message = "No section found";

            return result;
        }

        var studentTotalUnit = await context.Students
            .Where(s => s.Id == studentId)
            .SelectMany(s => s.Sections)
            .Select(s => s.Course)
            .SumAsync(c => c.Unit);

        if (studentTotalUnit > 24){
            result.Message = "Something went wrong";

            return result;
        }

        if (section.Course.Unit + studentTotalUnit > 24){
            result.Message = "You can not select more than 24 units";

            return result;
        }

        student.Sections.Add(section);
        context.Update(student);
        await context.SaveChangesAsync();
        result.Succeeded = true;
        result.Message = "Section selected";

        return result;
    }

    public async Task<Result> UnselectSection(int studentId, int sectionId)
    {
        var result = new Result();

        var student = await context.Students
            .Include(s => s.Sections)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student is null){
            result.Message = "No student found";

            return result;
        }

        var section = student.Sections.FirstOrDefault(s => s.Id == sectionId);

        if (section is null){
            result.Message = "No section found";

            return result;
        }

        student.Sections.Remove(section);
        context.Update(student);
        await context.SaveChangesAsync();
        result.Succeeded = true;
        result.Message = "Section unselected";

        return result;
    }

}
