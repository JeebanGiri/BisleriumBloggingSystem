using Domain.BisleriumBloggingSystem.Entities;
using Insfrastructure.BisleriumBloggingSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.BisleriumBloggingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public record LoginResponse(bool Flag, string Token, string Role, string Message);
        public record UserSession(string? Id, string? Name, string? Email, string? Role);


        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _signInManager = signInManager;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(Register model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Password don't Match");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser { UserName = model.Email, FullName = model.FullName, Email = model.Email };

            // Check if the specified role exists
            var roleExists = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
            {
                // If the role doesn't exist, return error
                return BadRequest("Invalid role specified.");
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign the specified role to the user
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok("User registered successfully.");
            }
            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                // User is not authenticated
                return Unauthorized();
            }

            // Retrieve the user from the database using the UserManager
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // User not found
                return NotFound("User not found");
            }

            // Return the user
            return Ok(user);
        }

        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers()
        {
            List<ViewSubAdmin> usersWithRoles = new List<ViewSubAdmin>();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.FirstOrDefault() == "Admin")
                {   
                    var userWithRole = new ViewSubAdmin
                    {
                        UserId = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        Role = roles.FirstOrDefault()
                    };
                    usersWithRoles.Add(userWithRole);
                }
            }
                
            return Ok(usersWithRoles);
        }


        [HttpDelete("delete-users")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("Succeeded");
            }
            return BadRequest(result.Errors);
        }

        [HttpPut("edit-profile")]
        public async Task<IActionResult> EditProfile(AppUser appUser)
        {
            var user = await _userManager.FindByIdAsync(appUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.PhoneNumber = appUser.PhoneNumber;
            user.UserName = appUser.Email;
            user.Email = appUser.Email;
            user.NormalizedEmail = appUser.Email.ToUpper();
            user.EmailConfirmed = appUser.EmailConfirmed;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(user);
            }
            return BadRequest();
        }

        [Authorize(Roles = "Blogger, Admin")]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Check if the old password matches the user's current password
            var passwordCheckResult = await _userManager.CheckPasswordAsync(user, model.OldPassword);
            if (!passwordCheckResult)
            {
                // Old password doesn't match
                ModelState.AddModelError("OldPassword", "The old password is incorrect.");
                return BadRequest(ModelState);
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
            if (changePasswordResult.Succeeded)
            {
                return Ok(new { message = "Password changed successfully" });
            }

            return BadRequest(changePasswordResult.Errors);
        }

        [HttpPost]
        [Route("login-user")]
        public async Task<IActionResult> Login([FromBody] Login loginUser)
        {
            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {

                var getUser = await _userManager.FindByEmailAsync(loginUser.Email);
                var getUserRole = await _userManager.GetRolesAsync(getUser);
                var userSession = new UserSession(getUser.Id, getUser.UserName, getUser.Email, getUserRole.First());
                string token = GenerateToken(userSession);


                ///* return new LoginResponse(true, token!, "Login Successfully!")*/;
                return Ok(new LoginResponse(true, token!, getUserRole.First(), "Login Successfully!"));
            }
            else
            {
                //return new LoginResponse(false, null!, "Failed to Login to system.");
                return BadRequest("Invalid email or password!");
            }
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
