using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Services.Helpers
{
    public static class FileHelper
    {

        public static string ConvertFileToString(byte[] data)
        {
            string fileToBase64Data = Convert.ToBase64String(data);
            return string.Format("data:image/jpg;base64,{0}", fileToBase64Data);
        }
    }
}
