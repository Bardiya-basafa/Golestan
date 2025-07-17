namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.Term;
using Interfaces;
using RepositoryInterfaces;
using Shared.Helpers;


public class TermService(ITermRepository termRepository) : ITermService {

    public async Task<TermDto?> GetCurrentTerm()
    {
        return await termRepository.GetCurrentTerm();
    }

    public async Task<Term?> GetLastTerm()
    {
        return await termRepository.GetLastTerm();
    }

    public async Task<Term?> GetCurrentTermEntity()
    {
        return await termRepository.GetCurrentTermEntity();
    }

    public async Task<List<TermDto>> GetAllTerms()
    {
        return await termRepository.GetAllTerms();
    }

    public async Task<Result> EditTerm(TermDto model)
    {
        var result = new Result();
        var currentDate = DateTime.UtcNow;
        var currentTerm = await GetCurrentTermEntity();

        if (currentTerm == null){
            result.Message = "No term available right now";

            return result;
        }

        var isExamDatesValid = TermHelper.IsExamDatesValid(model.ExamsStartTime, model.ExamsEndTime);

        if (!isExamDatesValid.Succeeded){
            result.Message = isExamDatesValid.Message;

            return result;
        }

        var isSelectionTimesValid = TermHelper.IsTermSelectionTimeValid(model.SelectionStartTime, model.SelectionEndTime, model.ExamsStartTime, editing: true);

        if (!isSelectionTimesValid.Succeeded){
            result.Message = isSelectionTimesValid.Message;

            return result;
        }

        if (!model.TermIdentifier.Contains("/") || string.IsNullOrEmpty(model.TermIdentifier)){
            result.Message = "Term identifier must be in format (year)/(termnumber)";

            return result;
        }

        if (string.IsNullOrEmpty(model.TermName)){
            result.Message = "Term name is required";

            return result;
        }


        if (model.Id != currentTerm.Id){
            result.Message = "Term IDs don't match";

            return result;
        }

        currentTerm.ExamsStartTime = model.ExamsStartTime;
        currentTerm.ExamsEndTime = model.ExamsEndTime;
        currentTerm.SectionSelectionStartTime = model.SelectionStartTime;
        currentTerm.SectionSelectionEndTime = model.SelectionEndTime;
        currentTerm.TermName = model.TermName;
        currentTerm.TermIdentifier = model.TermIdentifier;

        return await termRepository.EditTerm(currentTerm);
    }


    public async Task<Result> OpenNewTerm(OpenNewTermDto model)
    {
        var result = new Result();


        if (await termRepository.IsInsideAnyTermCurrently()){
            result.Message = "Term already opened.";

            return result;
        }


        var isExamDatesValid = TermHelper.IsExamDatesValid(model.ExamsStartTime, model.ExamsEndTime);

        if (!isExamDatesValid.Succeeded){
            result.Message = isExamDatesValid.Message;

            return result;
        }

        var isSelectionTimesValid = TermHelper.IsTermSelectionTimeValid(model.SectionSelectionStartTime, model.SectionSelectionEndTime, model.ExamsStartTime);

        if (!isSelectionTimesValid.Succeeded){
            result.Message = isSelectionTimesValid.Message;

            return result;
        }

        if (!model.TermIdentifier.Contains("/")){
            result.Message = "Term identifier must be in format (year)/(termnumber)";

            return result;
        }

        var term = new Term()
        {
            ExamsEndTime = model.ExamsEndTime,
            ExamsStartTime = model.ExamsStartTime,
            Year = DateTime.UtcNow.Year,
            StartTime = DateTime.UtcNow,
            SectionSelectionStartTime = model.SectionSelectionStartTime,
            SectionSelectionEndTime = model.SectionSelectionEndTime,
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
        term.EndTime = DateTime.UtcNow;

        return await termRepository.CloseTerm(term);
    }

    public async Task<bool> IsInsideAnyTermCurrently()
    {
        return await termRepository.IsInsideAnyTermCurrently();
    }

}
