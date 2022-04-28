using Darc.Models;
using Darc.Models.Domain;
using Darc.Models.Domain.Requests;
using Darc.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Darc.Services.Services
{
    public class LikeService : ILikeService
    {
        private readonly string _connectionString;

        public LikeService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default");
        }

        public void Add(LikeAddRequest model, int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "[dbo].[Likes_Insert]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PostId", model.PostId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                cmd.ExecuteNonQuery();
            }
        }

        public List<Like> GetLikesByPost(int postId)
        {
            List<Like> list = null;
            int totalCount = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.Likes_Select_ByPostId";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PostId", postId);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = 0;

                        Like like = new Like();

                        like.PostId = reader.GetInt32(index++);
                        like.UserId = reader.GetInt32(index++);
                        if (totalCount == 0)
                        {
                            totalCount = reader.GetInt32(index);
                        }

                        if (list == null)
                        {
                            list = new List<Like>();
                        }

                        list.Add(like);
                    }
                }
            }
            
            return list;
        }

        public Paged<Like> GetLikesByUser(int pageIndex, int pageSize, int userId)
        {
            Paged<Like> pagedList = null;
            List<Like> list = null;
            int totalCount = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.Likes_Select_ByUserId";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = 0;

                        Like like = new Like();

                        like.PostId = reader.GetInt32(index++);
                        like.UserId = reader.GetInt32(index++);
                        if (totalCount == 0)
                        {
                            totalCount = reader.GetInt32(index);
                        }

                        if (list == null)
                        {
                            list = new List<Like>();
                        }
                        list.Add(like);
                    }
                }
            }

            if (list != null)
            {
                pagedList = new Paged<Like>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public void DeleteByPost(int postId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Likes_Delete_ByPostId]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PostId", postId);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteByUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Likes_Delete_ByUserId]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteByUserAndPost(int userId, int postId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Likes_Delete_ByUser_and_ByPost]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@PostId", postId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
