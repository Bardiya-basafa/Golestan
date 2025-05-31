namespace Golestan.Domain.Entities;

public class Section {

    public int Id { get; set; }

    public int CourseId { get; set; }

    public int ClassroomId { get; set; }

    public int InstructorId { get; set; }

    public int TimeSlotId { get; set; }

    public Course Course { get; set; }

    public Classroom Classroom { get; set; }

    public Instructor Instructor { get; set; }

    public TimeSlot TimeSlot { get; set; }

    public ICollection<Student> Students { get; set; }

}
