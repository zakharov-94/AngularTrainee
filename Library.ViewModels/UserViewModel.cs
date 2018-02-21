﻿using Shared.Enums;

namespace Library.ViewModels
{
    public class UserViewModel  
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
    }
}
