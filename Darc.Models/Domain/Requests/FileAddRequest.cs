using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Darc.Models.Domain.Requests
{
    public class FileAddRequest
    {
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string FileName { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string FileUrl { get; set; }
        [Required]
        public int CreatedBy { get; set; }
    }
}
