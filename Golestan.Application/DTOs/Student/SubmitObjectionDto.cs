namespace Golestan.Application.DTOs.Student;

using System.ComponentModel.DataAnnotations;


public class SubmitObjectionDto {

    public int StudentId { get; set; }

    public int ExamResultId { get; set; }

    [Required]
    public string Objection { get; set; }

}
