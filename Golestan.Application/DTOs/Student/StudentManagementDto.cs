namespace Golestan.Application.DTOs.Student;

public class StudentManagementDto {

    public string FacultyName { get; set; }

    public int FacultyId { get; set; }

    public List<StudentDetailsDto> Students { get; set; }

}
