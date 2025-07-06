using System;
using System.ComponentModel.DataAnnotations;

public class UserCreateViewModel
{
    [Required]
    public string? Forename { get; set; }

    [Required]
    public string? Surname { get; set; }

    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

   

    public bool IsActive { get; set; }
}
