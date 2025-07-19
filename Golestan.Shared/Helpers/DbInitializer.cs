namespace Golestan.Shared.Helpers;

using Constants;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;


public static class DbInitializer {

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
