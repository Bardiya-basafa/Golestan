namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs.Term;
using Shared.Helpers;


public interface ITermService {

    Task<TermDto?> GetCurrentTerm();

    Task<Term?> GetLastTerm();

    Task<TermDto> GetTermById(int termId);

    Task<Term?> GetCurrentTermEntity();

    Task<List<TermDto>> GetAllTerms();

    Task<Result> EditTerm(TermDto model);

    Task<Result> OpenNewTerm(OpenNewTermDto model);

    Task<Result> CloseTerm(string confirmation, string currentTerm);

    Task<bool> IsInsideAnyTermCurrently();

}
