namespace Golestan.Application.Validations;

using System.ComponentModel.DataAnnotations;


public class NotInPast : ValidationAttribute {

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime hireDate){
            if (hireDate <= DateTime.Now){
                return new ValidationResult(ErrorMessage ?? "Exam time must be in the future");
            }
        }

        return ValidationResult.Success;
    }

}
