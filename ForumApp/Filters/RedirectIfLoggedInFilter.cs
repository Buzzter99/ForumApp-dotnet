using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForumApp.Filters
{
    public class RedirectIfLoggedInFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user != null && user.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new { StatusCode = 401, Message = "Cannot access this endpoint when logged in!" })
                {
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
        }
    }
}
