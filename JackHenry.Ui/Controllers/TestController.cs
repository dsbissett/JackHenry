using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JackHenry.Ui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return this.Ok();
        }
    }
}
