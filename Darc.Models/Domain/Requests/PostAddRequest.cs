using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Darc.Models.Domain.Requests
{
    public class PostAddRequest
    {
        [Required]
        [StringLength(144, MinimumLength = 2)]
        public string PostText { get; set; }
        [Required]
        public int CreatedBy { get; set; }

        public int? FileId { get; set; }

    }
}
