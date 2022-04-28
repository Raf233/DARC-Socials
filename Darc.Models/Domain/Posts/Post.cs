using Darc.Models.Domain.File;
using Darc.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Models.Domain.Posts
{
    public class Post
    {
        public int Id { get; set; }

        public string PostText { get; set; }

        public UserDetail User { get; set; }

        public FilesInfo Image { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
