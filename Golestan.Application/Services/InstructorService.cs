namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.ExamResult;
using DTOs.Instructor;
using DTOs.Score;
using DTOs.Student;
using Interfaces;
using Mapster;
using RepositoryInterfaces;
using Shared.Helpers;


public class InstructorService(IInstructorRepository instructorRepository) : IInstructorService {

    public async Task<InstructorDto> GetInstructorAppUser(string instructorId)
    {
        return await instructorRepository.GetInstructorAppUser(instructorId);
    }

    public async Task<InstructorDto> GetInstructorInfo(int instructorId)
    {
        return await instructorRepository.GetInstructorInfo(instructorId);
    }

    public async Task<List<InstructorDto>> GetFacultyInstructors()
    {
        return await instructorRepository.GetFacultyInstructors();
    }


    public async Task<InstructorDto> GetInstructorDtoById(int instructorId)
    {
        return await instructorRepository.GetInstructorDtoById(instructorId);
    }


    public async Task<List<StudentDto>> GetInstructorStudentsOfSection(int sectionId)
    {
        return await instructorRepository.GetInstructorStudentsOfSection(sectionId);
    }

    public async Task<List<ExamResultDto>> GetExamResultsOfSection(int sectionId)
    {
        return await instructorRepository.GetExamResultsOfSection(sectionId);
    }

  


    public async Task<Result> RemoveCourseInstructor(int instructorId, int courseId)
    {
        return await instructorRepository.RemoveCourseInstructor(instructorId, courseId);
    }

    public async Task<Result> RemoveInstructor(int instructorId)
    {
        return await instructorRepository.RemoveInstructor(instructorId);
    }

}
