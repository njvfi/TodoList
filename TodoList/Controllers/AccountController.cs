using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoList.DAL.Entities;
using TodoList.DAL.Repositories;

namespace TodoList.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IConfiguration _configuration;
        private readonly GoalRepository _goalRepository;

        public AccountController(IConfiguration configuration, GoalRepository goalRepository)
        {
            _configuration = configuration;
            _goalRepository = goalRepository;
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
        public async Task<IActionResult> Login(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Email) ||
                string.IsNullOrEmpty(user.Password))
                    return BadRequest("Username and/or Password not specified");
                if(await _goalRepository.LoginAsync(user))
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
                    Response.Cookies.Append("UserId", user.Id.ToString());
                    return RedirectToAction("TaskList", "Home");
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
        #region Register
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            try
            {
                if (string.IsNullOrEmpty(user.Email) ||
                string.IsNullOrEmpty(user.Name) ||
                string.IsNullOrEmpty(user.Password))
                    return BadRequest("Заповніть всі поля");
                if (await _goalRepository.RegisterAsync(user))
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    user.Email = "Такий email вже існує";
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
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("X-Access-Token");

            return RedirectToAction("Index", "Home");
        }
    }
}
