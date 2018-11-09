﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQLiteAuthenticationTest.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }

    }
}
