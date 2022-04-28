using Darc.Models;
using Darc.Models.Domain;
using Darc.Models.Domain.Requests;
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
    [Route("api/likes")]
    [ApiController]
    public class LikesApiController : BaseApiController
    {
        private ILikeService _service = null;

        private IHttpContextAccessor _httpContext;

        public LikesApiController(ILikeService service, IHttpContextAccessor httpContext, ILogger<LikesApiController> logger) : base(logger)
        {
            _service = service;
            _httpContext = httpContext;
        }

        [HttpPost]
        public ActionResult<SuccessResponse> Add(LikeAddRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            int idParsed = 0;
            var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentUser, out idParsed);

            try
            {
                _service.Add(model, idParsed);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [AllowAnonymous]
        [HttpGet("post/{postId:int}")]
        public ActionResult<ItemResponse<List<Like>>> GetByPost(int postId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                 List<Like> likeList = _service.GetLikesByPost(postId);

                if (likeList == null)
                {
                    code = 404;
                    response = new ErrorResponse("Likes of Post not found");
                }
                else
                {
                    response = new ItemResponse<List<Like>> { Item = likeList };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("user")]
        public ActionResult<ItemResponse<Paged<Like>>> GetByUser(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            int idParsed = 0;
            var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentUser, out idParsed);

            try
            {
                Paged<Like> page = _service.GetLikesByUser(pageIndex, pageSize, idParsed);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("User's likes not found");
                }
                else
                {
                    response = new ItemResponse<Paged<Like>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpDelete("post/{postId:int}")]
        public ActionResult<SuccessResponse> DeleteByPost(int postId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteByPost(postId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpDelete("user/{id:int}")]
        public ActionResult<SuccessResponse> DeleteByUser(int userId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteByUser(userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpDelete("post/{postId:int}/user")]
        public ActionResult<SuccessResponse> DeleteByUserAndPost(int postId)
        {
            int code = 200;
            BaseResponse response = null;

            int idParsed = 0;
            var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentUser, out idParsed);

            try
            {
                _service.DeleteByUserAndPost(idParsed, postId);

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
