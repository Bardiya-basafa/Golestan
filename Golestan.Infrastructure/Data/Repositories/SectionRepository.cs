namespace Golestan.Infrastructure.Data.Repositories;

using Application.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;


public class SectionRepository(AppDbContext context) : ISectionRepository {

    public async Task<List<Section>> GetClassroomSections(int classroomId)
    {
        return await context.Sections
            .Where(s => s.ClassroomId == classroomId)
            .ToListAsync();
    }

}
