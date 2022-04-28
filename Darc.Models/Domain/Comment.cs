using Darc.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Models.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int PostId { get; set; }
        public int? ParentId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public UserDetail User { get; set; }


        public List<Comment> Replies { get; set; }

    }
}
