using Darc.Models;
using Darc.Models.Domain;
using Darc.Models.Domain.File;
using Darc.Models.Domain.Requests;
using Darc.Models.Domain.Users;
using Darc.Services.Helpers;
using Darc.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Darc.Services.Services
{
    public class UserService : IUserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default");
        }

        public int Register(UserAddRequest model)
        {
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(model.Password, salt);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "[dbo].[Users_Insert]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                cmd.Parameters.AddWithValue("@LastName", model.LastName);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Username", model.Username);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@AvatarFileId", model.AvatarFileId);

                SqlParameter idParam = cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                idParam.Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                return (int)idParam.Value;

            }
        }

        public UserInfo GetUserProfile(int userId)
        {
            UserInfo user = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "[dbo].[Users_Select_ProfileById]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", userId);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int startingIndex = 0;

                        user = new UserInfo();
                        user.Id = reader.GetInt32(startingIndex++);
                        user.FirstName = reader.GetString(startingIndex++);
                        user.LastName = reader.GetString(startingIndex++);
                        user.Username = reader.GetString(startingIndex++);
                        if (reader.IsDBNull(startingIndex))
                        {
                            startingIndex = 7;
                        }
                        else
                        {
                            user.Avatar = new FilesInfo();
                            user.Avatar.Id = reader.GetInt32(startingIndex++);
                            user.Avatar.FileName = reader.GetString(startingIndex++);
                            user.Avatar.FileUrl = reader.GetString(startingIndex++);
                        }
                        user.DateCreated = reader.GetDateTime(startingIndex++);
                        user.Followers = reader.GetInt32(startingIndex++);
                        user.Following = reader.GetInt32(startingIndex++);
                        user.PostCount = reader.GetInt32(startingIndex++);
                        
                            
                    }
                }

            }

            return user;
        }

        public User GetById(int id)
        {
            User user = null;
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Users_Select_UserById]";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int startingIndex = 0;

                        user = new User();
                        user.Id = reader.GetInt32(startingIndex++);
                        user.FirstName = reader.GetString(startingIndex++);
                        user.LastName = reader.GetString(startingIndex++);
                        user.Email = reader.GetString(startingIndex++);
                        user.Username = reader.GetString(startingIndex++);
                        if (reader.IsDBNull(startingIndex))
                        {
                            startingIndex = 6;
                        }
                        else
                        {
                            user.AvatarFileId = reader.GetInt32(startingIndex++);
                        }
                        user.DateCreated = reader.GetDateTime(startingIndex++);
                    }
                }
            }
            return user;
        }

        public UserDetail GetUserById(int id)
        {
            UserDetail user = null;

            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "dbo.Users_Select_ById_V2";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = 0;
                        user = new UserDetail();
                        user.Id = reader.GetInt32(index++);
                        user.FirstName = reader.GetString(index++);
                        user.LastName = reader.GetString(index++);
                        user.Username = reader.GetString(index++);
                      
                        if (reader.IsDBNull(index))
                        {
                            index = 7;
                        }
                        else {
                            user.Avatar = new FilesInfo();
                            user.Avatar.Id = reader.GetInt32(index++);
                            user.Avatar.FileName = reader.GetString(index++);
                            user.Avatar.FileUrl = reader.GetString(index++);
                        }

                    }
                }
            }         
            return user;
        }

        public List<UserDetail> GetFollowers(int userId)
        {
            List<UserDetail> userList = null;
            UserDetail user = null;

            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "[dbo].[Users_SelectFollowers_ByUserId]";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    userList = new List<UserDetail>();
                    while (reader.Read())
                    {
                        int index = 0;
                        user = new UserDetail();
                        user.Id = reader.GetInt32(index++);
                        user.FirstName = reader.GetString(index++);
                        user.LastName = reader.GetString(index++);
                        user.Username = reader.GetString(index++);

                        if (reader.IsDBNull(index))
                        {
                            index = 7;
                        }
                        else
                        {
                            user.Avatar = new FilesInfo();
                            user.Avatar.Id = reader.GetInt32(index++);
                            user.Avatar.FileName = reader.GetString(index++);
                            user.Avatar.FileUrl = reader.GetString(index++);
                        }
                        userList.Add(user);
                    }
                }
                return userList;
            }
        }

        public List<UserDetail> GetFollowing(int userId)
        {
            List<UserDetail> userList = null;
            UserDetail user = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "[dbo].[Users_SelectFollowing_ByUserId]";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    userList = new List<UserDetail>();
                    while (reader.Read())
                    {
                        int index = 0;
                        user = MapUserDetail(reader, ref index);
                        userList.Add(user);
                    }
                }
                return userList;
            }
        }


        public UserAuth GetByEmail(string email)
        {
            UserAuth user = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Users_Select_ByEmail]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = 0;

                        user = new UserAuth();

                        user.Id = reader.GetInt32(index++);
                        user.Email = reader.GetString(index++);
                        user.Password = reader.GetString(index++);
                    }
                }

            }
            return user;
        }

        public UserAuth Login(LoginRequestModel model)
        {
            UserAuth currentUser = null;
            bool passwordsMatch = false;

            currentUser = GetByEmail(model.Email);

            if(currentUser != null)
            {
                passwordsMatch = BCrypt.BCryptHelper.CheckPassword(model.Password, currentUser.Password);
            }

            if(passwordsMatch == false)
            {
                currentUser = null;
            }
            return currentUser;
        }

        public List<UserDetail> GetUserByUsername(string userName)
        {
            List<UserDetail> userList = null;
            UserDetail user = null;
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Users_Select_User_BySearch]";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SearchQuery", userName);

                using(IDataReader reader = cmd.ExecuteReader())
                {
                    userList = new List<UserDetail>();
                    while (reader.Read())
                    {
                        int startingIndex = 0;
                        user = MapUserDetail(reader, ref startingIndex);
                        userList.Add(user);
                    }
                }
            }
            return userList;
        }

        public Paged<UserDetail> GetUsersNotFollowing(int pageIndex, int pageSize, int id)
        {
            Paged<UserDetail> pagedList = null;
            List<UserDetail> list = null;

            int totalCount = 0;

            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Users_Select_NonFollowing]";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@Id", id);

                using(IDataReader reader = cmd.ExecuteReader())
                {
                    UserDetail user = null;
                    while (reader.Read())
                    {
                        int index = 0;
                        user = MapUserDetail(reader, ref index); 
                        if(totalCount == 0)
                        {
                            totalCount = reader.GetInt32(index);
                        }
                        if(list == null)
                        {
                            list = new List<UserDetail>();
                        }

                        list.Add(user);
                        
                    }
                }
              
            }
            if (list != null)
            {
                pagedList = new Paged<UserDetail>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;

        }

        public void Update(UserUpdateRequest model, int id)
        {

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Users_Update]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                cmd.Parameters.AddWithValue("@LastName", model.LastName);
                cmd.Parameters.AddWithValue("@UserName", model.UserName);
                cmd.Parameters.AddWithValue("@AvatarFileId", model.AvatarFileId);
                cmd.Parameters.AddWithValue("@Id", id);



                cmd.ExecuteNonQuery();
            }
        }

        public bool CheckIfExists(string email, string userName)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Users_Select_ByEmailOrUsername]";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Username", userName);
                
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private UserDetail MapUserDetail(IDataReader reader, ref int index)
        {
            UserDetail user = new UserDetail();
            user.Id = reader.GetInt32(index++);
            user.FirstName = reader.GetString(index++);
            user.LastName = reader.GetString(index++);
            user.Username = reader.GetString(index++);

            if (reader.IsDBNull(index))
            {
                index = 7;
            }
            else
            {
                user.Avatar = new FilesInfo();
                user.Avatar.Id = reader.GetInt32(index++);
                user.Avatar.FileName = reader.GetString(index++);
                user.Avatar.FileUrl = reader.GetString(index++);
            }

            return user;
        }
    }
}
