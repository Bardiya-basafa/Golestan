namespace Golestan.Application.DTOs.Selection;

using Term;


public class SelectionInfoDto {

    public string FullName { get; set; }

    public string StudentNumber { get; set; }

    public TermDto Term { get; set; }

    public decimal Gpa { get; set; }

    public int TotalUnit { get; set; }

}
