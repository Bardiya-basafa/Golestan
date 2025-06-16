namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs.Instructor;
using Shared.Helpers;


public interface IInstructorService {

    Task<List<InstructorDetailsDto>> GetFacultyInstructors();
    Task<Result> RemoveCourseInstructor(int instructorId,int courseId);

}
