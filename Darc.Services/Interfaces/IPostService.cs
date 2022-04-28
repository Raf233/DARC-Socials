using Darc.Models;
using Darc.Models.Domain;
using Darc.Models.Domain.Posts;
using Darc.Models.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Services.Interfaces
{
    public interface IPostService
    {
        int Add(PostAddRequest model, int userId);

        Post GetById(int id);

        Paged<Post> GetAll(int pageIndex, int pageSize);

        Paged<Post> GetByCreatedBy(int pageIndex, int pageSize, int userId);

        Paged<PostInfo> GetRandomByDate(int pageIndex, int pageSize);

        void Delete(int id);

        Paged<PostInfo> GetPostFeedByUserId(int pageIndex, int pageSize, int userId);

        Paged<Post> GetLikedPostsByUserId(int pageIndex, int pageSize, int userId);
    }
}
