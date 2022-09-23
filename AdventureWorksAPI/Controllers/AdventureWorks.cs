using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdventureWorks : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("ping")]
        public IActionResult ping()
        {
            return Ok("pong");
        }
    }
}