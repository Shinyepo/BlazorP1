using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class UserRegister
    {
        public int Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [StringLength(16, ErrorMessage = "Maximum 16 characters")]
        [Required]
        public string Username { get; set; }
        public string Bio { get; set; }
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        [Required]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public int StartUnitId { get; set; }
        [Range(500,3000, ErrorMessage = "Please choose a number between 500 and 3000")]
        public int Bananas { get; set; } = 500;
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        [Range(typeof(bool),"true","true", ErrorMessage = "Only confirmed users can play!")]
        public bool isConfirmed { get; set; } = true;

    }
}
