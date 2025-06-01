namespace Golestan.Application.DTOs;

using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Enums;

// using Microsoft.AspNetCore.Mvc;


public class AddFacultyDto {

    [Required(ErrorMessage = "Major name is required")]

    // [Remote(controller: "Faculties", action: "VerifyMajor", ErrorMessage = "Major is already taken")]
    public string MajorName { get; set; }

    [Required(ErrorMessage = "Building name is required")]

    // [Remote(controller: "Faculties", action: "VerifyBuildingName", ErrorMessage = "Building is already taken")]
    public string BuildingName { get; set; }

    [Required(ErrorMessage = "Faculty Number is required")]
    public int Budget { get; set; }

    [Required(ErrorMessage = "Faculty start date is required")]
    public DateTime StartDate { get; set; }

}
