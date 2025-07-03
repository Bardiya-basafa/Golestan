namespace Golestan.Domain.Entities;

public class Term {

    public int Id { get; set; }

    public int Year { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime ExamsStartTime { get; set; }

    public DateTime ExamsEndTime { get; set; }

    public DateTime SectionSelectionStartTime { get; set; }

    public DateTime SectionSelectionEndTime { get; set; }

    public string TermNumber { get; set; }

    public bool IsFirstTerm { get; set; }

    public bool IsActive
    {
        get
        {
            if (EndTime <= DateTime.UtcNow){
                return false;
            }

            return IsActive;
        }

        set => IsActive = value;
    }

    public bool ExamSuspended { get; set; } = false;

    public string TermText => Year.ToString() + "/" + TermNumber;

}
