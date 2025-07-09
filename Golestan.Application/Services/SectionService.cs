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
        try{
            return await sectionRepository.GetFacultySections(facultyId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<SectionDto> GetSectionActionsDto(int sectionId)
    {
        try{
            var model = await sectionRepository.GetSectionById(sectionId);

            model.Students = await sectionRepository.GetSectionStudents(sectionId);

            return model;
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<StudentDto>> GetAvailableStudents(int sectionId, int facultyId)
    {
        try{
            var section = await sectionRepository.GetSectionById(sectionId);


            var course = await courseRepository.GetCourseById(section.Course.Id);


            var prerequisiteCourses = course.PrerequisiteCourses;

            return await sectionRepository.GetAvailableStudentsForSection(section, course, facultyId, prerequisiteCourses);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<SectionDto> GetSectionById(int sectionId)
    {
        try{
            return await sectionRepository.GetSectionById(sectionId);
        }
        catch (Exception e){
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task<Result> AddStudentsToSection(List<int> studentIds, int sectionId)
    {
        var result = new Result();

        try{
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
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> RemoveStudentFromSection(int studentId, int sectionId)
    {
        try{
            var section = await sectionRepository.GetSectionEntityById(sectionId);

            var student = await studentRepository.GetStudentEntityById(studentId);


            if (section.Students.Contains(student)){
                section.Students.Remove(student);
            }

            if (student.Sections.Contains(section)){
                student.Sections.Remove(section);
            }


            return await sectionRepository.RemoveExamResult(sectionId, studentId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> AddSection(AddSectionDto dto)
    {
        var result = new Result();

        try{
            if (0 < dto.TimeSlotId && dto.TimeSlotId <= 6 && 0 < dto.DayOfWeekId && dto.DayOfWeekId <= 7){
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

            var section = new Section()
            {
                CourseId = dto.CourseId,
                ClassroomId = dto.ClassroomId,
                InstructorId = dto.InstructorId,
                TimeSlot = GetTimeSlot(dto.TimeSlotId),
                DayOfWeek = GetDayOfWeek(dto.DayOfWeekId),
            };

            return await sectionRepository.AddSection(section);
        }
        catch (Exception e){
            throw new Exception(e.Message);
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

}
