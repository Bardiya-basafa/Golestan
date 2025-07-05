namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs.Term;
using Shared.Helpers;


public interface ITermService {

    Task<bool> IsInsideAnyTerm();
    Task<TermDto?> GetCurrentTerm();

    Task<Term?> GetCurrentTermEntity();

    Task<List<TermDto>> GetAllTerms();

    Task<bool> TermOpeningOption();

    Task<Result> OpenNewTerm(OpenNewTermDto dto);

    Task<Result> CloseTerm(string confirmation, string currentTerm);

    Task<bool> IsInsideAnyTermCurrently();

}
