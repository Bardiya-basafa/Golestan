namespace Golestan.Application.Validations;

using System;
using System.ComponentModel.DataAnnotations;


public class NotInFuture : ValidationAttribute {

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime hireDate){
            if (hireDate > DateTime.Now){
                return new ValidationResult(ErrorMessage ?? "Hire date cannot be in the future.");
            }
        }

        return ValidationResult.Success;
    }

    
}
