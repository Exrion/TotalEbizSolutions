using Login_Test.Models;
using Login_Test.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Token_Test.Models;
using Token_Test.Services;

namespace Token_Test.Controllers
{
    [EnableCors("_allowedOrigins")]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbCtx;

        public UserController(AppDbContext appDbContext)
        {
            _dbCtx = appDbContext;
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminsEndpoint()
        {
            return Ok(Greeting());
        }

        [HttpGet("seller")]
        [Authorize(Roles = "Seller, Admin")]
        public IActionResult SellersEndpoint()
        {
            return Ok(Greeting());
        }

        [HttpGet("products")]
        [AllowAnonymous]
        public IActionResult ListProducts()
        {
            DbSet<Product> products = _dbCtx.Products;

            if (products == null)
            {
                return BadRequest("Resource empty or not found.");
            }

            return Ok(products);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Public")]
        public IActionResult Public()
        {
            return Ok("Public :)");
        }

        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    GivenName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                };
            }
            return null;
        }

        private string Greeting()
        {
            var curUser = GetCurrentUser();
            
            return $"Hi {curUser.GivenName}, you are an {curUser.Role}"; 
        }
    }
}
