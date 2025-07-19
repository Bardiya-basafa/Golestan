namespace Golestan.Application.Services;

using Domain.Entities;
using Domain.Enums;
using DTOs.Instructor;
using DTOs.Student;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using RepositoryInterfaces;
using Shared.Constants;
using Shared.Helpers;


public class UserService(IUserRepository userRepository, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ITermService termService, IStudentRepository studentRepository, IInstructorRepository instructorRepository) : IUserService {

    private readonly RoleManager<IdentityRole> _roleManager = roleManager;


    public async Task<Result> RegisterNewInstructor(AddInstructorDto dto)
    {
        var finalResult = new Result();

        var appUser = new AppUser()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email,
            UserType = UserType.Instructor,
        };

        var instructor = new Instructor()
        {
            FullName = appUser.FirstName + " " + appUser.LastName,
            AppUser = appUser,
            HireDate = dto.HireDate,
            Salary = dto.Salary,
            FacultyId = dto.FacultyId,
            InstructorNumber = "N/A"
        };

        appUser.InstructorProfile = instructor;

        var result = await userManager.CreateAsync(appUser, dto.Password);

        if (!result.Succeeded){
            finalResult.Message = result.Errors.FirstOrDefault().Description;

            return finalResult;
        }


        var roleResult = await userManager.AddToRoleAsync(appUser, AppRoles.Instructor);

        if (!roleResult.Succeeded){
            finalResult.Message = roleResult.Errors.FirstOrDefault().Description;

            return finalResult;
        }


        instructor.InstructorNumber = await GetUniversalNumber(UserType.Instructor, dto.FacultyId, instructor.Id);
        var lastTerm = await termService.GetLastTerm();

        if (lastTerm != null){
            instructor.Terms.Add(lastTerm);
        }

        return await instructorRepository.UpdateInstructor(instructor);
    }

    public async Task<Result> RegisterNewStudent(AddStudentDto dto)
    {
        var finalResult = new Result();

        if (dto.Password != dto.ConfirmPassword){
            finalResult.Message = "Passwords don't match";

            return finalResult;
        }

        var appUser = new AppUser()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email,
            UserType = UserType.Student,
        };

        var studentProfile = new Student()
        {
            FullName = appUser.FirstName + " " + appUser.LastName,
            AppUser = appUser,
            FacultyId = dto.FacultyId,
            EnteredDate = DateTime.UtcNow,
            StudentNumber = "N/A"
        };

        appUser.StudentProfile = studentProfile;

        var result = await userManager.CreateAsync(appUser, dto.Password);

        if (!result.Succeeded){
            finalResult.Message = result.Errors.FirstOrDefault().Description;

            return finalResult;
        }

        var roleResult = await userManager.AddToRoleAsync(appUser, AppRoles.Student);

        if (!roleResult.Succeeded){
            finalResult.Message = roleResult.Errors.FirstOrDefault().Description;

            return finalResult;
        }


        studentProfile.StudentNumber = await GetUniversalNumber(UserType.Student, dto.FacultyId, studentProfile.Id);

        var lastTerm = await termService.GetLastTerm();

        if (lastTerm != null){
            studentProfile.Terms.Add(lastTerm);
        }

        return await studentRepository.UpdateStudent(studentProfile);
    }

    private async Task<string> GetUniversalNumber(UserType userType, int facultyId, int userId)
    {
        var lastTerm = await termService.GetLastTerm();
        var termIdentity = lastTerm?.TermIdentifier ?? "NA";

        var finalResult = "";

        if (userType == UserType.Student){
            string formattedCount = (userId).ToString("D6");// Increment count for new student

            finalResult = $"s-{termIdentity}-{facultyId}-{formattedCount}";
        }
        else if (userType == UserType.Instructor){
            string formattedCount = (userId).ToString("D6");// Increment count for new student

            finalResult = $"i-{termIdentity}-{facultyId}-{formattedCount}";
        }

        return finalResult;
    }

    public static async Task SeedRootAdmin(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Ensure all roles exist
        foreach (var roleName in AppRoles.AllRoles){
            if (!await roleManager.RoleExistsAsync(roleName)){
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Check if any admin user exists
        var adminUsers = await userManager.GetUsersInRoleAsync(AppRoles.Admin);

        // If no admin exists, create one
        if (!adminUsers.Any()){
            var adminUser = new AppUser()
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@golestan.com",
                UserName = "admin@golestan.com",
                UserType = UserType.Admin,
            };

            var password = "Bardiya1384$";
            var result = await userManager.CreateAsync(adminUser, password);

            if (result.Succeeded){
                await userManager.AddToRoleAsync(adminUser, AppRoles.Admin);

                // Optionally add to User role as well if needed
            }
            else{
                // Log errors if creation fails
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                throw new Exception($"Admin user creation failed: {errors}");
            }
        }
    }

}
