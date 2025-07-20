namespace Golestan.Infrastructure.Data.Repositories;

using Application.Interfaces;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class UserRepository(AppDbContext context, ITermService termService, UserManager<AppUser> userManager) : IUserRepository {

    public async Task<AppUser?> GetUserByUniversalNumber(string universalNumber)
    {
        var student = await context.Students
            .Where(s => s.StudentNumber == universalNumber)
            .Select(s => s.AppUser)
            .FirstOrDefaultAsync();

        if (student != null){
            return student;
        }

        var instructor = await context.Instructors
            .Where(i => i.InstructorNumber == universalNumber)
            .Select(i => i.AppUser)
            .FirstOrDefaultAsync();

        if (instructor != null){
            return instructor;
        }

        return null;
    }

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

    public async Task<bool> DeleteUser(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null){
            return false;
        }

        await userManager.DeleteAsync(user);

        return true;
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
