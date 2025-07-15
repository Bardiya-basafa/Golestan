namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using DTOs.ExamResult;
using Shared.Helpers;


public interface IExamRepository {

    Task<Result> SubmitExamResult(ExamResult examResult);
    Task<List<ExamResultDto>> GetSectionExamResults(int sectionId);

}
