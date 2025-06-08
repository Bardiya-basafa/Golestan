namespace Golestan.Application.Interfaces;

using DTOs.Course;
using Shared.Helpers;


public interface ICourseService {

    Task<CourseManagementDto> GetFacultyCourses(int facultyId);

    Task<Result> AddCourse(AddCourseDto dto);

}
