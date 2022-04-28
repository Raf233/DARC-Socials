using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Models.Domain.Users
{
    public class UserInfo : UserDetail
    {
        public DateTime DateCreated { get; set; }
        public int Following { get; set; }
        public int Followers { get; set; }
        public int PostCount { get; set; }
    }
}
