namespace Golestan.Application.DTOs;

using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Enums;


public class AddFacultyDto {

    [Required(ErrorMessage = "Major name is required")]
    public string MajorName { get; set; }

    [Required(ErrorMessage = "Building name is required")]
    public string BuildingName { get; set; }

    [Required(ErrorMessage = "Faculty Number is required")]
    public int Badge { get; set; }

    [Required(ErrorMessage = "Faculty start date is required")]
    public DateTime StartDate { get; set; }


}
