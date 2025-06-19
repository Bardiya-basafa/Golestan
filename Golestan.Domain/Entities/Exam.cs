namespace Golestan.Domain.Entities;

using Enums;


public class Exam {

    public int Id { get; set; }
    
    public TimeSlot TimeSlot { get; set; }

    public DateTime ExamDateTime { get; set; }

    public int CourseId { get; set; }

    public int ClassroomId { get; set; }

    public Course Course { get; set; }

    public Classroom Classroom { get; set; }

}
