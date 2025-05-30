namespace Golestan.Infrastructure.Persistence;

using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;


public class AppDbContext : IdentityDbContext<AppUser> {

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // Custom table names
        modelBuilder.Entity<AppUser>().ToTable("Users");
        modelBuilder.Entity<IdentityRole<string>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        // seed roles 
        modelBuilder.Entity<IdentityRole>().HasData(
        new IdentityRole { Name = AppRoles.Admin, NormalizedName = AppRoles.Admin.ToUpper() },
        new IdentityRole { Name = AppRoles.Student, NormalizedName = AppRoles.Student.ToUpper() },
        new IdentityRole { Name = AppRoles.Instructor, NormalizedName = AppRoles.Instructor.ToUpper() }
        );
    }

}
