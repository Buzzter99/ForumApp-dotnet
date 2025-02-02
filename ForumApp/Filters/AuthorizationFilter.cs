using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForumApp.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new { StatusCode = 200, Message = "Unauthorized. Please log in first." })
                {
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
        }
    }
}
