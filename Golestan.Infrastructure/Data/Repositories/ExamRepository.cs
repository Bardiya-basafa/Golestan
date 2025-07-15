namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.ExamResult;
using Application.DTOs.Student;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class ExamRepository(AppDbContext context) : IExamRepository {

    public async Task<Result> SubmitExamResult(ExamResult examResult)
    {
        var updateExamResult = await context.ExamResults
            .Where(e => e.StudentId == examResult.StudentId && e.SectionId == examResult.SectionId)
            .FirstOrDefaultAsync() ?? throw new Exception($"No exam result found for student  id:{examResult.StudentId}");


        examResult.Score = examResult.Score;
        examResult.Description = examResult.Description;
        context.Update(updateExamResult);
        await context.SaveChangesAsync();

        return new Result()
        {
            Message = "Exam result has been saved",
            Succeeded = true
        };
    }

    public async Task<List<ExamResultDto>> GetSectionExamResults(int sectionId)
    {
        return await context.ExamResults
            .AsNoTracking()
            .Where(r => r.SectionId == sectionId)
            .Select(r => new ExamResultDto()
            {
                Student = new StudentDto()
                {
                    Id = r.StudentId,
                    FullName = r.Student.FullName,
                    StudentNumber = r.Student.StudentNumber,
                },
                Score = r.Score,
                Objection = r.Objection,
                Description = r.Description,
            })
            .ToListAsync();
    }

}
