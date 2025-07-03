namespace Golestan.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using Enums;
using Microsoft.EntityFrameworkCore;


public class Exam {

    public int Id { get; set; }

    public TimeSlot TimeSlot { get; set; }

    public DateTime ExamDateTime { get; set; }


    public int CourseId { get; set; }

    public int ClassroomId { get; set; }

    public int TermId { get; set; }

    public Course Course { get; set; }

    public Classroom Classroom { get; set; }

    public Term Term { get; set; }

}
