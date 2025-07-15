namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs.Course;
using DTOs.Exam;
using DTOs.Instructor;
using Shared.Helpers;


public interface ICourseService {

    Task<List<CourseDto>> GetFacultyCourses(int facultyId);

    Task<CourseDto> GetCourseDtoById(int courseId);

    Task<CourseInstructorDto> GetAvailableInsturctorsForCourse(int facultyId, int courseId);

    Task<List<CourseDto>> GetPrerequisiteCourses(int courseId);

    Task<Dictionary<int, string>?> GetCourseInstructors(int courseId);

    Task<List<CourseDto>> GetAvailableCoursesForPrerequisite(int courseId);

    Task<List<Course>> GetAvailableCoursesForStudent(int studentId);

    Task<Dictionary<int, string>?> GetExamClassrooms(int courseId);

    Task<ExamDto?> GetCourseExam(int courseId);

    Task<Result> ApplyInstructorToCourse(CourseInstructorDto dto);

    Task<Result> AddCourse(AddCourseDto dto);

    Task<Result> SetExam(SetExamForCourseDto model);

    Task<Result> RemoveCourse(int courseId);

    Task<Result> AddPrerequisiteToCourse(int courseId, int prerequisiteCourseId);

    Task<Result> RemovePrerequisiteFromCourse(int courseId, int prerequisiteCourseId);

}
