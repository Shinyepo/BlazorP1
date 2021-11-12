using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int Bananas { get; set; } = 5000;
        public DateTime DateOfBirth { get; set; }
        public bool isConfirmed { get; set; }
        public bool isDeleted { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public DateTime LastLogin { get; set; } = DateTime.Now;

        public List<UserUnit> Units { get; set; }

        public int Battles { get; set; }
        public int Wins { get; set; }
        public int Defeats { get; set; }

    }
}
