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


public class UserService(IUserRepository userRepository, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ITermService termService) : IUserService {

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
            UserType = UserType.Instructor
        };

        var instructor = new Instructor()
        {
            FullName = appUser.FirstName + " " + appUser.LastName,
            AppUser = appUser,
            HireDate = dto.HireDate,
            Salary = dto.Salary,
            FacultyId = dto.FacultyId
        };

        appUser.InstructorProfile = instructor;

        var result = await userManager.CreateAsync(appUser, dto.Password);

        if (result.Succeeded){
            var roleResult = await userManager.AddToRoleAsync(appUser, AppRoles.Instructor);

            if (!roleResult.Succeeded){
                finalResult.Message = roleResult.Errors.FirstOrDefault().Description;

                return finalResult;
            }


            return await userRepository.AddInstructor(instructor);
        }

        finalResult.Message = result.Errors.FirstOrDefault().Description;

        return finalResult;
    }

    public async Task<Result> RegisterNewStudent(AddStudentDto dto)
    {
        try{
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
                StudentNumber = await GetUniversalNumber(UserType.Student, dto.FacultyId)
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


            return await userRepository.AddStudent(studentProfile);
        }

        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    private async Task<string> GetUniversalNumber(UserType userType, int facultyId)
    {
        DateTime currentDate = DateTime.UtcNow;

        DateTime firstTermStart = new DateTime(currentDate.Year, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime firstTermEnd = new DateTime(currentDate.Year, 12, 31, 23, 59, 59, DateTimeKind.Utc);
        DateTime secondTermStart = new DateTime(currentDate.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime secondTermEnd = new DateTime(currentDate.Year, 5, 31, 23, 59, 59, DateTimeKind.Utc);
        var year = currentDate.Year.ToString();
        string term = "";

        if (currentDate >= firstTermStart && currentDate <= firstTermEnd){
            term = "1";
        }
        else if (currentDate >= secondTermStart && currentDate <= secondTermEnd){
            term = "2";
        }
        else{
            term = "2";
        }

        var finalResult = "";

        if (userType == UserType.Student){
            var studentCount = await userRepository.StudentCount(facultyId);
            string formattedCount = (studentCount + 1).ToString("D5");// Increment count for new student

            finalResult = $"s{year}{term}t{facultyId}f{formattedCount}";
        }
        else if (userType == UserType.Instructor){
            var instructorCount = await userRepository.InstructorCount(facultyId);
            string formattedCount = (instructorCount + 1).ToString("D5");// Increment count for new student

            finalResult = $"i{year}{term}t{facultyId}f{formattedCount}";
        }

        return finalResult;
    }

}
