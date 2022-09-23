using Microsoft.AspNetCore.Mvc;

namespace Grocery_API.Controllers
{
    [ApiController]
    [Route("grocery")]
    public class Grocery : ControllerBase
    {
        [Route("list")]
        [HttpGet]
        public IActionResult getAllGroceries()
        {
            return Ok("");
        }
    }
}