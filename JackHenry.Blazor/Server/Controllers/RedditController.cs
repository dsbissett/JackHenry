using JackHenry.Blazor.Server.Hubs;
using JackHenry.Blazor.Server.Hubs.Clients;
using JackHenry.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace JackHenry.Blazor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedditController : ControllerBase
    {
        private readonly IRedditPostStreamer redditPostStreamer;
        private readonly IPostHub hub;


        public RedditController(IRedditPostStreamer redditPostStreamer, IPostHub hub)
        {
            this.redditPostStreamer = redditPostStreamer;
            this.hub = hub;
        }

        [HttpGet]
        public IActionResult Get()
        {
            this.hub.Noop();
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
