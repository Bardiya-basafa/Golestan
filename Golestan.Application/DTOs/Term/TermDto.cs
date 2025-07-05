namespace Golestan.Application.DTOs.Term;

public class TermDto {

    public int Id { get; set; }

    public int Year { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime ExamsStartTime { get; set; }

    public DateTime ExamsEndTime { get; set; }

    public DateTime SelectionStartTime { get; set; }

    public DateTime SelectionEndTime { get; set; }

    public string TermNumber { get; set; }

    public bool IsFirstTerm { get; set; }

    public string TermText => Year.ToString() + "/" + TermNumber;

}
