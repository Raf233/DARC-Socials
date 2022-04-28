using Darc.Services.Interfaces;
using Darc.Web.Models.Responses;
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
    [Route("api/follow")]
    [ApiController]
    public class FollowApiController : BaseApiController
    {
        private IFollowerService _service = null;
        private IHttpContextAccessor _httpContext;

        public FollowApiController(IFollowerService service, IHttpContextAccessor httpContext, ILogger<FollowApiController> logger) : base(logger)
        {
            _service = service;
            _httpContext = httpContext;
        }

        [HttpPost("{userId:int}")]
        public ActionResult<SuccessResponse> FollowUser(int userId)
        {
            int code = 200;
            BaseResponse response = null;

            int idParsed = 0;
            var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentUser, out idParsed);

            try
            {
                _service.FollowUser(userId, idParsed);
                response = new SuccessResponse();
            } catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{userId:int}")]
        public ActionResult<SuccessResponse> UnfollowUser(int userId)
        {
            int code = 200;
            BaseResponse response = null;

            int idParsed = 0;
            var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentUser, out idParsed);

            try
            {
                _service.UnfollowUser(userId, idParsed);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }



    }
}
