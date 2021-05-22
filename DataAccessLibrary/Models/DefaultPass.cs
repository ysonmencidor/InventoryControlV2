using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Models
{
    public class DefaultPass
    {
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter new password.")]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match.")]
        public string RepeatPassword { get; set; }
    }
}
