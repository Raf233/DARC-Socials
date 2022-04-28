using Darc.Models.Domain;
using Darc.Models.Domain.File;
using Darc.Models.Domain.Requests;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Services.Interfaces
{
    public interface IFileService
    {
        FilesInfo AddFile(FileAddRequest model, int userId);

        FilesInfo Upload(IFormFile file, int userId);

    }
}
