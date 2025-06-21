namespace Golestan.Application.Interfaces;

using DTOs.Selection;


public interface ISelectionService {

    Task<SelectionDetailsDto> SelectionTermDetails();

    Task<List<AvailableSectionsDto>?> GetAvailableSectionsForSelection(int studentId);

}
