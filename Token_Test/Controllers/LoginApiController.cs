using Login_Test.ViewModels;
using Login_Test.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;

namespace Login_Test.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {
        private IConfiguration _config;

        public LoginApiController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ping")]
        public IActionResult ping()
        {
            return Ok("pong");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserDTO userDTO)
        {
            var user = Authenticate(userDTO);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private object Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.GivenName),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(UserDTO userDTO)
        {
            var curUser = UserConstants.Users.FirstOrDefault(
                o => o.UserName.ToLower() == userDTO.UserName.ToLower() &&
                o.Password == userDTO.Password);

            if (curUser != null)
            {
                return curUser;
            }

            return null;
        }
    }
}
