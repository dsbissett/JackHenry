using JackHenry.Data;
using JackHenry.Ui.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace JackHenry.Ui.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedditController : ControllerBase, IRedditController
    {
        private readonly IRedditPostStreamer redditPostStreamer;

        public RedditController(IRedditPostStreamer redditPostStreamer, IPostHub postHub)
        {
            this.redditPostStreamer = redditPostStreamer;
        }

        [HttpGet]
        public IActionResult Get()
        {
            this.redditPostStreamer.Run();
            return this.Ok();
        }

        [HttpPost]
        public IActionResult Post()
        {
            this.redditPostStreamer.Stop();
            return this.Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            // I chose to add a flush database method only
            // because I didn't have a need for any other
            // async controller actions so I wanted to show one.
            await this.redditPostStreamer.FlushDatabaseAsync();
            
            return this.Ok();
        }
    }
}
