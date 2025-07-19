namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using DTOs.Term;
using Shared.Helpers;


public interface ITermRepository {

    Task<TermDto?> GetCurrentTerm();
    Task<Term?> GetLastTerm();
    Task<TermDto> GetTermById(int termId);

    Task<Term?> GetCurrentTermEntity();

    Task<List<TermDto>> GetAllTerms();

    Task<Result> AddTerm(Term term);

    Task<Result> EditTerm(Term term);

    Task<Result> CloseTerm(Term term);

    Task<bool> IsInsideAnyTermCurrently();

}
