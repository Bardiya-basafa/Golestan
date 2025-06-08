namespace Golestan.Application.DTOs.Classroom;

public class FacultiesClassroomsDto {

    public string FacultyName { get; set; }

    public int FacultyId { get; set; }


    public List<ClassroomDetailsDto> Classrooms { get; set; }

}
