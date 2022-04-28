using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Services.Interfaces
{
    public interface IFollowerService
    {
        public void FollowUser(int userId, int followerId);

        public void UnfollowUser(int userId, int followerId);
    }
}
