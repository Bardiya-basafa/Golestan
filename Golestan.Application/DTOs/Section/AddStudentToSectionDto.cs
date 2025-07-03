namespace Golestan.Application.DTOs.Section;

using System.ComponentModel.DataAnnotations;
using Student;


public class AddStudentToSectionDto {

    [Required(ErrorMessage = "Must provide at least one student")]
    public List<int> StudentIds { get; set; }

    public List<StudentDetailsDto> Students { get; set; }

    public int SectionId { get; set; }

}
