namespace Golestan.Application.Interfaces;

using DTOs.Exam;
using DTOs.ExamResult;
using DTOs.Objection;
using DTOs.Score;
using DTOs.Student;
using Shared.Helpers;


public interface IExamService {

    Task<Result> SubmitStudentScore(ScoreDto model,string instructorId);

    Task<List<ExamResultDto>> GetSectionExamResults(int sectionId,string userId);

    Task<List<ExamResultDto>> GetTermExamResults(int termId, string studentId);
    Task<List<ExamResultDto>> GetTermFinalResults(int termId, string instructorId);

    Task<Result> SubmitObjection(ObjectionDto model,string studentId);


    Task<ExamDto> GetExamInfo(int sectionId);

}
