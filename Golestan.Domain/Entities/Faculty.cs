namespace Golestan.Domain.Entities;

using Enums;


public class Faculty {

    public int Id { get; set; }

    public string Name { get; set; }

    public Major Major { get; set; }

    public int Badge { get; set; }

    public ICollection<Student> Students { get; set; }

    public ICollection<Instructor> Instructors { get; set; }

    public ICollection<Course> Courses { get; set; }

    public ICollection<Classroom> Classrooms { get; set; }

}
