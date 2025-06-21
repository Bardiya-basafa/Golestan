namespace Golestan.Application.DTOs.Term;

using System.ComponentModel.DataAnnotations;


public class OpenNewTermDto {

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]

    public DateTime ExamsStartTime { get; set; }

    [Required]

    public DateTime ExamsEndTime { get; set; }

    [Required]
    public DateTime SectionSelectionStartTime { get; set; }

    [Required]
    public DateTime SectionSelectionEndTime { get; set; }

}
