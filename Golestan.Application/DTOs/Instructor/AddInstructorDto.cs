namespace Golestan.Application.DTOs.Instructor;

using System.ComponentModel.DataAnnotations;


public class AddInstructorDto {

    [Required(ErrorMessage = "You must provide a first name")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "The field must contain only letters.")]

    public string FirstName { get; set; }

    [Required(ErrorMessage = "You must provide a last name")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "The field must contain only letters.")]

    public string LastName { get; set; }

    [Required(ErrorMessage = "You must provide a email address")]
    [EmailAddress(ErrorMessage = "You must provide a valid email address")]

    public string Email { get; set; }

    [Required(ErrorMessage = "You must provide a password")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]

    public string Password { get; set; }

    [Required(ErrorMessage = "You must provide a password confirmation password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]

    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "You must provide the hire date")]

    public DateTime HireDate { get; set; }

    [Required(ErrorMessage = "You must provide the salary")]

    public int Salary { get; set; }

    [Required(ErrorMessage = "You must provide the a faculty")]
    public int FacultyId { get; set; }


    public Dictionary<int, string>? FacultyOptions { get; set; }

}
