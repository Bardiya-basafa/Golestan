namespace Golestan.Application.DTOs.Score;

public class ScoreDto {

    public int StudentId { get; set; }

    public int InstructorId { get; set; }

    public int SectionId { get; set; }

    public int CourseId { get; set; }

    public decimal Score { get; set; } = decimal.Zero;

    public string Description { get; set; } = string.Empty;

    public string Term { get; set; } = string.Empty;

    public string StudentFullName { get; set; }

    public string StudentNumber { get; set; }

    public string Objection { get; set; }

}
