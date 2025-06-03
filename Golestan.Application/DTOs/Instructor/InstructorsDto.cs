namespace Golestan.Application.DTOs.Instructor;

public class InstructorsDto {

    public int Id { get; set; }

    public string AppUserId { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public DateTime HireDate { get; set; }

    public int Salary { get; set; }

    public int FacultyId { get; set; }

    public string FacultyName { get; set; }

}
