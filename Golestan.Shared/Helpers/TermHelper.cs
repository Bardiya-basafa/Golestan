namespace Golestan.Shared.Helpers;



public static class TermHelper {

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


    public static Result IsTermSelectionTimeValid(DateTime selectionStartDate, DateTime selectionEndDate, DateTime examStartDate, bool editing = false)
    {
        var result = new Result();

        if (selectionEndDate <= selectionStartDate){
            result.Message = "Start date must be before end date";

            return result;
        }

        

        if (selectionEndDate - selectionStartDate <= TimeSpan.FromDays(2)){
            result.Message = "Selection time span must be more than 2 days";

            return result;
        }


        if (selectionEndDate >= examStartDate){
            result.Message = "Selection must start before exams";

            return result;
        }

        var currentDate = DateTime.Now;

        if (!editing){
            if (selectionStartDate <= currentDate){
                result.Message = "Selection start date must be after current date";

                return result;
            }
        }


        result.Succeeded = true;

        return result;
    }

}
