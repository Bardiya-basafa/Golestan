namespace Golestan.Domain.Entities;

using Enums;


public class Student {

    public int Id { get; set; }

    public string AppUserId { get; set; }

    public AppUser AppUser { get; set; }

    public string FullName { get; set; }

    public string StudentNumber { get; set; }

    public DateTime EnteredDate { get; set; }

    public int FacultyId { get; set; }


    public Faculty Faculty { get; set; }


    public ICollection<Section> Sections { get; set; } = new HashSet<Section>();

    public ICollection<Course> PassedCourses { get; set; } = new HashSet<Course>();

    public ICollection<ExamResult> ExamResults { get; set; } = new HashSet<ExamResult>();

    public ICollection<Term> Terms { get; set; } = new HashSet<Term>();

}
