namespace Golestan.Domain.Entities;

public class Term {

    public int Id { get; set; }

    public int Year { get; set; }

    // public DateTime StartTime { get; set; }
    //
    // public DateTime EndTime { get; set; }

    public DateTime ExamsStartTime { get; set; }

    public DateTime ExamsEndTime { get; set; }

    public DateTime SectionSelectionStartTime { get; set; }

    public DateTime SectionSelectionEndTime { get; set; }


    public string TermIdentifier { get; set; }

    public bool ExamSuspended { get; set; }


    public bool IsClosed { get; set; } = false;

}
