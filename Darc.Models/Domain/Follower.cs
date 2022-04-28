using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Models.Domain
{
    public class Follower
    {
        public int UserId { get; set; }
        public int FollowerId { get; set; }
    }
}
