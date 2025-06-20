namespace Golestan.Domain.Entities;

using Enums;


public class Section {

    public int Id { get; set; }

    public int CourseId { get; set; }

    public int ClassroomId { get; set; }

    public int InstructorId { get; set; }

    public int TermId { get; set; }


    public Course Course { get; set; }

    public Classroom Classroom { get; set; }

    public Instructor Instructor { get; set; }

    public Term Term { get; set; }

    public TimeSlot TimeSlot { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public ICollection<Student> Students { get; set; } = new HashSet<Student>();

    public ICollection<ExamResult> ExamResults { get; set; } = new HashSet<ExamResult>();

}
