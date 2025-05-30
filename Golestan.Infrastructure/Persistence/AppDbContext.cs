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

    public DbSet<Student> Students { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<Instructor> Instructors { get; set; }

    public DbSet<Faculty> Faculties { get; set; }

    public DbSet<TimeSlot> TimeSlots { get; set; }

    public DbSet<Classroom> Classrooms { get; set; }

    public DbSet<Section> Sections { get; set; }


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

        // Relations configurations 
        // Students
        modelBuilder.Entity<Student>()
            .HasOne(s => s.AppUser)
            .WithOne(au => au.StudentProfile)
            .HasForeignKey<Student>(s => s.AppUserId);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.Faculty)
            .WithMany(f => f.Students)
            .HasForeignKey(s => s.FacultyId);

        modelBuilder.Entity<Student>()
            .HasMany(s => s.Sections)
            .WithMany(s => s.Students)
            .UsingEntity(j => j.ToTable("StudentSections"));

        // Instructor 
        modelBuilder.Entity<Instructor>()
            .HasOne(s => s.AppUser)
            .WithOne(au => au.ProfessorProfile)
            .HasForeignKey<Instructor>(s => s.AppUserId);

        modelBuilder.Entity<Instructor>()
            .HasOne(s => s.Faculty)
            .WithMany(f => f.Instructors)
            .HasForeignKey(s => s.FacultyId);

        modelBuilder.Entity<Instructor>()
            .HasMany(i => i.Sections)
            .WithOne(s => s.Instructor)
            .HasForeignKey(s => s.InstructorId);


        // seed roles 
        modelBuilder.Entity<IdentityRole>().HasData(
        new IdentityRole { Name = AppRoles.Admin, NormalizedName = AppRoles.Admin.ToUpper() },
        new IdentityRole { Name = AppRoles.Student, NormalizedName = AppRoles.Student.ToUpper() },
        new IdentityRole { Name = AppRoles.Instructor, NormalizedName = AppRoles.Instructor.ToUpper() }
        );
    }

}
