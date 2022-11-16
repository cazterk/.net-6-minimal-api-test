using System;

namespace ChurchSystem.Models

{
    public class UserRegistration
    {

        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Everyone";



    }

}