using ForumApp.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ForumApp.Attributes
{
    public class AuthorizationLoggedInAttribute : TypeFilterAttribute
    {
        public AuthorizationLoggedInAttribute() : base(typeof(RedirectIfLoggedInFilter)) { }
    }
}
