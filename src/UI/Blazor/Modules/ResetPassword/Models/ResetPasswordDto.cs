﻿using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.ResetPassword.Models;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
    public string Token { get; set; }
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
    ErrorMessage = "Minimum length should be 6 characters\r\nPassword requirements: At least one uppercase letter (A-Z), one lowercase letter (a-z), and one non-alphanumeric character. ")]
    public string NewPassword { get; set; }
    [Compare("NewPassword", ErrorMessage = "NewPassword do not match with ConfirmPassword.")]
    public string ConfirmPassword { get; set; }


}
