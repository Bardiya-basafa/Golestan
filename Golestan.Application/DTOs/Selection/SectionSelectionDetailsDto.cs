namespace Golestan.Application.DTOs.Selection;

using Shared.Helpers;


public class SectionSelectionDetailsDto {

    public string Term { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Result Result { get; set; }

}
