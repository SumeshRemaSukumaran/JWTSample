using JWTModels.Dto;
using JWTSample.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;

namespace JWTSample.Filters
{
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        private readonly int unAuthorized = 401;

        public string Key { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var _resourceAccessProvider =(IResourceAccessProvider) serviceProvider.GetService(typeof(IResourceAccessProvider));
            var _config = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));           
            var requestToken = context.HttpContext.Request.Headers["Authorization"].ToString();
            if(string.IsNullOrEmpty(requestToken))
            {
                context.HttpContext.Response.StatusCode = unAuthorized;
                context.Result = new JsonResult(new { HttpStatusCode.Unauthorized });
                return;
            }
            requestToken = requestToken.Substring("bearer ".Length);
            var tokenManagementDto = _config.GetSection("TokenManagement").Get<TokenManagementDto>();
            var isValidToken = _resourceAccessProvider.ValidateToken(requestToken, tokenManagementDto, true);

            if(!isValidToken)
            {
                context.HttpContext.Response.StatusCode = unAuthorized;
                context.Result = new JsonResult(new { HttpStatusCode.Unauthorized });
            }
        }        
    }
}
