namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using Domain.Enums;
using DTOs.Course;
using DTOs.Exam;
using Shared.Helpers;


public interface ICourseRepository {

    Task<Course> GetCourseEntityById(int courseId);

    Task<List<CourseDto>> GetFacultyCourses(int facultyId);

    Task<CourseDto> GetCourseDtoById(int courseId);

    Task<List<CourseDto>> GetPrerequisiteCourses(List<int> prerequisiteCourseIds);

    Task<Dictionary<int, string>?> GetCourseInstructors(int courseId);

    Task<Dictionary<int, string>?> GetExamClassrooms(int courseId);

    Task<List<CourseDto>> GetAvailableCoursesForPrerequisite(int courseId);

    Task<List<Course>> GetAvailableCoursesForStudent(int studentId);

    Task<CourseInstructorDto> GetAvailableInstructorsForCourse(int facultyId, int courseId);

    Task<ExamDto?> GetCourseExam(int courseId);

    Task<Result> ApplyInstructorToCourse(int courseId, int instructorId);

    Task<Result> AddCourse(Course course);

    Task<Result> RemoveCourse(int courseId);

    Task<Result> AddPrerequisiteToCourse(int courseId, int prerequisiteId);

    Task<bool> CourseNameExist(string courseName, int facultyId);

    Task<Result> SetExam(Exam exam);

    Task<Result> RemovePrerequisiteFromCourse(Course course, int prerequisiteId);

    Task<bool> IsExamExistInClass(DateTime examDate, TimeSlot timeSlot, int classroomId);

}
