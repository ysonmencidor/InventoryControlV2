using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class UserModel : Roles
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is Required")]
        public string Username { get; set; }

        public string Password { get; set; } = "Defaultpassword";

        [Required(ErrorMessage = "Name is Required"),MaxLength(150, ErrorMessage = "You have reached maximum limit of characters allowed (150)")]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public string AccessType { get; set; } = "Grouped";
    }

}