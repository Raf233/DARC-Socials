using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Models.Domain.Posts
{
    public class PostInfo : Post
    {
        public int CommentCount { get; set; }
    }
}
