namespace Golestan.Application.Interfaces;

using DTOs.Section;
using DTOs.Selection;
using Shared.Helpers;


public interface ISelectionService {

    Task<SelectionDto> GetSelectionDto(int studentId);

    Task<List<SectionDto>?> GetAvailableSectionsForSelection(int studentId);

    Task<SelectionInfoDto> GetSelectionInfo(int studentId);

    Task<List<SectionDto>> GetSelectedSections(int studentId);

    Task<Result> SelectSection(int studentId, int sectionId);

    Task<Result> UnselectSection(int studentId, int sectionId);

}
