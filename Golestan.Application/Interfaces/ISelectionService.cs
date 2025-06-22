namespace Golestan.Application.Interfaces;

using DTOs.Section;
using DTOs.Selection;
using Shared.Helpers;


public interface ISelectionService {

    Task<SectionSelectionDetailsDto> SelectionTermDetails();

    Task<List<AvailableSectionsDto>?> GetAvailableSectionsForSelection(int studentId);

    Task<List<SectionDetailsDto>> GetSelectedSections(int studentId);

    Task<Result> SelectSection(int studentId, int sectionId);
    Task<Result> UnselectSection(int studentId, int sectionId);

}
