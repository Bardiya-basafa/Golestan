namespace Golestan.Application.Interfaces;

using DTOs.Section;
using DTOs.Selection;
using Shared.Helpers;


public interface ISelectionService {

    Task<SelectionDto> SelectionTermDetails();

    Task<List<SectionDto>> GetAvailableSectionsForSelection(int studentId);

    Task<List<SectionDto>> GetSelectedSections(int studentId);

    Task<Result> SelectSection(int studentId, int sectionId);
    Task<Result> UnselectSection(int studentId, int sectionId);

}
