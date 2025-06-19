namespace Golestan.Domain.Entities;

public class ExamResult {

    public int Id { get; set; }

    public decimal Score { get; set; }

    public string Description { get; set; } = String.Empty;

    public int TermId { get; set; }

    public int StudentId { get; set; }

    public int InstructorId { get; set; }

    public int CourseId { get; set; }


    public Term Term { get; set; }

    public Student Student { get; set; }


    public Instructor Instructor { get; set; }


    public Course Course { get; set; }

}
