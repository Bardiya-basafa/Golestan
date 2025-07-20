namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;
using Domain.Enums;
using DTOs.Section;
using Shared.Helpers;


public interface ISelectionRepository {

    Task<List<SectionDto>> GetAvailableSections(List<Course> availableCourses, Student student);

    Task<List<SectionDto>> GetSelectedSections(string studentId);

   

    Task<bool> IsStudentTimeTaken(string studentId, Section section);

    Task<bool> IsExamTimeTaken(string studentId, DateTime examDate, TimeSlot examTimeSlot);

}
