using System.ComponentModel.DataAnnotations;

namespace eTickets.Data.ViewModels;

public class RegisterVM
{
    [Display(Name = "Full Name")]
    [Required(ErrorMessage = "Full Name must be provided")]
    public string FullName { get; set; }
    
    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Email Address must be provided")]
    public string EmailAddress { get; set; }
    
    [Display(Name = "Password")]
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Display(Name = "Confirm Password")]
    [Required(ErrorMessage = "Confirm must be provided")]
    [DataType(DataType.Password)]
    [Compare("Password",ErrorMessage = "Password do not match!")]
    public string ConfirmPassword { get; set; }
}