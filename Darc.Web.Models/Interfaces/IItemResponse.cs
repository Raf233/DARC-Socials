using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Web.Models.Interfaces
{
    public interface IItemResponse
    {
        public bool IsSuccessful { get; set; }

        object Item { get; }
    }
}
