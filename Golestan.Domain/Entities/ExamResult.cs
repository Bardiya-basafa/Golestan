namespace Golestan.Domain.Entities;

public class ExamResult {

    public int Id { get; set; }

    public decimal Score { get; set; }

    public string? Description { get; set; }

    public string Term { get; set; }

    public int StudentId { get; set; }

    public Student Student { get; set; }

    public int InstructorId { get; set; }

    public Instructor Instructor { get; set; }

    public int CourseId { get; set; }

    public Course Course { get; set; }

}
