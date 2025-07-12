namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using Domain.Enums;
using DTOs.Course;
using DTOs.Section;
using DTOs.Student;
using DTOs.Term;
using Shared.Helpers;


public interface ISectionRepository {

    Task<List<Section>> GetClassroomSections(int classroomId);

    Task<List<SectionDto>> GetFacultySections(int facultyId);

    Task<SectionDto> GetSectionById(int sectionId);

    Task<Section> GetSectionEntityById(int sectionId);

    Task<List<StudentDto>> GetSectionStudents(int sectionId);

    Task<List<StudentDto>> GetAvailableStudentsForSection(SectionDto section, CourseDto course, int facultyId, List<int> prerequisitesCourses);

    Task<Course> GetCourseBySectionId(int sectionId);

    Task<Result> AddStudentsToSection(List<int> studentIds, int sectionId, List<int> prerequisitesCourses, TermDto term, Course course);

    Task<Result> RemoveExamResult(int sectionId, int studentId);

    Task<Result> AddSection(Section section);

    Task<bool> IsClassroomTakenAtTime(int classroomId, TimeSlot timeSlot, DayOfWeek dayOfWeek);

    Task<bool> IsInstructorTakenAtTime(int instructorId, TimeSlot timeSlot, DayOfWeek dayOfWeek);

}
