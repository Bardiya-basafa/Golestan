namespace Golestan.Application.DTOs.Course;

public class CourseManagementDto {

    public int FacultyId { get; set; }

    public string FacultyName { get; set; }

    public List<CourseDetailsDto> Courses { get; set; }

}
