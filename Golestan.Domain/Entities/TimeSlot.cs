namespace Golestan.Domain.Entities;

public class TimeSlot {

    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public int ClassroomId { get; set; }

    public int CourseId { get; set; }


    public Classroom Classroom { get; set; }

    public Course Course { get; set; }

}
