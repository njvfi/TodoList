using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoList.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoList.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IConfiguration _configuration;
        GoalContext db;

        public AccountController(IConfiguration configuration, GoalContext context)
        {
            _configuration = configuration;
            db = context;
        }

        #region Login
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(User user)
        {
            IQueryable<Models.User> users = db.Users;
            try
            {
                if (string.IsNullOrEmpty(user.Name) ||
                string.IsNullOrEmpty(user.Password))
                    return BadRequest("Username and/or Password not specified");
                if (user.Name.Equals("joydip") &&
                user.Password.Equals("joydip123"))
                {
                    var secretKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                    var signinCredentials = new SigningCredentials
                   (secretKey, SecurityAlgorithms.HmacSha256);
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: _configuration["Jwt:ValidIssuer"],
                        audience: _configuration["Jwt:ValidAudience"],
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
                        signingCredentials: signinCredentials
                    );
                    var token = new JwtSecurityTokenHandler().
                    WriteToken(jwtSecurityToken);
                    Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                    return View(user);
                }
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }
            return Unauthorized();
        }
        #endregion
    }
}
