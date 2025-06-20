namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs.Instructor;
using DTOs.Section;
using DTOs.Student;
using Shared.Helpers;


public interface IInstructorService {

    Task<List<InstructorDetailsDto>> GetFacultyInstructors();

    Task<List<SectionDetailsDto>> GetInstructorSections(int instructorId);

    Task<List<StudentDetailsDto>> GetInstructorStudentsForSection(int sectionId);

    Task<List<StudentScoreDto>> GetStudentsForCourseExam(int instructorId, int sectionId);

    Task<Result> SubmitStudentScore(SubmitScoreDto dto);

    Task<Result> RemoveCourseInstructor(int instructorId, int courseId);

    Task<Result> RemoveInstructor(int instructorId);

}
