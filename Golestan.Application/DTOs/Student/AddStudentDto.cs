namespace Golestan.Application.DTOs.Student;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Validations;


public class AddStudentDto {

    [Required(ErrorMessage = "You must provide a first name")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "The field must contain only letters")]

    public string FirstName { get; set; }

    [Required(ErrorMessage = "You must provide a last name")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "The field must contain only letters")]

    public string LastName { get; set; }

    [Remote("VerifyEmail", "Admin", ErrorMessage = "Email address already exists")]
    [Required(ErrorMessage = "You must provide a email address")]
    [EmailAddress(ErrorMessage = "You must provide a valid email address")]
    [UniqueEmail]

    public string Email { get; set; }

    [Required(ErrorMessage = "You must provide a password")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
    ErrorMessage = "Least 8 characters long, Digits, Uppercase letters, Lowercase letters and numbers")]

    public string Password { get; set; }

    [Required(ErrorMessage = "You must provide a password confirmation password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]

    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "You must provide the entered date")]
    [NotInFuture]

    public DateTime EnteredDate { get; set; }


    [Required(ErrorMessage = "You must provide the a faculty")]
    public int FacultyId { get; set; }

    public string FacultyName { get; set; }

}
