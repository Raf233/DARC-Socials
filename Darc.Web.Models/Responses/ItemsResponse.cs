using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Web.Models.Responses
{
    public class ItemsResponse<T> : SuccessResponse
    {
        public List<T> Items { get; set; }
    }
}
