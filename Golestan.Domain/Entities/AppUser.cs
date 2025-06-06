namespace Golestan.Domain.Entities;

using Enums;
using Microsoft.AspNetCore.Identity;


public class AppUser : IdentityUser {

    // Common properties for all users
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public UserType UserType { get; set; }// Discriminator property

    public int InstructorId { get; set; }

    public int StudentId { get; set; }

    // Navigation properties 
    public Instructor InstructorProfile { get; set; }

    public Student StudentProfile { get; set; }

}
