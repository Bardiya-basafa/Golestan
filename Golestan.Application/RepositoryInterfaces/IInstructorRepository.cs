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

    Task<InstructorDto> GetInstructorInfo(int instructorId);

    Task<List<TermDto>> GetAllInstructorTerms(int instructorId);
    Task<List<CourseDto>> GetCourses(int instructorId);


    Task<InstructorDto> GetInstructorDtoById(int instructorId);

    Task<List<InstructorDto>> GetFacultyInstructors();

    Task<List<StudentDto>> GetInstructorStudentsOfSection(int sectionId);


    Task<Result> RemoveCourseInstructor(int instructorId, int courseId);

    Task<Result> RemoveInstructor(int instructorId);

    Task<Result> UpdateInstructor(Instructor instructor);

}
