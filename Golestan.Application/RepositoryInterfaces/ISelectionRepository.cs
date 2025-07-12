namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using Domain.Enums;
using DTOs.Section;
using Shared.Helpers;


public interface ISelectionRepository {

    Task<List<SectionDto>> GetAvailableSections(List<Course> availableCourses, Student student);

    Task<List<SectionDto>> GetSelectedSections(int studentId);

   

    Task<bool> IsStudentTimeTaken(int studentId, Section section);

    Task<bool> IsExamTimeTaken(int studentId, DateTime examDate, TimeSlot examTimeSlot);

}
