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

   

    public async Task<StudentDto> GetStudentInfo(string studentId)
    {
        
        return await studentRepository.GetStudentInfo(studentId);
    }

    public async Task<List<StudentDto>> GetFacultyStudents(int facultyId)
    {
        return await studentRepository.GetFacultyStudents(facultyId);
    }

    public async Task<StudentDto> GetStudentSections(string studentId)
    {
        return await studentRepository.GetStudentDtoById(studentId);
    }

    public async Task<List<TermDto>> GetAllStudentTerms(string studentId)
    {
        return await studentRepository.GetAllStudentTerms(studentId);
    }

   

    public async Task<List<ExamResultDto>?> GetActiveExamResults(string studentId)
    {
        var currentTerm = await termService.GetCurrentTermEntity();

        if (currentTerm == null){
            return null;
        }

        return await studentRepository.GetAllTermExamResults(studentId, currentTerm.Id);
    }

  

    public async Task<Result> Rmove(int studentId)
    {
        return await studentRepository.RemoveStudent(studentId);
    }

}
