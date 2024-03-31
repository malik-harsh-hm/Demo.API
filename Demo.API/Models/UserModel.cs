﻿using Microsoft.AspNetCore.Identity;

namespace Demo.API.Models
{
    public class UserModel: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
