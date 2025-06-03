namespace Golestan.Domain.Entities;

public class Classroom {

    public int Id { get; set; }

    public string ClassNumber { get; set; }

    public int Capacity { get; set; }

    public int FacultyId { get; set; }

    public Faculty Faculty { get; set; }

    public ICollection<Section> Sections { get; set; } = new HashSet<Section>();

    public ICollection<TimeSlot> TimeSlots { get; set; } = new HashSet<TimeSlot>();

}
