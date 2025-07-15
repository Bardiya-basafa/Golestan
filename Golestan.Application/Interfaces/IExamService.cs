namespace Golestan.Application.Interfaces;

using DTOs.Exam;
using DTOs.ExamResult;
using DTOs.Score;
using DTOs.Student;
using Shared.Helpers;


public interface IExamService {

    Task<Result> SubmitStudentScore(ScoreDto model);

    Task<List<ExamResultDto>> GetSectionExamResults(int sectionId);

    Task<ExamDto> GetExamInfo(int sectionId);

}
