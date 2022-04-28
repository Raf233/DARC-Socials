using Darc.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Darc.Models.Domain
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string  Email { get; set; }

        public string Username { get; set; }

        public int? AvatarFileId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
