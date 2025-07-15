namespace Golestan.Application.DTOs.Score;

public class ScoreDto {

    public int StudentId { get; set; }

    public int InstructorId { get; set; }

    public int SectionId { get; set; }

    public int CourseId { get; set; }

    public decimal Score { get; set; } = -1;

    public string Description { get; set; } = string.Empty;


    

    public string Objection { get; set; } 

}
