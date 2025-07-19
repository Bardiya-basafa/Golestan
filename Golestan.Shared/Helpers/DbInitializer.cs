namespace Golestan.Shared.Helpers;

using Constants;
using Domain.Entities;


public static class DbInitializer {

    public static async Task SeedRootAdmin(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // roles 
        if (!roleManager.Roles.Any()){
            foreach (var roleName in AppRoles.AllRoles){
                if (!await roleManager.RoleExistsAsync(roleName)){
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    
        var adminUser = await use
        // users with roles 
        // Root user 
        if (!userManager.Users.Any(u => u)){
            var password = "Bardiya1384$";
    
            var newUser = new User()
            {
                UserName = "bardiya",
                Email = "bardiya@gmail.com",
                EmailConfirmed = true,
                FullName = "bardiya basafa",
                ProfilePictureUrl = "https://img-b.udemycdn.com/user/200_H/16004620_10db_5.jpg"
            };
    
            var result = await userManager.CreateAsync(newUser, password);
    
            if (result.Succeeded){
                await userManager.AddToRoleAsync(newUser, AppRoles.User);
            }
    
            var newUserAdmin = new User()
            {
                UserName = "bardiyaAdmin",
                Email = "bardiyaAdmin@gmail.com",
                EmailConfirmed = true,
                FullName = "bardiya Admin",
                ProfilePictureUrl = "https://img-b.udemycdn.com/user/200_H/16004620_10db_5.jpg"
            };
    
            var resultAdmin = await userManager.CreateAsync(newUserAdmin, password);
    
            if (result.Succeeded){
                await userManager.AddToRoleAsync(newUserAdmin, AppRoles.Admin);
            }
        }
    }

}
