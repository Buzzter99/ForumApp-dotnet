using ForumApp.Models;
using ForumApp.Attributes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        [AuthorizationLoggedIn]
        public async Task<IActionResult> Login([FromBody]LoginUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse { StatusCode = 400, Message = string.Join("", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage))) });
            }
            var requestedUser = await _userManager.FindByEmailAsync(user.Email);
            if (requestedUser == null)
            {
                return Ok(new ApiResponse { StatusCode = 401, Message = "Invalid username or password!"});
            }
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var result = await _signInManager.PasswordSignInAsync(requestedUser, user.Password, true, false);
            if (!result.Succeeded)
            {
                return Ok(new ApiResponse { StatusCode = 401, Message = "Wrong Email Or Password!" });
            }
            return Ok(new ApiResponse { StatusCode = 200, Message="Logged in succesfully!"});
        }
        [HttpPost("Register")]
        [AllowAnonymous]
        [AuthorizationLoggedIn]
        public async Task<IActionResult> Register([FromBody] RegisterUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse { StatusCode = 400, Message = string.Join("", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)))});
            }
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return Ok(new ApiResponse { StatusCode = 409, Message = "User with that email already exists!" });
            }
            var existingUserByUsername = await _userManager.FindByNameAsync(user.UserName);
            if (existingUserByUsername != null)
            {
                return Ok(new ApiResponse { StatusCode = 409, Message = "Username Already Exists!" });
            }
            var newUser = new IdentityUser
            {
                Email = user.Email,
                UserName = user.UserName
            };
            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            if (isCreated.Succeeded)
            {
                return Ok(new ApiResponse { StatusCode = 201, Message = "User Created Successfully!" });
            } 
            return Ok(new ApiResponse { StatusCode = 500, Message = string.Join("",isCreated.Errors.Select(c => c.Description)) });
        }
        [HttpGet("IsLoggedIn")]
        [AllowAnonymous]
        public async Task<IActionResult> IsLoggedIn()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Ok(new ApiResponse { StatusCode = 401, Message = "Unauthorized" });
            }
            return Ok(new ApiResponse { StatusCode = 200, Message = $"Welcome {user.UserName}" });
        }
        [HttpPost("Logout")]
        [Authorization]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new ApiResponse { StatusCode = 200, Message = "Logged out successfully!" });
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }
    }
}
