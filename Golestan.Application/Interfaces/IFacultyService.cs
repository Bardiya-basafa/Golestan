namespace Golestan.Application.Interfaces;

using Domain.Entities;
using DTOs;


public interface IFacultyService {

    Task<List<Faculty>> GetFaculties();

}
