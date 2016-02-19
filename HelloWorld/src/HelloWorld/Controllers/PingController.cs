using Microsoft.AspNet.Mvc;

namespace HelloWorld.Controllers
{
    [Route("[controller]")]
    public class PingController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Success!");
        }
    }
}
