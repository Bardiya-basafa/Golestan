namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.Exam;
using DTOs.ExamResult;
using DTOs.Objection;
using DTOs.Score;
using Interfaces;
using Mapster;
using RepositoryInterfaces;
using Shared.Helpers;


public class ExamService(IExamRepository examRepository, ITermService termService, IStudentRepository studentRepository) : IExamService {

    public async Task<Result> SubmitStudentScore(ScoreDto model, string instructorId)
    {
        if (model.Score < 0 || model.Score > 20){
            return new Result()
            {
                Message = "Score must be between 0 and 20"
            };
        }


        return await examRepository.SubmitExamResult(model.Adapt<ExamResult>(), instructorId);
    }

    public async Task<List<ExamResultDto>> GetSectionExamResults(int sectionId, string userId)
    {
        return await examRepository.GetSectionExamResults(sectionId, userId);
    }

    public async Task<List<ExamResultDto>> GetTermExamResults(int termId, string studentId)
    {
        return await examRepository.GetTermExamResults(termId, studentId);
    }

    public async Task<List<ExamResultDto>> GetTermFinalResults(int termId, string instructorId)
    {
        return await examRepository.GetTermFinalResults(termId, instructorId);
    }


    public async Task<Result> SubmitObjection(ObjectionDto model, string studentId)
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

        var examResult = await studentRepository.GetExamResultForObjection(model, studentId);

        return await examRepository.SubmitObjection(examResult, model.Objection);
    }


    public async Task<ExamDto> GetExamInfo(int sectionId)
    {
        return await examRepository.GetExamInfo(sectionId);
    }

}
