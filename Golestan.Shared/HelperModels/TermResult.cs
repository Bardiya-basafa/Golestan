namespace Golestan.Shared.HelperModels;

public class TermResult {

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Year { get; set; }

    public string TermNumber { get; set; }

    public bool IsFirstTerm { get; set; }

    public string TermText => Year.ToString() + "/" + TermNumber;

}
