namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using DTOs.Exam;
using DTOs.ExamResult;
using Shared.Helpers;


public interface IExamRepository {

    Task<Result> SubmitExamResult(ExamResult examResult);

    Task<List<ExamResultDto>> GetSectionExamResults(int sectionId);

    Task<List<ExamResultDto>> GetTermExamResults(int termId, int studentId);

    Task<Result> SubmitObjection(ExamResult examResult, string objection);


    Task<ExamDto> GetExamInfo(int sectionId);

}
