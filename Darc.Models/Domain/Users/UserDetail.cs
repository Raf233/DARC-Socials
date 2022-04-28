using Darc.Models.Domain.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Models.Domain.Users
{
    public class UserDetail
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public FilesInfo Avatar { get; set; }
    }
}
