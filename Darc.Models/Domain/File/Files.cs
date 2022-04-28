using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Models.Domain
{
    public class Files
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
