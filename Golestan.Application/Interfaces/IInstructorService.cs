namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs.Instructor;


public interface IInstructorService {

    Task<List<InstructorsDto>> GetInstructors();

}
