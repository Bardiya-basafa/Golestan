namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using DTOs.Course;
using DTOs.ExamResult;
using DTOs.Instructor;
using DTOs.Score;
using DTOs.Student;
using DTOs.Term;
using Shared.Helpers;


public interface IInstructorRepository {

    Task<InstructorDto> GetInstructorAppUser(string instructorId);

    Task<InstructorDto> GetInstructorInfo(string instructorId);

    Task<List<TermDto>> GetAllInstructorTerms(string instructorId);

    Task<List<CourseDto>> GetCourses(string instructorId);


    Task<InstructorDto> GetInstructorDtoById(string instructorId);

    Task<List<InstructorDto>> GetFacultyInstructors();

    Task<List<StudentDto>> GetInstructorStudentsOfSection(int sectionId, string instructorId);


    Task<Result> RemoveCourseInstructor(int instructorId, int courseId);

    Task<Result> RemoveInstructor(int instructorId);

    Task<Result> UpdateInstructor(Instructor instructor);

}
