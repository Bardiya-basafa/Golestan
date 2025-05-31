namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.EntityFrameworkCore;


public class FacultyService : IFacultyService {

    private readonly AppDbContext _context;

    public FacultyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Faculty>> GetFaculties()
    {
        var faculties = await _context.Faculties.ToListAsync();

        return faculties;
    }

   

}
