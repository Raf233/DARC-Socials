using Darc.Web.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Web.Models.Responses
{
    public class ItemResponse<T> : SuccessResponse, IItemResponse
    {
        public T Item { get; set; }

        object IItemResponse.Item { get { return this.Item; } }
    }
}
