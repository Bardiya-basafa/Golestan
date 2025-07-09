namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.ExamResult;
using DTOs.Objection;
using DTOs.Student;
using DTOs.Term;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using RepositoryInterfaces;
using Shared.Helpers;


public class StudentService(IStudentRepository studentRepository, UserManager<AppUser> userManager, ITermService termService) : IStudentService {

    private readonly UserManager<AppUser> _userManager = userManager;

    public async Task<List<StudentDto>> GetFacultyStudents(int facultyId)
    {
        try{
            return await studentRepository.GetFacultyStudents(facultyId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<StudentDto> GetStudentSections(int studentId)
    {
        try{
            return await studentRepository.GetStudentDtoById(studentId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<TermDto>> GetAllStudentTerms(int studentId)
    {
        try{
            return await studentRepository.GetAllStudentTerms(studentId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<ExamResultDto>> GetAllTermExamResults(int termId, int studentId)
    {
        try{
            return await studentRepository.GetAllTermExamResults(studentId, termId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<ExamResultDto>?> GetActiveExamResults(int studentId)
    {
        var currentTerm = await termService.GetCurrentTermEntity();

        if (currentTerm == null){
            return null;
        }

        return await studentRepository.GetAllTermExamResults(studentId, currentTerm.Id);
    }

    public async Task<Result> SubmitObjection(ObjectionDto model)
    {
        var result = new Result();
        var isInsideTerm = await termService.IsInsideAnyTermCurrently();

        if (!isInsideTerm){
            result.Message = "You cant submit an objection right now";

            return result;
        }

        if (model.Objection == string.Empty){
            result.Message = "You must provide an objection";

            return result;
        }

        var examResult = await studentRepository.GetExamResultForObjection(model);

        return await studentRepository.SubmitObjection(examResult, model.Objection);
    }

}
