using FlightFinderApi.Data;
using FlightFinderApi.Models;
using FlightFinderApi.Services.Interfaces;
using FlightFinderApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwTokenController : ControllerBase
    {
        private IConfiguration _config;
        public readonly FlightFinderDbContext _context;
        private readonly IUserService _userService;
        public JwTokenController(IConfiguration config, FlightFinderDbContext context, IUserService userService)
        {
            _config = config;
            _context = context;
            _userService = userService;
        }

        //[AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] JwtUserRequestDto userLogin)
        {
            try
            {
                var user = Authenticate(userLogin);

                if (user != null)
                {
                    var token = Generate(user);
                    return Ok(token);
                }

                return NotFound("User not found");
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);

            }
        }

        [NonAction]
        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [NonAction]
        private User Authenticate(JwtUserRequestDto userLogin)
        {
            var currentUser = _userService.VerifyUser(userLogin.UserName, userLogin.Password).Result;

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
    }
}

    
    
    

