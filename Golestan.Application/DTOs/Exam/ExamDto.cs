namespace Golestan.Application.DTOs.Exam;

using Classroom;
using Course;
using Domain.Enums;
using Term;


public class ExamDto {

    public int Id { get; set; }

    public TimeSlot TimeSlot { get; set; }

    public DateTime ExamDateTime { get; set; }


    public CourseDto Course { get; set; }

    public ClassroomDto Classroom { get; set; }

    public TermDto Term { get; set; }

}
