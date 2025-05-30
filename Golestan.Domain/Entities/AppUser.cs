namespace Golestan.Domain.Entities;

using Enums;
using Microsoft.AspNetCore.Identity;


public class AppUser : IdentityUser {

    // Common properties for all users
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public UserType UserType { get; set; }// Discriminator property

    // Navigation properties 
    public Instructor ProfessorProfile { get; set; }

    public Student StudentProfile { get; set; }

}
