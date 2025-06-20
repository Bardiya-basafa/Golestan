namespace Golestan.Application.DTOs.Instructor;

public class SubmitScoreDto {

    public string Description { get; set; }

    public int StudentId { get; set; }

    public int InstructorId { get; set; }

    public int SectionId { get; set; }

    public int CourseId { get; set; }

    public decimal Score { get; set; }

}
