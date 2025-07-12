namespace Golestan.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using Enums;


public class Faculty {

    public int Id { get; set; }

    public string BuildingName { get; set; }

    [StringLength(maximumLength: 25)]
    public  string MajorName { get; set; }
    
    public int Budget { get; set; }

    public DateTime StartDate { get; set; }


    public ICollection<Student> Students { get; set; } = new HashSet<Student>();

    public ICollection<Instructor> Instructors { get; set; } = new HashSet<Instructor>();

    public ICollection<Course> Courses { get; set; } = new HashSet<Course>();

    public ICollection<Classroom> Classrooms { get; set; } = new HashSet<Classroom>();

}
