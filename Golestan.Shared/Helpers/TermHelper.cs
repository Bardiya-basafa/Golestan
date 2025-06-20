namespace Golestan.Shared.Helpers;

using HelperModels;


public static class TermHelper {

    public static TermResult? CurrentNormalTerm()
    {
        var term = new TermResult();
        DateTime currentDate = DateTime.UtcNow;

        DateTime firstTermStart = new DateTime(currentDate.Year, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime firstTermEnd = new DateTime(currentDate.Year, 12, 31, 23, 59, 59, DateTimeKind.Utc);
        DateTime secondTermStart = new DateTime(currentDate.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime secondTermEnd = new DateTime(currentDate.Year, 5, 31, 23, 59, 59, DateTimeKind.Utc);
        var year = currentDate.Year;
        string termNumber = "";

        if (currentDate >= firstTermStart && currentDate <= firstTermEnd){
            termNumber = "1";
            term.IsFirstTerm = true;
            term.StartDate = firstTermStart;
            term.EndDate = firstTermEnd;
        }
        else if (currentDate >= secondTermStart && currentDate <= secondTermEnd){
            termNumber = "2";
            term.IsFirstTerm = false;
            term.StartDate = secondTermStart;
            term.EndDate = secondTermEnd;
        }
        else{
            return null;
        }

        term.TermNumber = termNumber;
        term.Year = year;


        return term;
    }

    public static bool IsInsideTerms()
    {
        DateTime currentDate = DateTime.UtcNow;

        DateTime firstTermStart = new DateTime(currentDate.Year, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime firstTermEnd = new DateTime(currentDate.Year, 12, 31, 23, 59, 59, DateTimeKind.Utc);
        DateTime secondTermStart = new DateTime(currentDate.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime secondTermEnd = new DateTime(currentDate.Year, 5, 31, 23, 59, 59, DateTimeKind.Utc);

        if (currentDate >= firstTermStart && currentDate <= firstTermEnd){
            return true;
        }

        if (currentDate >= secondTermStart && currentDate <= secondTermEnd){
            return true;
        }

        return false;
    }

    public static Result IsExamDatesValid(DateTime? termStartDate, DateTime? termEndDate, DateTime examStartDate, DateTime examEndDate)
    {
        var result = new Result();
        var examTime = examEndDate - examStartDate;

        if (examTime <= TimeSpan.FromDays(25)){
            result.Message = "Time between exam start and exam end dates are too small";

            return result;
        }

        if (examTime >= TimeSpan.FromDays(30)){
            result.Message = "Time between exam start and exam end dates are too large";

            return result;
        }

        if (examStartDate >= termEndDate || examStartDate <= termStartDate || examEndDate - examStartDate >= TimeSpan.FromDays(100) || examEndDate - examStartDate <= TimeSpan.FromDays(85)){
            result.Message = "Exam start date is not valid";

            return result;
        }

        if (examEndDate <= termEndDate || examEndDate >= examStartDate || termEndDate - examEndDate >= TimeSpan.FromDays(10) || termEndDate - examEndDate <= TimeSpan.FromDays(20)){
            result.Message = "Exam end date is not valid";

            return result;
        }

        result.Succeeded = true;

        return result;
    }

    public static Result IsTermDatesValid(DateTime? termStartDate, DateTime? termEndDate)
    {
        var result = new Result();

        if (termEndDate <= termStartDate){
            result.Message = "Start date is not valid";

            return result;
        }

        if (termEndDate - termStartDate >= TimeSpan.FromDays(135)){
            result.Message = "Term time span too large";

            return result;
        }

        if (termEndDate - termStartDate <= TimeSpan.FromDays(120)){
            result.Message = "Term time span too small";

            return result;
        }

        result.Succeeded = true;

        return result;
    }

    public static TermResult GetCostumeTermProperties(DateTime startDate, DateTime endDate)
    {
        var termResult = new TermResult();
        var currentDate = DateTime.UtcNow;
        DateTime secondTermStart = new DateTime(currentDate.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        if (secondTermStart <= startDate){
            termResult.IsFirstTerm = false;
            termResult.TermNumber = "2";
            termResult.Year = startDate.Year;
        }
        else{
            termResult.IsFirstTerm = true;
            termResult.TermNumber = "1";
            termResult.Year = startDate.Year;
        }

        return termResult;
    }

}
