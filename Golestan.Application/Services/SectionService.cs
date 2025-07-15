namespace Golestan.Application.Services;

using Domain.Entities;
using Domain.Enums;
using DTOs.Section;
using DTOs.Student;
using Interfaces;
using RepositoryInterfaces;
using Shared.Helpers;


public class SectionService(ISectionRepository sectionRepository, ICourseRepository courseRepository, IStudentRepository studentRepository, ITermService termService) : ISectionService {

    public async Task<List<SectionDto>> GetFacultySections(int facultyId)
    {
        return await sectionRepository.GetFacultySections(facultyId);
    }

    public async Task<SectionDto> GetSectionActionsDto(int sectionId)
    {
        var model = await sectionRepository.GetSectionById(sectionId);

        model.Students = await sectionRepository.GetSectionStudents(sectionId);

        return model;
    }

    public async Task<List<StudentDto>> GetAvailableStudents(int sectionId)
    {
        var section = await sectionRepository.GetSectionById(sectionId);


        var course = await courseRepository.GetCourseDtoById(section.Course.Id);


        var prerequisiteCourses = course.PrerequisiteCourses;

        return await sectionRepository.GetAvailableStudentsForSection(section, course, course.Faculty.Id, prerequisiteCourses);
    }

    public async Task<SectionDto> GetSectionById(int sectionId)
    {
        return await sectionRepository.GetSectionById(sectionId);
    }

    public async Task<Result> AddStudentsToSection(List<int> studentIds, int sectionId)
    {
        var result = new Result();

        if (studentIds.Count == 0){
            result.Message = "No students found";

            return result;
        }

        var course = await sectionRepository.GetCourseBySectionId(sectionId);


        var prerequisiteCourses = course.PrerequisiteCourses;
        var term = await termService.GetCurrentTerm();

        if (term == null){
            result.Message = "No term available";

            return result;
        }


        return await sectionRepository.AddStudentsToSection(studentIds, sectionId, prerequisiteCourses, term, course);
    }

    public async Task<Result> RemoveStudentFromSection(int studentId, int sectionId)
    {
        var student = await sectionRepository.GetStudent(studentId);

        var section = await sectionRepository.GetSection(sectionId);


        await sectionRepository.RemoveStudent(section, student);


        // return await sectionRepository.RemoveExamResult(sectionId, studentId);
      return new Result();
    }

    public async Task<Result> AddSection(AddSectionDto dto)
    {
        var result = new Result();


        if (0 > dto.TimeSlotId || dto.TimeSlotId > 6 || 0 > dto.DayOfWeekId || dto.DayOfWeekId > 7){
            result.Message = "time ranges must valid";

            return result;
        }


        if (await sectionRepository.IsClassroomTakenAtTime(dto.ClassroomId, GetTimeSlot(dto.TimeSlotId), GetDayOfWeek(dto.DayOfWeekId))){
            result.Message = "The classroom in that time is taken";

            return result;
        }


        if (await sectionRepository.IsInstructorTakenAtTime(dto.InstructorId, GetTimeSlot(dto.TimeSlotId), GetDayOfWeek(dto.DayOfWeekId))){
            result.Message = "The instructor time is taken";

            return result;
        }

        var exam = await sectionRepository.GetCourseExam(dto.CourseId);

        if (exam == null){
            result.Message = "Set an exam for course before adding section to it";

            return result;
        }

        var term = await termService.GetCurrentTerm();

        if (term == null){
            result.Message = "No term available";

            return result;
        }

        var section = new Section()
        {
            CourseId = dto.CourseId,
            ClassroomId = dto.ClassroomId,
            InstructorId = dto.InstructorId,
            TimeSlot = GetTimeSlot(dto.TimeSlotId),
            DayOfWeek = GetDayOfWeek(dto.DayOfWeekId),
            TermId = term.Id,
        };

        return await sectionRepository.AddSection(section);
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

}
