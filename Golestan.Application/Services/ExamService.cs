namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.ExamResult;
using DTOs.Score;
using Interfaces;
using Mapster;
using RepositoryInterfaces;
using Shared.Helpers;


public class ExamService(IExamRepository examRepository) : IExamService {

    public async Task<Result> SubmitStudentScore(ScoreDto model)
    {
        if (model.Score < 0 || model.Score > 20){
            return new Result()
            {
                Message = "Score must be between 0 and 20"
            };
        }

        return await examRepository.SubmitExamResult(model.Adapt<ExamResult>());
    }

    public async Task<List<ExamResultDto>> GetSectionExamResults(int sectionId)
    {
        return await examRepository.GetSectionExamResults(sectionId);
    }

}
