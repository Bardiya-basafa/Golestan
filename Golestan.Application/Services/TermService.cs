namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.Term;
using Interfaces;
using RepositoryInterfaces;
using Shared.Helpers;


public class TermService(ITermRepository termRepository) : ITermService {

    public async Task<TermDto?> GetCurrentTerm()
    {
        try{
            return await termRepository.GetCurrentTerm();
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Term?> GetCurrentTermEntity()
    {
        try{
            return await termRepository.GetCurrentTermEntity();
        }

        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<TermDto>> GetAllTerms()
    {
        try{
            return await termRepository.GetAllTerms();
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }


    public async Task<Result> OpenNewTerm(OpenNewTermDto dto)
    {
        var result = new Result();


        if (await termRepository.IsInsideAnyTermCurrently()){
            result.Message = "Term already opened.";

            return result;
        }


        // var isTermDatesValid = TermHelper.IsTermDatesValid(dto.StartDate, dto.EndDate);
        //
        // if (!isTermDatesValid.Succeeded){
        //     result.Message = isTermDatesValid.Message;
        //
        //     return result;
        // }

        var isExamDatesValid = TermHelper.IsExamDatesValid(dto.ExamsStartTime, dto.ExamsEndTime);

        if (!isExamDatesValid.Succeeded){
            result.Message = isExamDatesValid.Message;

            return result;
        }

        var isSelectionTimesValid = TermHelper.IsTermSelectionTimeValid(dto.SectionSelectionStartTime, dto.SectionSelectionEndTime, dto.ExamsStartTime);

        if (!isSelectionTimesValid.Succeeded){
            result.Message = isSelectionTimesValid.Message;

            return result;
        }

        if (!dto.TermIdentifier.Contains("/")){
            result.Message = "Term identifier must be in format (year)/(termnumber)";

            return result;
        }

        var term = new Term()
        {
            ExamsEndTime = dto.ExamsEndTime,
            ExamsStartTime = dto.ExamsStartTime,
            Year = DateTime.UtcNow.Year,
            SectionSelectionStartTime = dto.SectionSelectionStartTime,
            SectionSelectionEndTime = dto.SectionSelectionEndTime,
        };

        return await termRepository.AddTerm(term);
    }

    public async Task<Result> CloseTerm(string confirmation, string currentTerm)
    {
        var result = new Result();

        if (confirmation.ToLower() != $"i am sure to close the {currentTerm} term with no turing back"){
            result.Message = "Please provide confirmation to continue.";

            return result;
        }


        var term = await termRepository.GetCurrentTermEntity();

        if (term == null){
            result.Message = "We are inside a term you have to close it first";

            return result;
        }

        var currentDate = DateTime.UtcNow;

        if (currentDate <= term.ExamsEndTime){
            term.ExamSuspended = true;
        }

        term.IsClosed = true;

        return await termRepository.CloseTerm(term);
    }

    public async Task<bool> IsInsideAnyTermCurrently()
    {
        return await termRepository.IsInsideAnyTermCurrently();
    }

}
