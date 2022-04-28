using Darc.Web.Models.Interfaces;
using Darc.Web.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Darc.Web.Api.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected ILogger Logger { get; set; }
        public BaseApiController(ILogger logger)
        {
            logger.LogInformation($"Controller Firing {this.GetType().Name} ");
            Logger = logger;
        }

        protected OkObjectResult Ok200(BaseResponse response)
        {

            return base.Ok(response);
        }

        protected CreatedResult Created201(IItemResponse response)
        {
            string url = Request.Path + "/" + response.Item.ToString();

            return base.Created(url, response);
        }

        protected NotFoundObjectResult NotFound404(BaseResponse response)
        {
            return base.NotFound(response);
        }

        protected ObjectResult CustomResponse(HttpStatusCode code, BaseResponse response)
        {
            return StatusCode((int)code, response);
        }
    }
}
