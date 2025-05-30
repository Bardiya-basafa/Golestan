namespace Golestan.Domain.Entities;

public class Course {

    public int Id { get; set; }

    public string Name { get; set; }

    public int Unit { get; set; }

    public string Description { get; set; }

    public DateTime ExamTime { get; set; }

    public int SectionId { get; set; }

    public int FacultyId { get; set; }

    public Section Section { get; set; }

    public Faculty Faculty { get; set; }

}
