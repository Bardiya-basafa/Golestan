namespace Golestan.Domain.Entities;

using Enums;


public class Instructor {

    public int Id { get; set; }

    public string AppUserId { get; set; }

    public AppUser AppUser { get; set; }

    public string FullName { get; set; }

    public DateTime HireDate { get; set; }

    public int Salary { get; set; }

    public int FacultyId { get; set; }


    public Faculty Faculty { get; set; }


    public ICollection<Section> Sections { get; set; } = new HashSet<Section>();

    public ICollection<Course> Courses { get; set; } = new HashSet<Course>();

    public ICollection<ExamResult> ExamResults { get; set; } = new HashSet<ExamResult>();

}
