namespace Golestan.Application.DTOs.Classroom;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


public class AddClassroomDto {

    [Required(ErrorMessage = "Class number is required")]
    [RegularExpression(@"^\d{3}$", ErrorMessage = "Class number should be 3 digits")]
    [Remote(action: "VerifyClassNumber", controller: "Classrooms", AdditionalFields = "FacultyId", ErrorMessage = "Class number already exists")]

    public string ClassNumber { get; set; }

    [Required(ErrorMessage = "Class capacity is required")]
    [Range(10, 150, ErrorMessage = "Class capacity should be between 10 and 150 ")]
    public int Capacity { get; set; }

    [Required]
    public int FacultyId { get; set; }

    public string FacultyName { get; set; }

}
