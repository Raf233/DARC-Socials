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
    [Route("api/comments")]
    [ApiController]
    public class CommentsApiController : BaseApiController
    {
        private ICommentService _service = null;

        private IHttpContextAccessor _httpContext;

        public CommentsApiController(ICommentService service, IHttpContextAccessor httpContext, ILogger<CommentsApiController> logger) : base(logger)
        {
            _service = service;
            _httpContext = httpContext;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> AddComment(CommentAddRequest model)
        {
            ObjectResult result = null;
            int idParsed = 0;
            var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentUser, out idParsed);

            try
            {
                int id = _service.AddComment(model, idParsed);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            } catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, result);
            }
            return result;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Comment>> GetCommentById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Comment comment = _service.GetCommentById(id);

                if (comment == null)
                {
                    code = 404;
                    response = new ErrorResponse("No comment found");
                }
                else
                {
                    response = new ItemResponse<Comment> { Item = comment };
                }
            } catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [AllowAnonymous]
        [HttpGet("post/{postId:int}")]
        public ActionResult<ItemResponse<List<Comment>>> GetCommentsByPost(int postId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Comment> commentList = _service.GetCommentsByPost(postId);

                if (commentList == null)
                {
                    code = 404;
                    response = new ErrorResponse("No Comments Found");
                } else
                {
                    response = new ItemResponse<List<Comment>> { Item = commentList };
                }
            } catch (Exception ex)
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
