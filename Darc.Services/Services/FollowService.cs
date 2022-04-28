using Darc.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Darc.Services.Services
{
    public class FollowService : IFollowerService
    {
        private readonly string _connectionString = null;

        public FollowService( IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default"); 
        }

        public void FollowUser(int userId, int followerId)
        {
            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.Followers_Insert_Follower";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@FollowerId", followerId);

                cmd.ExecuteNonQuery();
            }
        }

        public void UnfollowUser(int userId, int followerId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.Followers_DeleteFollower";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@FollowerId", followerId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
