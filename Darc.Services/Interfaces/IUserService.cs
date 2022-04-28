using Darc.Models.Domain.Users;
using Darc.Models.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using Darc.Models.Domain;
using Darc.Models;

namespace Darc.Services.Interfaces
{
    public interface IUserService
    {
        int Register(UserAddRequest model);

        UserAuth GetByEmail(string email);

        UserAuth Login(LoginRequestModel model);

        UserInfo GetUserProfile(int userId);

        User GetById(int id);

        UserDetail GetUserById(int id);

        void Update(UserUpdateRequest model, int id);

        List<UserDetail> GetFollowing(int userId);

        List<UserDetail> GetFollowers(int userId);

        Paged<UserDetail> GetUsersNotFollowing(int pageIndex, int pageSize, int id);

        List<UserDetail> GetUserByUsername(string userName);

        bool CheckIfExists(string email, string userName);
    }
}
