namespace Golestan.Domain.Entities;

public class Course {

    public int Id { get; set; }

    public string Name { get; set; }

    public int Unit { get; set; }

    public string Description { get; set; }

    public DateTime ExamTime { get; set; }
    
    public int FacultyId { get; set; }

    public Faculty Faculty { get; set; }

    public ICollection<Section> Sections { get; set; }

}
