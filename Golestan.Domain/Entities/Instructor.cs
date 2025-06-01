namespace Golestan.Domain.Entities;

using Enums;


public class Instructor {

    public int Id { get; set; }

    public string AppUserId { get; set; }// foreign key to AppUser

    public AppUser AppUser { get; set; }


    public DateTime HireDate { get; set; }

    public int Salary { get; set; }

    public int FacultyId { get; set; }


    public Faculty Faculty { get; set; }


    public ICollection<Section> Sections { get; set; } = new HashSet<Section>();

}
