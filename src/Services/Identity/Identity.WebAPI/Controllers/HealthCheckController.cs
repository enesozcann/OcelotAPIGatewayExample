using Microsoft.AspNetCore.Mvc;

namespace Identity.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("I'm Alive!");
        }
    }
}