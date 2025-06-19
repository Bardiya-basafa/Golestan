namespace Golestan.Domain.Entities;

public class Course {

    public int Id { get; set; }

    public string Name { get; set; }

    public int Unit { get; set; }

    public string Description { get; set; }

    public int ExamId { get; set; }

    public int FacultyId { get; set; }


    public Exam Exam { get; set; }

    public Faculty Faculty { get; set; }


    public ICollection<Section> Sections { get; set; } = new HashSet<Section>();

    public ICollection<Instructor> Instructors { get; set; } = new HashSet<Instructor>();

    public List<int> PrerequisiteCourses { get; set; } = new List<int>();

    public List<int> SiblingCourses { get; set; } = new List<int>();

}
