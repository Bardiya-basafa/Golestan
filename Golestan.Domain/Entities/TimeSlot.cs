namespace Golestan.Domain.Entities;

public class TimeSlot {

    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public bool IsActive { get; set; }

    public int SectionId { get; set; }


    public Section Section { get; set; }


}
