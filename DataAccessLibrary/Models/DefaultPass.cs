using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Models
{
    public class DefaultPass
    {
        public string Username { get; set; }

        //[Required(ErrorMessage = "Please enter old password.")]
        public string OldPasss { get; set; }

        [MinLength(6, ErrorMessage = "The new password field must be at least 6 characters long")]
        [MaxLength(30, ErrorMessage = "The new password filed must not exceed 30 characters.")]
        [Required(ErrorMessage = "Please enter new password.")]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match.")]
        public string RepeatPassword { get; set; }
    }
}
