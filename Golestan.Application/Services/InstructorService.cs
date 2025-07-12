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

    public async Task<List<InstructorDto>> GetFacultyInstructors()
    {
        try{
            return await instructorRepository.GetFacultyInstructors();
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<InstructorDto> GetInstructorDtoById(int instructorId)
    {
        try{
            return await instructorRepository.GetInstructorDtoById(instructorId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }


    public async Task<List<StudentDto>> GetInstructorStudentsOfSection(int sectionId)
    {
        try{
            return await instructorRepository.GetInstructorStudentsOfSection(sectionId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<ExamResultDto>> GetExamResultsOfSection(int sectionId)
    {
        try{
            return await instructorRepository.GetExamResultsOfSection(sectionId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> SubmitStudentScore(ScoreDto model)
    {
        try{
            if (model.Score < 0 || model.Score > 20){
                return new Result()
                {
                    Message = "Score must be between 0 and 20"
                };
            }

            return await instructorRepository.SubmitExamResult(model.Adapt<ExamResult>());
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }


    public async Task<Result> RemoveCourseInstructor(int instructorId, int courseId)
    {
        try{
            return await instructorRepository.RemoveCourseInstructor(instructorId, courseId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> RemoveInstructor(int instructorId)
    {
        try{
            return await instructorRepository.RemoveInstructor(instructorId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

}
