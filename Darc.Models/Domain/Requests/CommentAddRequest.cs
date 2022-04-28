using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Darc.Models.Domain.Requests
{
    public class CommentAddRequest
    {
        [Required]
        [StringLength(144, MinimumLength = 1)]
        public string CommentText { get; set; }
        [Required]
        public int PostId { get; set; }
        public int? ParentId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
