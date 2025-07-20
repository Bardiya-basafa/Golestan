namespace Golestan.Application.Services;

using Domain.Entities;
using Domain.Enums;
using DTOs.Account;
using DTOs.Instructor;
using DTOs.Student;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using RepositoryInterfaces;
using Shared.Constants;
using Shared.Helpers;


public class UserService(IUserRepository userRepository, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ITermService termService, IStudentRepository studentRepository, IInstructorRepository instructorRepository, SignInManager<AppUser> signInManager) : IUserService {

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

    public async Task<Result> UniversalNumberLogin(UniNumberLoginDto model)
    {
        var result = new Result();
        var appUser = await userRepository.GetUserByUniversalNumber(model.UniNumber);

        if (appUser == null){
            result.Message = "Invalid UniNumber or Password";

            return result;
        }

        var resul = await signInManager.PasswordSignInAsync(appUser.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (!resul.Succeeded){
            result.Message = "Invalid UniNumber or Password";

            return result;
        }

        result.Succeeded = true;

        return result;
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

}
