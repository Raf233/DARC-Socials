using Darc.Models;
using Darc.Models.Domain;
using Darc.Models.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Services.Interfaces
{
    public interface ILikeService
    {
        void Add(LikeAddRequest model, int userId);

        List<Like> GetLikesByPost(int postId);

        Paged<Like> GetLikesByUser(int pageIndex, int pageSize, int userId);

        void DeleteByPost(int postId);

        void DeleteByUser(int userId);

        void DeleteByUserAndPost(int userId, int postId);
    }
}
