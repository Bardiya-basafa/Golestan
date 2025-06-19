namespace Golestan.Application.Interfaces;

using DTOs.Course;
using DTOs.Instructor;
using Shared.Helpers;


public interface ICourseService {

    Task<CourseManagementDto> GetFacultyCourses(int facultyId);

    Task<CourseActionsDto> GetCourseActionsDto(int courseId);

    Task<ApplyNewInstructorDto> GetAllFacultyInstructors(int facultyId, int courseId);

    Task<Dictionary<int, string>>? GetCourseInstructors(int courseId);

    Task<List<CourseDetailsDto>> GetAvailableCoursesForPrerequisite(int courseId);

    Task<Result> ApplyNewInstructorToCourse(ApplyNewInstructorDto dto);

    Task<Result> AddCourse(AddCourseDto dto);

    Task<Result> RemoveCourse(int courseId);

    Task<Result> AddPrerequisiteToCourse(int courseId, int prerequisiteCourseId);

}
