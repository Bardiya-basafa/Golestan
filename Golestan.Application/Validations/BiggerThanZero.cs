namespace Golestan.Application.Validations;

using System.ComponentModel.DataAnnotations;


public class BiggerThanZero : ValidationAttribute {

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is int salary){
            if (salary <= 100){
                return new ValidationResult(ErrorMessage ?? "Salary must be greater than 100");
            }
        }

        return ValidationResult.Success;
    }

}
