namespace Golestan.Domain.Entities;

public class Term {

    public int Id { get; set; }

    public int Year { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime ExamsStartTime { get; set; }

    public DateTime ExamsEndTime { get; set; }
    public string TermNumber { get; set; }

    public bool IsFirstTerm { get; set; }

    public string TermText => Year.ToString() + "/" + TermNumber;

}
