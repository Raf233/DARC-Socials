using Darc.Models.Domain.File;
using Darc.Services.Interfaces;
using Darc.Web.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Darc.Web.Api.Controllers
{
    [Authorize]
    [Route("api/files")]
    [ApiController]
    public class FileApiController : BaseApiController
    {
        private IFileService _service;

        private IHttpContextAccessor _httpContext;

        public FileApiController(IFileService service, IHttpContextAccessor httpContext, ILogger<FileApiController> logger) : base(logger)
        {
            _service = service;
            _httpContext = httpContext;

        }

        [HttpPost()]
        public ActionResult<ItemResponse<FilesInfo>> Upload(IFormFile file)
        {
            ObjectResult result = null;
            int idParsed = 0;
            var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentUser, out idParsed);

            try
            {
                FilesInfo newFile = _service.Upload(file, idParsed);
                ItemResponse<FilesInfo> response = new ItemResponse<FilesInfo>() { Item = newFile };
                result = Created201(response);
            }catch(Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, result);
            }
            return result;
        }
    }
}
