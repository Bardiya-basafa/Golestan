namespace Golestan.Application.Validations;

using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


public class UniqueEmail : ValidationAttribute {

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        var userManager = context.GetService<UserManager<AppUser>>();
        var email = value?.ToString();


        var existingUser = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();


        if (existingUser != null){
            return new ValidationResult(ErrorMessage ?? "Email address already exists");
        }

        return ValidationResult.Success;
    }

}
