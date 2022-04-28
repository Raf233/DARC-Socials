using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Web.Models.Responses
{
    public class SuccessResponse : BaseResponse
    {
        public SuccessResponse()
        {
            this.IsSuccessful = true;
        }
    }
}
