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

    public async Task<TermDetailsDto?> GetCurrentTerm()
    {
        var currentDate = DateTime.UtcNow;

        var term = await _context.Terms
            .Where(t => t.StartTime <= currentDate && t.EndTime >= currentDate)
            .Select(t => new TermDetailsDto()
            {
                EndTime = t.EndTime,
                StartTime = t.StartTime,
                ExamsStartTime = t.ExamsStartTime,
                ExamsEndTime = t.ExamsEndTime,
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
            .Where(t => t.StartTime <= currentDate && t.EndTime >= currentDate && t.IsActive)
            .FirstOrDefaultAsync();

        return term;
    }

    public Task<List<TermDetailsDto>> GetAllTerms()
    {
        var terms = _context.Terms
            .Select(t => new TermDetailsDto()
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

    public async Task<TermOpeningOptionsDto> TermOpeningOptions()
    {
        var dto = new TermOpeningOptionsDto();
        var currentDate = DateTime.UtcNow;
        var currentlyTermExists = await _context.Terms.AnyAsync(t => t.StartTime <= currentDate && t.EndTime >= currentDate);

        if (currentlyTermExists){
            dto.CanOpenAnyTermNow = false;

            return dto;
        }

        var isInsideNormalTerm = TermHelper.CurrentNormalTerm();

        if (isInsideNormalTerm == null){
            dto.CanOpenNormalTermNow = true;
        }

        dto.CanOpenNormalTermNow = false;

        return dto;
    }

    public async Task<Result> OpenNormalTerm(OpenNewTermDto dto)
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

        var termHelper = TermHelper.CurrentNormalTerm();

        if (termHelper == null){
            result.Message = "Term could not be opened.";

            return result;
        }

        var isExamDatesValid = TermHelper.IsExamDatesValid(termHelper.StartDate, termHelper.EndDate, dto.ExamsStartTime, dto.ExamsEndTime);

        if (!isExamDatesValid.Succeeded){
            result.Message = isExamDatesValid.Message;

            return result;
        }

        var isSelectionTimesValid = TermHelper.IsTermSelectionTimeValid(dto.SectionSelectionStartTime, dto.SectionSelectionEndTime, dto.ExamsStartTime, dto.ExamsEndTime);

        if (!isSelectionTimesValid.Succeeded){
            result.Message = isSelectionTimesValid.Message;

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

    public async Task<Result> OpenCostumeTerm(OpenNewTermDto dto)
    {
        var result = new Result();

        var currentDate = DateTime.UtcNow;

        if (currentDate < dto.StartDate || currentDate > dto.EndDate){
            result.Message = "Current date must be between start and end date.";

            return result;
        }

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

        var isSelectionTimesValid = TermHelper.IsTermSelectionTimeValid(dto.SectionSelectionStartTime, dto.SectionSelectionEndTime, dto.ExamsStartTime, dto.ExamsEndTime);

        if (!isSelectionTimesValid.Succeeded){
            result.Message = isSelectionTimesValid.Message;

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

        term.IsActive = false;
        _context.Terms.Update(term);
        await _context.SaveChangesAsync();
        result.Message = "Term closed.";
        result.Succeeded = true;

        return result;
    }

    public async Task<bool> IsInsideAnyTermCurrently()
    {
        var currentDate = DateTime.UtcNow;
        var isInsideTerm = await _context.Terms.AnyAsync(t => t.StartTime <= currentDate && t.EndTime >= currentDate && t.IsActive);

        return isInsideTerm;
    }

}
