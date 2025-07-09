namespace Golestan.Infrastructure.Data.Repositories;

using Application.Interfaces;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class UserRepository(AppDbContext context, ITermService termService) : IUserRepository {

    public async Task<Result> AddInstructor(Instructor instructor)
    {
        context.Instructors.Add(instructor);
        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Instructor added"
        };
    }

    public async Task<Result> AddStudent(Student student)
    {
        context.Students.Add(student);
        var term = await termService.GetCurrentTermEntity();

        if (term != null){
            student.Terms.Add(term);
            context.Students.Update(student);
        }

        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Student added"
        };
    }

    public async Task<int> StudentCount(int facultyId)
    {
        return await context.Students
            .AsNoTracking()
            .Where(s => s.FacultyId == facultyId)
            .CountAsync();
    }

    public async Task<int> InstructorCount(int facultyId)
    {
        return await context.Instructors
            .AsNoTracking()
            .Where(s => s.FacultyId == facultyId)
            .CountAsync();
    }

}
