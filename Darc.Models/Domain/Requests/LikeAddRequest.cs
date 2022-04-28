using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Darc.Models.Domain.Requests
{
    public class LikeAddRequest
    {
        [Required]
        public int PostId { get; set; }
    }
}
