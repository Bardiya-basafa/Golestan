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

    public Task<bool> IsInsideAnyTerm()
    {
        var result = _context.Terms
            .Where(t => t.EndTime > DateTime.Now)
            .AnyAsync();
        return result;
    }

    public async Task<TermDto?> GetCurrentTerm()
    {
        var currentDate = DateTime.UtcNow;

        var term = await _context.Terms
            .Where(t => t.EndTime > currentDate)
            .Select(t => new TermDto()
            {
                EndTime = t.EndTime,
                StartTime = t.StartTime,
                ExamsStartTime = t.ExamsStartTime,
                ExamsEndTime = t.ExamsEndTime,
                SelectionStartTime = t.SectionSelectionStartTime,
                SelectionEndTime = t.SectionSelectionEndTime,
                Year = t.Year,
                TermNumber = t.TermNumber,
                Id = t.Id
            })
            .FirstOrDefaultAsync();

        return term;
    }

    public async Task<Term?> GetCurrentTermEntity()
    {
        var currentDate = DateTime.UtcNow;

        var term = await _context.Terms
            .FirstOrDefaultAsync(t => t.EndTime > currentDate);

        return term;
    }

    public Task<List<TermDto>> GetAllTerms()
    {
        var terms = _context.Terms
            .Select(t => new TermDto()
            {
                EndTime = t.EndTime,
                StartTime = t.StartTime,
                ExamsStartTime = t.ExamsStartTime,
                ExamsEndTime = t.ExamsEndTime,
                Year = t.Year,
                TermNumber = t.TermNumber,
            })
            .ToListAsync();

        return terms;
    }

    public async Task<bool> TermOpeningOption()
    {
        var currentDate = DateTime.UtcNow;
        var currentlyTermExists = await _context.Terms.AnyAsync(t => t.EndTime > currentDate);

        if (currentlyTermExists){
            return false;
        }

        return true;
    }


    public async Task<Result> OpenNewTerm(OpenNewTermDto dto)
    {
        var result = new Result();

        var currentDate = DateTime.UtcNow;


        var termExist = await _context.Terms
            .FirstOrDefaultAsync(t => t.EndTime > currentDate);


        if (termExist != null){
            result.Message = "Term already opened.";

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

        var isSelectionTimesValid = TermHelper.IsTermSelectionTimeValid(dto.SectionSelectionStartTime, dto.SectionSelectionEndTime, dto.StartDate, dto.EndDate, dto.ExamsStartTime);

        if (!isSelectionTimesValid.Succeeded){
            result.Message = isSelectionTimesValid.Message;

            return result;
        }

        var costumeTermProperties = TermHelper.GetTermProperties(dto.StartDate, dto.EndDate);

        var term = new Term()
        {
            EndTime = dto.EndDate,
            StartTime = dto.StartDate,
            ExamsEndTime = dto.ExamsEndTime,
            ExamsStartTime = dto.ExamsStartTime,
            Year = costumeTermProperties.Year,
            TermNumber = costumeTermProperties.TermNumber,
            IsFirstTerm = costumeTermProperties.IsFirstTerm,
            SectionSelectionStartTime = dto.SectionSelectionStartTime,
            SectionSelectionEndTime = dto.SectionSelectionEndTime,
        };

        _context.Terms.Add(term);
        var students = await _context.Students.Include(s => s.Terms).ToListAsync();

        students.ForEach(student => {
            student.Terms.Add(term);
            _context.Students.Update(student);
        });

        await _context.SaveChangesAsync();
        result.Message = "Term opened.";
        result.Succeeded = true;

        return result;
    }

    public async Task<Result> CloseTerm(string confirmation, string currentTerm)
    {
        var result = new Result();

        if (confirmation.ToLower() != $"i am sure to close the {currentTerm} term with no turing back"){
            result.Message = "Please provide confirmation to continue.";

            return result;
        }

        var currentDate = DateTime.UtcNow;

        var term = await _context.Terms
            .FirstOrDefaultAsync(t => t.StartTime <= currentDate && t.EndTime >= currentDate);

        if (term == null){
            result.Message = "Term could not be found.";

            return result;
        }

        if (currentDate <= term.ExamsEndTime){
            term.ExamSuspended = true;
        }

        if (currentDate < term.EndTime){
            term.EndTime = currentDate;
        }

        _context.Terms.Update(term);
        await _context.SaveChangesAsync();
        result.Message = "Term closed.";
        result.Succeeded = true;

        return result;
    }

    public async Task<bool> IsInsideAnyTermCurrently()
    {
        var currentDate = DateTime.UtcNow;
        var isInsideTerm = await _context.Terms.AnyAsync(t => t.EndTime > currentDate);

        return isInsideTerm;
    }

}
