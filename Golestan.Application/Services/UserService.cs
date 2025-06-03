namespace Golestan.Application.Services;

using Domain.Entities;
using Domain.Enums;
using DTOs.Instructor;
using Infrastructure.Persistence;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Constants;
using Shared.Helpers;


public class UserService : IUserService {

    private readonly UserManager<AppUser> _userManager;

    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly AppDbContext _context;

    public UserService(UserManager<AppUser> userManager, AppDbContext context, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Result> RegisterNewInstructor(AddInstructorDto dto)
    {
        var finalResult = new Result();

        var appUser = new AppUser()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserType = UserType.Instructor
        };

        var result = await _userManager.CreateAsync(appUser, dto.Password);

        if (result.Succeeded){
            var roleResult = await _userManager.AddToRoleAsync(appUser, AppRoles.Instructor);

            if (!roleResult.Succeeded){
                finalResult.Message = roleResult.Errors.FirstOrDefault().Description;

                return finalResult;
            }

            var instructor = new Instructor()
            {
                AppUser = appUser,
                HireDate = dto.HireDate,
                Salary = dto.Salary,
                FacultyId = dto.FacultyId
            };

            appUser.InstructorProfile = instructor;
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();
            finalResult.Succeeded = true;
            finalResult.Message = "Instructor created";

            return finalResult;
        }

        finalResult.Message = result.Errors.FirstOrDefault().Description;

        return finalResult;
    }

}
