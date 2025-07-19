namespace Golestan.Application.Interfaces;

using DTOs.Exam;
using DTOs.ExamResult;
using DTOs.Objection;
using DTOs.Score;
using DTOs.Student;
using Shared.Helpers;


public interface IExamService {

    Task<Result> SubmitStudentScore(ScoreDto model);

    Task<List<ExamResultDto>> GetSectionExamResults(int sectionId);

    Task<List<ExamResultDto>> GetTermExamResults(int termId, int studentId);

    Task<Result> SubmitObjection(ObjectionDto model);


    Task<ExamDto> GetExamInfo(int sectionId);

}
