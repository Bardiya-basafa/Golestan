namespace Golestan.Application.DTOs.Account;

using System.ComponentModel.DataAnnotations;


public class UniNumberLoginDto {

    [Required]
    public string UniNumber { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }

}
