namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using DTOs.Exam;
using DTOs.ExamResult;
using Shared.Helpers;


public interface IExamRepository {

    Task<Result> SubmitExamResult(ExamResult examResult,string userId);

    Task<List<ExamResultDto>> GetSectionExamResults(int sectionId,string userId);

    Task<List<ExamResultDto>> GetTermExamResults(int termId, string studentId);

    Task<List<ExamResultDto>> GetTermFinalResults(int termId, string instructorId);

    Task<Result> SubmitObjection(ExamResult examResult, string objection);


    Task<ExamDto> GetExamInfo(int sectionId);

}
