namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs.ExamResult;
using DTOs.Instructor;
using DTOs.Score;
using DTOs.Student;
using Shared.Helpers;


public interface IInstructorService {

    Task<InstructorDto> GetInstructorAppUser(string instructorId);
    Task<InstructorDto> GetInstructorInfo(int instructorId);
    

    Task<List<InstructorDto>> GetFacultyInstructors();

    Task<InstructorDto> GetInstructorDtoById(int instructorId);

    Task<List<StudentDto>> GetInstructorStudentsOfSection(int sectionId);

    Task<List<ExamResultDto>> GetExamResultsOfSection(int sectionId);


    Task<Result> RemoveCourseInstructor(int instructorId, int courseId);

    Task<Result> RemoveInstructor(int instructorId);

}
