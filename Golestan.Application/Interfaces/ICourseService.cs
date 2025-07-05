namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs.Course;
using DTOs.Instructor;
using Shared.Helpers;


public interface ICourseService {

    Task<List<CourseDto>> GetFacultyCourses(int facultyId);

    Task<CourseDto> GetCourseById(int courseId);

    Task<CourseInstructorDto> GetAvailableInsturctorsForCourse(int facultyId, int courseId);

    Task<Dictionary<int, string>?> GetCourseInstructors(int courseId);

    Task<List<CourseDto>> GetAvailableCoursesForPrerequisite(int courseId);

    Task<List<Course>> GetAvailableCoursesForStudent(int studentId);

    Task<Result> ApplyInstructorToCourse(CourseInstructorDto dto);

    Task<Result> AddCourse(AddCourseDto dto);

    Task<Result> SetExam(SetExamForCourseDto dto);

    Task<Result> RemoveCourse(int courseId);

    Task<Result> AddPrerequisiteToCourse(int courseId, int prerequisiteCourseId);

}
