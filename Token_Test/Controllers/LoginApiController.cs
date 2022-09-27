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
using Microsoft.EntityFrameworkCore;
using Token_Test.Services;
using Microsoft.AspNetCore.Cors;

namespace Login_Test.Controllers
{
    [EnableCors("_allowedOrigins")]
    [Route("api/account")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {
        private IConfiguration _config;
        private AppDbContext _dbCtx;

        public LoginApiController(IConfiguration config, AppDbContext appDbContext)
        {
            _dbCtx = appDbContext;
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
        [Route("login")]
        public IActionResult Login(UserLoginDTO userDTO)
        {
            var user = Authenticate(userDTO);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("Username or Password incorrect");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserRegisterDTO newUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model invalid");
            }
            else
            {
                //Check for duplicate email
                User? dupeUserEmail = _dbCtx.Users
                    .Where(u => u.Email.ToLower().Equals(newUserDTO.Email.ToLower()))
                    .FirstOrDefault();

                //Check for duplicate username (case sensitive)
                User? dupeUserUsername = _dbCtx.Users
                    .Where(u => u.UserName.Equals(newUserDTO.UserName))
                    .FirstOrDefault();

                if (dupeUserEmail == null && dupeUserUsername == null)
                {
                    //Convert model
                    User newUser = new()
                    {
                        UserName = newUserDTO.UserName,
                        Email = newUserDTO.Email,
                        Password = newUserDTO.Password,
                        GivenName = newUserDTO.GivenName,
                        Surname = newUserDTO.Surname,
                        Role = newUserDTO.Role
                    };

                    //Retrieve dbset and save changes
                    DbSet<User> users = _dbCtx.Users;
                    users.Add(newUser);

                    switch (_dbCtx.SaveChanges())
                    {
                        case 0: 
                            return BadRequest("Unable to register");
                        case 1:
                            return Ok("Registration Successful");
                    }
                }
                else
                {
                    return BadRequest("Duplicate username and/or email");
                }

                return BadRequest("Unknown Error");
            }
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

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(UserLoginDTO userDTO)
        {
            User? curUser = _dbCtx.Users
                .Where(u => u.UserName == userDTO.UserName &&
                            u.Password == userDTO.Password)
                .FirstOrDefault();
            /*var curUser = UserConstants.Users.FirstOrDefault(
                o => o.UserName.ToLower() == userDTO.UserName.ToLower() &&
                o.Password == userDTO.Password);*/

            if (curUser != null)
            {
                return curUser;
            }

            return null;
        }
    }
}
