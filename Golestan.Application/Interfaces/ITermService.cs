namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs.Term;
using Shared.Helpers;


public interface ITermService {

    Task<TermDetailsDto?> GetCurrentTerm();

    Task<Term?> GetCurrentTermEntity();

    Task<List<TermDetailsDto>> GetAllTerms();

    Task<TermOpeningOptionsDto> TermOpeningOptions();

    Task<Result> OpenNormalTerm(OpenNewTermDto dto);

    Task<Result> OpenCostumeTerm(OpenNewTermDto dto);

    Task<Result> CloseTerm(string confirmation, string currentTerm);

    Task<bool> IsInsideAnyTermCurrently();

}
