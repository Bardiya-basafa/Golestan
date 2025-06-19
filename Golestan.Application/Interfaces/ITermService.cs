namespace Golestan.Application.Interfaces;

using DTOs.Term;
using Shared.Helpers;


public interface ITermService {

    Task<Result> OpenNewNormalTerm(OpenNewTermDto dto);

    Task<Result> OpenCostumeTerm(OpenNewTermDto dto);

}
