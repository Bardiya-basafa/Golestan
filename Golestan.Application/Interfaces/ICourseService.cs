namespace Golestan.Application.Interfaces;

using DTOs.Course;


public interface ICourseService {

    Task<List<CourseDto>> GetCoursesDto();

}
