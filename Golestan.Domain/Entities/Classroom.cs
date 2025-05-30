namespace Golestan.Domain.Entities;

public class Classroom {

    public int Id { get; set; }

    public string Name { get; set; }

    public int Capacity { get; set; }
    
    public int FacultyId { get; set; }

    public Faculty Faculty { get; set; }

    public ICollection<Section> Sections { get; set; }

}
