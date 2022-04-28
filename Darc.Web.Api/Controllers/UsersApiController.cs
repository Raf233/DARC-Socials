using Darc.Models;
using Darc.Models.Domain;
using Darc.Models.Domain.Requests;
using Darc.Models.Domain.Users;
using Darc.Services.Interfaces;
using Darc.Web.Models.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    [Route("api/users")]
    [ApiController]
    public class UsersApiController : BaseApiController
    {
        private IUserService _service = null;
        private IHttpContextAccessor _httpContext;
        private IJwtService _JwtService;

        public UsersApiController(IUserService service, IJwtService jwtService,IHttpContextAccessor httpContext, ILogger<UsersApiController> logger) : base(logger)
        {
            _service = service;
            _httpContext = httpContext;
            _JwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<ItemResponse<int>> Register(UserAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                bool exists = _service.CheckIfExists(model.Email, model.Username);
                if (exists)
                {
                    ErrorResponse response = new ErrorResponse("email or username already taken.");
                    result = StatusCode(409, response);
                }
                else
                {
                    int id = _service.Register(model);
                    ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                    result = Created201(response);
                }
            } catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }

            return result;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<ItemResponse<bool>> Login(LoginRequestModel model) {

            int code = 200;
            BaseResponse response = null;

            try
            {
                UserAuth currentUser = null;
                currentUser = _service.Login(model);

                if (currentUser != null)
                {
                    string tokenString = _JwtService.GenerateJwtToken(currentUser);
                    _httpContext.HttpContext.Response.Cookies.Append("authorization", tokenString, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });
                    //response = new ItemResponse<string>() { Item = tokenString };
                    response = new SuccessResponse();

                } else
                {
                    code = 404;
                    response = new ErrorResponse("Incorrect Email or Password");
                }
            } catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);

        }
        [HttpGet("search")]
        public ActionResult<ItemResponse<List<UserDetail>>> GetUserByUsername(string searchQuery)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<UserDetail> userList = _service.GetUserByUsername(searchQuery);
                if(userList == null)
                {
                    code = 404;
                    response = new ErrorResponse("No Users found.");
                }
                else
                {
                    response = new ItemResponse<List<UserDetail>> { Item = userList };
                }
            }catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("profileinfo/{userId:int}")]
        public ActionResult<ItemResponse<UserInfo>> GetUserProfile(int userId)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                UserInfo userProfile = _service.GetUserProfile(userId);

                if (userProfile == null)
                {
                    code = 404;
                    response = new ErrorResponse("No user found.");
                } else
                {
                    response = new ItemResponse<UserInfo> { Item = userProfile };
                }


            } catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("followers/{userId:int}")]
        public ActionResult<ItemResponse<List<UserDetail>>> GetFollowers(int userId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<UserDetail> user = _service.GetFollowers(userId);
                if(user == null)
                {
                    code = 404;
                    response = new ErrorResponse("No Followers Found.");
                } else
                {
                    response = new ItemResponse<List<UserDetail>> { Item = user };
                }
            }catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("following/{userId:int}")]
        public ActionResult<ItemResponse<List<UserDetail>>> GetFollowing(int userId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<UserDetail> user = _service.GetFollowing(userId);
                if (user == null)
                {
                    code = 404;
                    response = new ErrorResponse("No Followers Found.");
                }
                else
                {
                    response = new ItemResponse<List<UserDetail>> { Item = user };
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

        [HttpGet("recommended")]
        public ActionResult<ItemResponse<Paged<UserDetail>>> GetUsersNotFollowing(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            int idParsed = 0;
            var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentUser, out idParsed);

            try
            {
                Paged<UserDetail> user = _service.GetUsersNotFollowing(pageIndex, pageSize, idParsed);

                if(user == null)
                {
                    code = 404;
                    response = new ErrorResponse("No users found.");
                } else
                {
                    response = new ItemResponse<Paged<UserDetail>> { Item = user };
                }
            }catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("current")]
        public ActionResult<ItemResponse<User>> GetCurrentUser()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
       
                int idParsed = 0;
                var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                int.TryParse(currentUser, out idParsed);

                User user = _service.GetById(idParsed);

                if(user == null)
                {
                    code = 404;
                    response = new ErrorResponse("No user found");
                }
                else
                {
                    response = new ItemResponse<User>() { Item = user };
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

        [HttpGet("current/new")]
        public ActionResult<ItemResponse<UserDetail>> GetCurrentUserV2()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {

                int idParsed = 0;
                var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                int.TryParse(currentUser, out idParsed);

                UserDetail user = _service.GetUserById(idParsed);

                if (user == null)
                {
                    code = 404;
                    response = new ErrorResponse("No user found");
                }
                else
                {
                    response = new ItemResponse<UserDetail>() { Item = user };
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

        [HttpGet("logout")]
        public async Task<ActionResult<SuccessResponse>> Logout()
        {
            _httpContext.HttpContext.Response.Cookies.Delete("authorization");
            SuccessResponse response = new SuccessResponse();
            return Ok200(response);
        }

        [HttpPut("updateuser")]
        public ActionResult<SuccessResponse> UpdateUser(UserUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int idParsed = 0;
                var currentUser = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                int.TryParse(currentUser, out idParsed);

                _service.Update(model, idParsed);

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
