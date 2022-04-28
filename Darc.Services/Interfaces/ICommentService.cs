using Darc.Models.Domain;
using Darc.Models.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Services.Interfaces
{
    public interface ICommentService
    {
        int AddComment(CommentAddRequest model, int userId);

        Comment GetCommentById(int id);
        
        List<Comment> GetCommentsByPost(int PostId);
        
        void Delete(int id);
    }
}
