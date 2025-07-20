namespace Golestan.Application.DTOs.Account;

using System.ComponentModel.DataAnnotations;


public class ForgetPasswordDto {

    [Required]
    [EmailAddress]
    public string Email { get; set; }

}
