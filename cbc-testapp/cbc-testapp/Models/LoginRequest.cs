﻿using System;
using System.Collections.Generic;
using System.Text;

namespace cbc_testapp.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
