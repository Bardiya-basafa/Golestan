namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.Term;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;


public class TermService : ITermService {

    private readonly AppDbContext _context;

    public TermService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> OpenNewNormalTerm(OpenNewTermDto dto)
    {
        var result = new Result();
        var currentDate = DateTime.UtcNow;

        if (TermHelper.IsInsideTerms()){
            result.Message = "You can open a normal term now, please add a costume term";

            return result;
        }

        var termExist = await _context.Terms
            .FirstOrDefaultAsync(t => t.StartTime <= currentDate && t.EndTime >= currentDate);

        if (termExist != null){
            result.Message = "Term already opened.";

            return result;
        }

        var termHelper = TermHelper.CurrentTerm();

        if (termHelper == null){
            result.Message = "Term could not be opened.";

            return result;
        }

        var isExamDatesValid = TermHelper.IsExamDatesValid(termHelper.StartDate, termHelper.EndDate, dto.ExamsStartTime, dto.ExamsEndTime);

        if (!isExamDatesValid.Succeeded){
            result.Message = isExamDatesValid.Message;

            return result;
        }

        var term = new Term()
        {
            StartTime = termHelper.StartDate,
            EndTime = termHelper.EndDate,
            ExamsEndTime = dto.ExamsEndTime,
            ExamsStartTime = dto.ExamsStartTime,
            IsFirstTerm = termHelper.IsFirstTerm,
            TermNumber = termHelper.TermNumber,
            Year = termHelper.Year,
        };


        _context.Terms.Add(term);
        await _context.SaveChangesAsync();
        result.Message = "Term opened.";
        result.Succeeded = true;

        return result;
    }

    public async Task<Result> OpenCostumeTerm(OpenNewTermDto dto)
    {
        var result = new Result();

        var currentDate = DateTime.UtcNow;

        var termExist = await _context.Terms
            .FirstOrDefaultAsync(t => t.StartTime <= currentDate && t.EndTime >= currentDate);

        if (termExist != null){
            result.Message = "Term already opened.";

            return result;
        }

        if (dto.StartDate == null || dto.EndDate == null){
            result.Message = "Please provide start and end time.";

            return result;
        }

        if (!TermHelper.IsInsideTerms()){
            result.Message = "You cant open a costume term now, please add a normal first then edit it";

            return result;
        }

        var isTermDatesValid = TermHelper.IsTermDatesValid(dto.StartDate, dto.EndDate);

        if (!isTermDatesValid.Succeeded){
            result.Message = isTermDatesValid.Message;

            return result;
        }

        var isExamDatesValid = TermHelper.IsExamDatesValid(dto.StartDate, dto.EndDate, dto.ExamsStartTime, dto.ExamsEndTime);

        if (!isExamDatesValid.Succeeded){
            result.Message = isExamDatesValid.Message;

            return result;
        }

        var costumeTermProperties = TermHelper.GetCostumeTermProperties(dto.StartDate.Value, dto.EndDate.Value);

        var term = new Term()
        {
            EndTime = dto.EndDate.Value,
            StartTime = dto.StartDate.Value,
            ExamsEndTime = dto.ExamsEndTime,
            ExamsStartTime = dto.ExamsStartTime,
            Year = costumeTermProperties.Year,
            TermNumber = costumeTermProperties.TermNumber,
            IsFirstTerm = costumeTermProperties.IsFirstTerm,
        };

        _context.Terms.Add(term);
        await _context.SaveChangesAsync();
        result.Message = "Term opened.";
        result.Succeeded = true;

        return result;
    }

}
