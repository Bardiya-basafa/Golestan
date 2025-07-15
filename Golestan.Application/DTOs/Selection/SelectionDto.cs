namespace Golestan.Application.DTOs.Selection;

using Section;
using Shared.Helpers;


public class SelectionDto {

    public string Term { get; set; }

    public int studentId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Result Result { get; set; }

    public List<SectionDto> AvailableSections { get; set; }

    public List<SectionDto> SelectedSections { get; set; }

}
