namespace Golestan.Application.DTOs.Course;

public class CourseDetailsDto {

    public int Id { get; set; }

    public string Name { get; set; }

    public int Unit { get; set; }

    public string Description { get; set; }

    public DateTime ExamTime { get; set; }

    public int FacultyId { get; set; }

    public string FacultyName { get; set; }

    public int SectionsCount { get; set; }

}
