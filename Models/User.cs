using System.ComponentModel.DataAnnotations;
using System;

namespace ChurchSystem.Models
{

    public class User
    {
        [Key]
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = "Everyone";
        public bool IsActive { get; set; }

        public User(string userName, string name, string password, string role)
        {

            UserName = userName;
            Name = name;
            Password = password;
            Role = role;

        }

    }
}