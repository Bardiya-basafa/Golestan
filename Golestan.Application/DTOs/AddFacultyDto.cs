namespace Golestan.Application.DTOs;

using System.ComponentModel.DataAnnotations;
using Domain.Enums;


public class AddFacultyDto {

    [Required(ErrorMessage = "Faculty Name is required")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Faculty Name is between 5 and 50 characters")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Faculty Name must be alphabetical")]
    public string FacultyName { get; set; }

    [Required(ErrorMessage = "Major is required")]
    public string Major { get; set; }

    [Required(ErrorMessage = "Faculty Number is required")]
    public int Badge { get; set; }

    [Required(ErrorMessage = "Faculty Number is required")]
    public DateTime StartDate { get; set; }

}
