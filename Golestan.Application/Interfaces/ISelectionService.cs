namespace Golestan.Application.Interfaces;

using DTOs.Section;
using DTOs.Selection;
using Shared.Helpers;


public interface ISelectionService {

    Task<SelectionDto> GetSelectionDto(string studentId);

    Task<List<SectionDto>?> GetAvailableSectionsForSelection(string studentId);

    Task<SelectionInfoDto> GetSelectionInfo(string studentId);

    Task<List<SectionDto>> GetSelectedSections(string studentId);

    Task<Result> SelectSection(string studentId, int sectionId);

    Task<Result> UnselectSection(string studentId, int sectionId);

}
