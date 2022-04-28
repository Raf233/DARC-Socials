using Darc.Models.Domain.Posts;
using Darc.Models.Domain.Requests;
using Darc.Models;
using Darc.Models.Domain;
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
    [Route("api/posts")]
    [ApiController]
    public class PostsApiController : BaseApiController
    {
        private IPostService _service = null;

        private IHttpContextAccessor _httpContext;

        public PostsApiController(IPostService service, IHttpContextAccessor httpContext, ILogger<PostsApiController> logger) : base(logger)
        {

            _service = service;
            _httpContext = httpContext;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(PostAddRequest model)
        {
            ObjectResult result = null;

            int idParsed = 0;
            var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentUser, out idParsed);

            try
            {
                int id = _service.Add(model, idParsed);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, result);
            }

            return result;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Post>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Post post = _service.GetById(id);

                if (post == null)
                {
                    code = 400;
                    response = new ErrorResponse("No Post found");
                }
                else
                {
                    response = new ItemResponse<Post>() { Item = post };
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

        [AllowAnonymous]
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Post>>> GetAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Post> page = _service.GetAll(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Post page not found");
                }
                else
                {
                    response = new ItemResponse<Paged<Post>> { Item = page };
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

        [AllowAnonymous]
        [HttpGet("user/{userId:int}")]
        public ActionResult<ItemResponse<Paged<Post>>> GetByCreatedBy(int pageIndex, int pageSize, int userId)
        {
            int code = 200;
            BaseResponse response = null;

            //int idParsed = 0;
            //var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            //int.TryParse(currentUser, out idParsed);

            try
            {
                Paged<Post> page = _service.GetByCreatedBy(pageIndex, pageSize, userId);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("User's post page not found");
                }
                else
                {
                    response = new ItemResponse<Paged<Post>> { Item = page };
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

        [HttpGet("likes/{userId:int}")]
        public ActionResult<ItemResponse<Paged<Post>>> GetLikedPosts(int pageIndex, int pageSize, int userId)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<Post> posts = _service.GetLikedPostsByUserId(pageIndex, pageSize, userId);
                if(posts == null)
                {
                    code = 404;
                    response = new ErrorResponse("No Liked Posts Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Post>> { Item = posts };
                }
            }catch( Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("feed")]
        public ActionResult<ItemResponse<Paged<PostInfo>>> GetPostFeedByUserId(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            int idParsed = 0;
            var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentUser, out idParsed);

            try
            {
                Paged<PostInfo> posts = _service.GetPostFeedByUserId(pageIndex, pageSize, idParsed);
                if(posts == null)
                {
                    code = 404;
                    response = new ErrorResponse("No Posts found");
                }
                else
                {
                    response = new ItemResponse<Paged<PostInfo>> { Item = posts };
                }
            }catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("random")]
        public ActionResult<ItemResponse<Paged<PostInfo>>> GetRandomByDate(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<PostInfo> page = _service.GetRandomByDate(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Post page not found");
                }
                else
                {
                    response = new ItemResponse<Paged<PostInfo>> { Item = page };
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

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

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
