using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NParks_Maven_II_AVS_ClassLibrary.Functions;

namespace NParks_Maven_II_AVS_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostalCodeConfigApiController : ControllerBase
    {
        #region Class Lib
        private readonly PostalCodeConfig? _postalCtx;
        public PostalCodeConfigApiController(PostalCodeConfig PostalCodeConfigCL)
        {
            _postalCtx = PostalCodeConfigCL;
        }

        #endregion

        private readonly string ENTITY = "nparks_postalcodeconfigs";
        
        [HttpGet]
        [Route("get")]
        public IActionResult getPostalCodeConfig()
        {
            string apiUrl = $"{ENTITY}";
            string data = _postalCtx!.getPostalCodeConfigs(apiUrl);
            
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest();
        }
    }
}
