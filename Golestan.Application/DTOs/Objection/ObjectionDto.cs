namespace Golestan.Application.DTOs.Objection;

using System.ComponentModel.DataAnnotations;


public class ObjectionDto {

    public int StudentId { get; set; }

    public int ExamResultId { get; set; }

    [Required]
    public string Objection { get; set; }

}
