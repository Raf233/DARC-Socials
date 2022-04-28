using Darc.Models;
using Darc.Models.Domain;
using Darc.Models.Domain.File;
using Darc.Models.Domain.Posts;
using Darc.Models.Domain.Requests;
using Darc.Models.Domain.Users;
using Darc.Services.Helpers;
using Darc.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Darc.Services.Services
{
    public class PostService : IPostService
    {
        private readonly string _connectionString;

        public PostService (IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default");
        }

        public int Add(PostAddRequest model, int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "[dbo].[Posts_Insert]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PostText", model.PostText);
                cmd.Parameters.AddWithValue("@CreatedBy", userId);
                cmd.Parameters.AddWithValue("@FileId", model.FileId);

                SqlParameter idParam = cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                idParam.Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                return (int)idParam.Value;
            }
        }

        public Post GetById(int id)
        {
            Post post = new Post();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Posts_GetById]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = 0;

                        post = MapPost(reader, ref index);
                    }
                }
            }
            return post;
        }

        public Paged<Post> GetAll(int pageIndex, int pageSize)
        {
            Paged<Post> pagedList = null;
            List<Post> list = null;
            int totalCount = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Posts_Select_All]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = 0;

                        Post post = MapPost(reader, ref index);

                        if (totalCount == 0)
                        {
                            totalCount = reader.GetInt32(index);
                        }

                        if (list == null)
                        {
                            list = new List<Post>();
                        }

                        list.Add(post);
                    }
                }

                if (list != null)
                {
                    pagedList = new Paged<Post>(list, pageIndex, pageSize, totalCount);
                }
                return pagedList;
            }
        }

        public Paged<Post> GetByCreatedBy(int pageIndex, int pageSize, int userId)
        {
            Paged<Post> pagedList = null;
            List<Post> list = null;
            int totalCount = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Posts_Select_ByCreatedBy]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@Id", userId);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = 0;

                        Post post = MapPost(reader, ref index);

                        if (totalCount == 0)
                        {
                            totalCount = reader.GetInt32(index);
                        }

                        if (list == null)
                        {
                            list = new List<Post>();
                        }

                        if (totalCount == 0)
                        {
                            totalCount = reader.GetInt32(index);
                        }

                        if (list == null)
                        {
                            list = new List<Post>();
                        }

                        list.Add(post);
                    }
                }

                if (list != null)
                {
                    pagedList = new Paged<Post>(list, pageIndex, pageSize, totalCount);
                }
                return pagedList;
            }
        }

        public Paged<PostInfo> GetRandomByDate(int pageIndex, int pageSize)
        {
            Paged<PostInfo> pagedList = null;
            List<PostInfo> list = null;
            int totalCount = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Posts_Select_All_Random]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = 0;

                        PostInfo post = MapPostDetails(reader, ref index);

                        if (totalCount == 0)
                        {
                            totalCount = reader.GetInt32(index);
                        }

                        if (list == null)
                        {
                            list = new List<PostInfo>();
                        }

                        list.Add(post);
                    }
                }
                if (list != null)
                {
                    pagedList = new Paged<PostInfo>(list, pageIndex, pageSize, totalCount);
                }
                return pagedList;
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Posts_Delete_ById]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }
        }

        public Paged<Post> GetLikedPostsByUserId(int pageIndex, int pageSize, int userId)
        {
            Paged<Post> pagedList = null;
            List<Post> list = null;
            int totalCount = 0;

            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Posts_SelectLikes_ByUserId]";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        int index = 0;

                        Post post = MapPostDetails(reader, ref index);

                        if (totalCount == 0)
                        {
                            totalCount = reader.GetInt32(index++);
                        }

                        if (list == null)
                        {
                            list = new List<Post>();
                        }
                        list.Add(post);
                    }
                   
                }
            }
            if (list != null)
            {
                pagedList = new Paged<Post>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<PostInfo> GetPostFeedByUserId(int pageIndex, int pageSize, int userId)
        {
            Paged<PostInfo> pagedList = null;
            List<PostInfo> list = null;
            int totalCount = 0;

            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Posts_SelectPostsFeed_ByUserId]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = 0;

                        PostInfo post = MapPostDetails(reader, ref index);

                        if(totalCount == 0)
                        {
                            totalCount = reader.GetInt32(++index);
                        }
                        if(list == null)
                        {
                            list = new List<PostInfo>();
                        }
                        list.Add(post);

                    }
                }
            }
            if (list != null)
            {
                pagedList = new Paged<PostInfo>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;

        }

        private PostInfo MapPostDetails(IDataReader reader, ref int index)
        {
            PostInfo post = new PostInfo();

            post.Id = reader.GetInt32(index++);
            post.PostText = reader.GetString(index++);
            if (reader.IsDBNull(index))
            {
                index = 9;
            }
            else
            {
                post.User = new UserDetail();
                post.User.Id = reader.GetInt32(index++);
                post.User.FirstName = reader.GetString(index++);
                post.User.LastName = reader.GetString(index++);
                post.User.Username = reader.GetString(index++);
                if (reader.IsDBNull(index))
                {
                    index = 9;
                }
                else
                {
                    post.User.Avatar = new FilesInfo();
                    post.User.Avatar.Id = reader.GetInt32(index++);
                    post.User.Avatar.FileName = reader.GetString(index++);
                    post.User.Avatar.FileUrl = reader.GetString(index++);
                }
            }
            if (reader.IsDBNull(index))
            {
                index = 12;
            }
            else
            {
                post.Image = new FilesInfo();
                post.Image.Id = reader.GetInt32(index++);
                post.Image.FileName = reader.GetString(index++);
                post.Image.FileUrl = reader.GetString(index++);
            }

            post.DateCreated = reader.GetDateTime(index++);
            post.CommentCount = reader.GetInt32(index);
            return post;
        }

        private Post MapPost(IDataReader reader, ref int index)
        {
            Post post = new Post();

            post.Id = reader.GetInt32(index++);
            post.PostText = reader.GetString(index++);
            if (reader.IsDBNull(index))
            {
                index = 9;
            }
            else
            {
                post.User = new UserDetail();
                post.User.Id = reader.GetInt32(index++);
                post.User.FirstName = reader.GetString(index++);
                post.User.LastName = reader.GetString(index++);
                post.User.Username = reader.GetString(index++);
                if (reader.IsDBNull(index))
                {
                    index = 9;
                }
                else
                {
                    post.User.Avatar = new FilesInfo();
                    post.User.Avatar.Id = reader.GetInt32(index++);
                    post.User.Avatar.FileName = reader.GetString(index++);
                    post.User.Avatar.FileUrl = reader.GetString(index++);
                }
            }
            if (reader.IsDBNull(index))
            {
                index = 12;
            }
            else
            {
                post.Image = new FilesInfo();
                post.Image.Id = reader.GetInt32(index++);
                post.Image.FileName = reader.GetString(index++);
                post.Image.FileUrl = reader.GetString(index++);
            }

            post.DateCreated = reader.GetDateTime(index++);
            return post;
        }
    }
}
