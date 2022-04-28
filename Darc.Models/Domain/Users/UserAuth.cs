using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Models.Domain.Users
{
    public class UserAuth
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
