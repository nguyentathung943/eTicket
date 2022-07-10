using System.ComponentModel.DataAnnotations;

namespace eTickets.Data.ViewModels;

public class LoginVM
{
    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Email Address must be provided")]
    public string EmailAddress { get; set; }
    
    [Display(Name = "Password")]
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}