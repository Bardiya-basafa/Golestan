namespace Golestan.Shared.Helpers;

using HelperModels;


public static class TermHelper {

    // public static TermResult? CurrentNormalTerm()
    // {
    //     var term = new TermResult();
    //     DateTime currentDate = DateTime.UtcNow;
    //
    //     DateTime firstTermStart = new DateTime(currentDate.Year, 9, 1, 0, 0, 0, DateTimeKind.Utc);
    //     DateTime firstTermEnd = new DateTime(currentDate.Year, 12, 31, 23, 59, 59, DateTimeKind.Utc);
    //     DateTime secondTermStart = new DateTime(currentDate.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    //     DateTime secondTermEnd = new DateTime(currentDate.Year, 5, 31, 23, 59, 59, DateTimeKind.Utc);
    //     var year = currentDate.Year;
    //     string termNumber = "";
    //
    //     if (currentDate >= firstTermStart && currentDate <= firstTermEnd){
    //         termNumber = "1";
    //         term.IsFirstTerm = true;
    //         term.StartDate = firstTermStart;
    //         term.EndDate = firstTermEnd;
    //     }
    //     else if (currentDate >= secondTermStart && currentDate <= secondTermEnd){
    //         termNumber = "2";
    //         term.IsFirstTerm = false;
    //         term.StartDate = secondTermStart;
    //         term.EndDate = secondTermEnd;
    //     }
    //     else{
    //         return null;
    //     }
    //
    //     term.TermNumber = termNumber;
    //     term.Year = year;
    //
    //
    //     return term;
    // }

    // public static bool IsInsideTerms()
    // {
    //     DateTime currentDate = DateTime.UtcNow;
    //
    //     DateTime firstTermStart = new DateTime(currentDate.Year, 9, 1, 0, 0, 0, DateTimeKind.Utc);
    //     DateTime firstTermEnd = new DateTime(currentDate.Year, 12, 31, 23, 59, 59, DateTimeKind.Utc);
    //     DateTime secondTermStart = new DateTime(currentDate.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    //     DateTime secondTermEnd = new DateTime(currentDate.Year, 5, 31, 23, 59, 59, DateTimeKind.Utc);
    //
    //     if (currentDate >= firstTermStart && currentDate <= firstTermEnd){
    //         return true;
    //     }
    //
    //     if (currentDate >= secondTermStart && currentDate <= secondTermEnd){
    //         return true;
    //     }
    //
    //     return false;
    // }

    public static Result IsExamDatesValid(DateTime examStartDate, DateTime examEndDate)
    {
        var result = new Result();
        var examTime = examEndDate - examStartDate;

        if (examTime <= TimeSpan.FromDays(20)){
            result.Message = "Time between exam start and exam end dates are too small";

            return result;
        }


        if (examEndDate <= examStartDate){
            result.Message = "Exam start date cannot be greater than exam end date";

            return result;
        }

        var currentDate = DateTime.Now;

        if (examStartDate <= currentDate || examEndDate <= currentDate){
            result.Message = "Exam should be start in the future";

            return result;
        }


        result.Succeeded = true;

        return result;
    }

    // public static Result IsTermDatesValid(DateTime? termStartDate, DateTime? termEndDate)
    // {
    //     var result = new Result();
    //
    //     if (termEndDate <= termStartDate){
    //         result.Message = "Start date should be after end date";
    //
    //         return result;
    //     }
    //
    //
    //     if (termEndDate - termStartDate <= TimeSpan.FromDays(120)){
    //         result.Message = "Term time span too small";
    //
    //         return result;
    //     }
    //
    //     result.Succeeded = true;
    //
    //     return result;
    // }


    public static Result IsTermSelectionTimeValid(DateTime startDate, DateTime endDate, DateTime examStartDate)
    {
        var result = new Result();

        if (endDate <= startDate){
            result.Message = "Start date must be before end date";

            return result;
        }

        if (endDate - startDate >= TimeSpan.FromDays(10)){
            result.Message = "Selection time span must be under 10 days";

            return result;
        }

        if (endDate - startDate <= TimeSpan.FromDays(2)){
            result.Message = "Selection time span must be more than 2 days";

            return result;
        }


        if (endDate >= examStartDate){
            result.Message = "Selection must start before exams";

            return result;
        }

        var currentDate = DateTime.Now;

        if (endDate <= currentDate || startDate <= currentDate){
            result.Message = "Start date cannot be greater than current date";

            return result;
        }

        result.Succeeded = true;

        return result;
    }

}
