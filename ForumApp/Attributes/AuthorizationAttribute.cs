using ForumApp.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ForumApp.Attributes
{
    public class AuthorizationAttribute : TypeFilterAttribute
    {
        public AuthorizationAttribute() : base(typeof(AuthorizationFilter))
        {
        }
    }
}
