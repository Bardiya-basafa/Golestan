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
        // AppUser
        modelBuilder.Entity<AppUser>()
            .HasOne(a => a.StudentProfile)
            .WithOne(s => s.AppUser)
            .HasForeignKey<AppUser>(a => a.StudentId);

        modelBuilder.Entity<AppUser>()
            .HasOne(a => a.InstructorProfile)
            .WithOne(i => i.AppUser)
            .HasForeignKey<AppUser>(a => a.InstructorId);

        // Students
        modelBuilder.Entity<Student>()
            .HasOne(s => s.AppUser)
            .WithOne(au => au.StudentProfile)
            .HasForeignKey<Student>(s => s.AppUserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.Faculty)
            .WithMany(f => f.Students)
            .HasForeignKey(s => s.FacultyId)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Student>()
            .HasMany(s => s.Sections)
            .WithMany(s => s.Students)
            .UsingEntity(j => j.ToTable("StudentSections"));

        modelBuilder.Entity<Student>()
            .HasIndex(s => s.StudentNumber)
            .IsUnique();

        // Instructor 
        modelBuilder.Entity<Instructor>()
            .HasOne(s => s.AppUser)
            .WithOne(au => au.InstructorProfile)
            .HasForeignKey<Instructor>(s => s.AppUserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Instructor>()
            .HasOne(s => s.Faculty)
            .WithMany(f => f.Instructors)
            .HasForeignKey(s => s.FacultyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Instructor>()
            .HasMany(i => i.Sections)
            .WithOne(s => s.Instructor)
            .HasForeignKey(s => s.InstructorId)
            .OnDelete(DeleteBehavior.NoAction);

        // Course 
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Sections)
            .WithOne(s => s.Course)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Faculty)
            .WithMany(s => s.Courses)
            .HasForeignKey(s => s.FacultyId)
            .OnDelete(DeleteBehavior.NoAction);

        // Classroom
        modelBuilder.Entity<Classroom>()
            .HasOne(c => c.Faculty)
            .WithMany(f => f.Classrooms)
            .HasForeignKey(c => c.FacultyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Classroom>()
            .HasMany(c => c.Sections)
            .WithOne(s => s.Classroom)
            .HasForeignKey(s => s.ClassroomId)
            .OnDelete(DeleteBehavior.NoAction);

        // Time slot 
        modelBuilder.Entity<TimeSlot>()
            .HasOne(t => t.Section)
            .WithOne(s => s.TimeSlot)
            .HasForeignKey<TimeSlot>(t => t.SectionId)
            .OnDelete(DeleteBehavior.NoAction);

        // Section 
        modelBuilder.Entity<Section>()
            .HasOne(s => s.Classroom)
            .WithMany(c => c.Sections)
            .HasForeignKey(s => s.ClassroomId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Section>()
            .HasOne(s => s.Course)
            .WithMany(c => c.Sections)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Section>()
            .HasOne(s => s.Instructor)
            .WithMany(i => i.Sections)
            .HasForeignKey(s => s.InstructorId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Section>()
            .HasOne(s => s.TimeSlot)
            .WithOne(s => s.Section)
            .OnDelete(DeleteBehavior.NoAction);

        // Faculty
        modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Classrooms)
            .WithOne(c => c.Faculty)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Instructors)
            .WithOne(i => i.Faculty)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Students)
            .WithOne(s => s.Faculty)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Courses)
            .WithOne(c => c.Faculty)
            .OnDelete(DeleteBehavior.NoAction);


        // seed roles 
        modelBuilder.Entity<IdentityRole>().HasData(
        new IdentityRole { Name = AppRoles.Admin, NormalizedName = AppRoles.Admin.ToUpper() },
        new IdentityRole { Name = AppRoles.Student, NormalizedName = AppRoles.Student.ToUpper() },
        new IdentityRole { Name = AppRoles.Instructor, NormalizedName = AppRoles.Instructor.ToUpper() }
        );
    }

}
