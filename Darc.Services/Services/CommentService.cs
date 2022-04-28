using Darc.Models.Domain;
using Darc.Models.Domain.File;
using Darc.Models.Domain.Requests;
using Darc.Models.Domain.Users;
using Darc.Services.Helpers;
using Darc.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Darc.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly string _connectionString;

        public CommentService (IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default");
        }

        public int AddComment(CommentAddRequest model, int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "[dbo].[Comments_Insert_Comment]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Text", model.CommentText);
                cmd.Parameters.AddWithValue("@PostId", model.PostId);
                cmd.Parameters.AddWithValue("@CreatedBy", userId);
                cmd.Parameters.AddWithValue("@ParentId", model.ParentId);
                cmd.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);

                SqlParameter idParam = cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                idParam.Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                return (int)idParam.Value;

            }
        }

        public Comment GetCommentById(int id)
        {
            Comment comment = new Comment();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Comments_Select_ById]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = 0;

                        comment = MapComment(reader, ref index);
                    }
                }
            }
            return comment;
        }

        public List<Comment> GetCommentsByPost(int PostId)
        {

            List<Comment> list = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.Comments_Select_Comments_ByPostId";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PostId", PostId);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = 0;

                        Comment comment = MapComment(reader, ref index);

                        if(list == null)
                        {
                            list = new List<Comment>();
                        }
                        list.Add(comment);

                    }
                }
                if(list != null)
                {
                    foreach (var root in list)
                    {
                        root.Replies = list.Where(x => x.ParentId == root.Id).ToList();
                    }
                } else
                {
                    list = null;
                }
                
            }
            return list;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Comments_Delete_Comment]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CommentId", id);

                cmd.ExecuteNonQuery();
            }
        }

        private Comment MapComment(IDataReader reader, ref int index)
        {
            Comment comment = new Comment();

            comment.Id = reader.GetInt32(index++);
            comment.CommentText = reader.GetString(index++);
            comment.PostId = reader.GetInt32(index++);
            if (reader.IsDBNull(index))
            {
                index = 4;
            }
            else
            {
                comment.ParentId = reader.GetInt32(index++);
            }
            comment.CreatedBy = reader.GetInt32(index++);
            comment.DateCreated = reader.GetDateTime(index++);
            comment.IsDeleted = reader.GetBoolean(index++);
            comment.User = new UserDetail();
            comment.User.Id = reader.GetInt32(index++);
            comment.User.FirstName = reader.GetString(index++);
            comment.User.LastName = reader.GetString(index++);
            comment.User.Username = reader.GetString(index++);
            if (reader.IsDBNull(index))
            {
                index = 11;
            }
            else
            {
                comment.User.Avatar = new FilesInfo();
                comment.User.Avatar.Id = reader.GetInt32(index++);
                comment.User.Avatar.FileName = reader.GetString(index++);
                comment.User.Avatar.FileUrl = reader.GetString(index++);
            }
            return comment;
        }
    }
}
