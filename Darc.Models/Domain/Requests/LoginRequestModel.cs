﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Darc.Models.Domain.Requests
{
    public class LoginRequestModel
    {
        [Required]
        //[EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
