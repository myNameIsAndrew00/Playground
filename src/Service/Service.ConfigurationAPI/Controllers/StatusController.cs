using Microsoft.AspNetCore.Mvc;
using Service.ConfigurationAPI.Response;

namespace Service.ConfigurationAPI.Controllers
{
    [ApiController]
    [Route("status")]
    public class StatusController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return this.StandardResponse(true);
        }
    }
}
